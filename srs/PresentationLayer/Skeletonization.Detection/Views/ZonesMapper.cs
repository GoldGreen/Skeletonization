using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Skeletonization.PresentationLayer.Detection.Views
{
    public class ZonesMapper : Control
    {
        public ObservableCollection<Zone> Zones
        {
            get => (ObservableCollection<Zone>)GetValue(ZonesProperty);
            set => SetValue(ZonesProperty, value);
        }

        public Zone SelectedZone
        {
            get => (Zone)GetValue(SelectedZoneProperty);
            set => SetValue(SelectedZoneProperty, value);
        }

        public DataTemplatesCollection DataTemplates
        {
            get => (DataTemplatesCollection)GetValue(DataTemplatesProperty);
            set => SetValue(DataTemplatesProperty, value);
        }

        public static readonly DependencyProperty ZonesProperty =
            DependencyProperty.Register(nameof(Zones),
                                        typeof(ObservableCollection<Zone>),
                                        typeof(ZonesMapper),
                                        new PropertyMetadata(ZonesChangedCallback));

        public static readonly DependencyProperty SelectedZoneProperty =
            DependencyProperty.Register(nameof(SelectedZone),
                                        typeof(Zone),
                                        typeof(ZonesMapper));

        public static readonly DependencyProperty DataTemplatesProperty =
          DependencyProperty.Register(nameof(DataTemplates),
                                      typeof(DataTemplatesCollection),
                                      typeof(ZonesMapper),
                                      new PropertyMetadata(new DataTemplatesCollection()));

        private IDisposable _sub;
        private readonly Dictionary<Zone, (IEnumerable<FrameworkElement> elements, IDisposable sub)> _zonesData = new();

        static ZonesMapper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZonesMapper), new FrameworkPropertyMetadata(typeof(ZonesMapper)));
        }

        private static void ZonesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ZonesMapper).ZonesChanged();
        }

        private void ZonesChanged()
        {
            _sub?.Dispose();

            var add = Zones.ToObservable(NotifyCollectionChangedAction.Add)
                             .Do(AddZone);

            var remove = Zones.ToObservable(NotifyCollectionChangedAction.Remove)
                                .Do(RemoveZone);

            _sub = add.Merge(remove)
                                   .Subscribe();
        }

        private void AddZone(Zone zone)
        {
            var visualElements = DataTemplates.Select(x => x.LoadContent())
                                              .Cast<FrameworkElement>()
                                              .ToList();

            var parent = Parent as Panel;

            foreach (var visualElement in visualElements)
            {
                visualElement.DataContext = zone;
                parent.Children.Add(visualElement);
            }

            var selectedZoneSub = visualElements.Select(el => el.MouseDownObservable())
                                                .Merge()
                                                .Select(_ => zone)
                                                .Subscribe(zone => SelectedZone = zone);

            _zonesData.Add(zone, (visualElements, selectedZoneSub));
        }

        private void RemoveZone(Zone zone)
        {
            var (elements, sub) = _zonesData[zone];

            var parent = Parent as Panel;
            foreach (var visualEl in elements)
            {
                parent.Children.Remove(visualEl);
            }

            sub.Dispose();

            _zonesData.Remove(zone);

            if (SelectedZone == zone)
            {
                SelectedZone = null;
            }
        }
    }
}
