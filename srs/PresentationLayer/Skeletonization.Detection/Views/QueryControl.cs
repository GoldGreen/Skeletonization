using ReactiveUI;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Reactive;
using Skeletonization.PresentationLayer.Shared.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace Skeletonization.PresentationLayer.Detection.Views
{
    public class QueryControl : ControlBase
    {
        public Query Query
        {
            get => (Query)GetValue(QueryProperty);
            set => SetValue(QueryProperty, value);
        }

        public static readonly DependencyProperty QueryProperty =
            DependencyProperty.Register(nameof(Query), typeof(Query), typeof(QueryControl));

        public ObservableCollection<Zone> Zones
        {
            get => (ObservableCollection<Zone>)GetValue(ZonesProperty);
            set => SetValue(ZonesProperty, value);
        }

        public static readonly DependencyProperty ZonesProperty =
            DependencyProperty.Register(nameof(Zones), typeof(ObservableCollection<Zone>), typeof(QueryControl));

        public IEnumerable<Zone> AddingZones
        {
            get => (IEnumerable<Zone>)GetValue(AddingZonesProperty);
            private set => SetValue(AddingZonesProperty, value);
        }

        public static readonly DependencyProperty AddingZonesProperty =
            DependencyProperty.Register(nameof(AddingZones), typeof(IEnumerable<Zone>), typeof(QueryControl));

        public Zone SelectedZone
        {
            get => (Zone)GetValue(SelectedZoneProperty);
            set => SetValue(SelectedZoneProperty, value);
        }

        public static readonly DependencyProperty SelectedZoneProperty =
            DependencyProperty.Register(nameof(SelectedZone), typeof(Zone), typeof(QueryControl));

        public ICommand RemoveQueryCommand
        {
            get => (ICommand)GetValue(RemoveQueryCommandProperty);
            set => SetValue(RemoveQueryCommandProperty, value);
        }

        public static readonly DependencyProperty RemoveQueryCommandProperty =
            DependencyProperty.Register(nameof(RemoveQueryCommand), typeof(ICommand), typeof(QueryControl));

        public ICommand AddZoneCommand
        {
            get => (ICommand)GetValue(AddZoneCommandProperty);
            set => SetValue(AddZoneCommandProperty, value);
        }

        public static readonly DependencyProperty AddZoneCommandProperty =
            DependencyProperty.Register(nameof(AddZoneCommand), typeof(ICommand), typeof(QueryControl));

        public ICommand RemoveZoneCommand
        {
            get => (ICommand)GetValue(RemoveZoneCommandProperty);
            set => SetValue(RemoveZoneCommandProperty, value);
        }

        public static readonly DependencyProperty RemoveZoneCommandProperty =
            DependencyProperty.Register(nameof(RemoveZoneCommand), typeof(ICommand), typeof(QueryControl));


        static QueryControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QueryControl), new FrameworkPropertyMetadata(typeof(QueryControl)));
        }

        private IDisposable _sub;

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            _sub = Zones.ToObservable()
                 .Merge(Query.Zones.ToObservable())
                 .Select(_ => Unit.Default)
                 .Merge(Observable.Return(Unit.Default))
                 .Select(_ => Zones.Where(x => !Query.Zones.Contains(x)))
                 .Select(x => x.ToList())
                 .Subscribe(x => AddingZones = x);

            AddZoneCommand = ReactiveCommand.Create
            (
                () =>
                {
                    Query.Zones.Add(SelectedZone);
                    SelectedZone = null;
                },
                this.WhenAnyValue(x => x.SelectedZone).Select(s => s != null)
            );

            RemoveZoneCommand = ReactiveCommand.Create<Zone>
            (
                z => Query.Zones.Remove(z)
            );
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _sub.Dispose();
        }
    }
}
