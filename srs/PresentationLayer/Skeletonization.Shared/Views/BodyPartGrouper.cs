using ReactiveUI;
using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;

namespace Skeletonization.PresentationLayer.Shared.Views
{
    public class BodyPartGrouper : ControlBase
    {
        public IEnumerable<Selectable<BodyPart>> BodyParts
        {
            get => (IEnumerable<Selectable<BodyPart>>)GetValue(BodyPartsProperty);
            set => SetValue(BodyPartsProperty, value);
        }

        public IEnumerable<Selectable<string>> GroupedBodyParts
        {
            get => (IEnumerable<Selectable<string>>)GetValue(GroupedBodyPartsProperty);
            private set => SetValue(GroupedBodyPartsProperty, value);
        }

        public static readonly DependencyProperty BodyPartsProperty =
            DependencyProperty.Register(nameof(BodyParts), typeof(IEnumerable<Selectable<BodyPart>>), typeof(BodyPartGrouper), new PropertyMetadata(PartsChangedCallback));


        public static readonly DependencyProperty GroupedBodyPartsProperty =
            DependencyProperty.Register(nameof(GroupedBodyParts), typeof(IEnumerable<Selectable<string>>), typeof(BodyPartGrouper));


        private static readonly Dictionary<string, IEnumerable<BodyPart>> _textToBodyParts = new()
        {
            { "Голова", new[] { BodyPart.Head } },
            {
                "Руки",
                new[]
                {
                    BodyPart.RightShoulder, BodyPart.RightElbow, BodyPart.RightWrist,
                    BodyPart.LeftShoulder, BodyPart.LeftElbow, BodyPart.LeftWrist
                }
            },
            {
                "Ноги",
                new[]
                {
                    BodyPart.RightHip, BodyPart.RightKnee, BodyPart.RightAnkle,
                    BodyPart.LeftHip, BodyPart.LeftKnee, BodyPart.LeftAnkle
                }
            },
            {
                "Тело",
                new[]
                {
                    BodyPart.Neck, BodyPart.LeftShoulder, BodyPart.RightShoulder,
                    BodyPart.Hip, BodyPart.LeftHip, BodyPart.RightHip
                }
            }
        };
        private static readonly Dictionary<BodyPart, IEnumerable<string>> _bodyPartToTexts = new();


        private IDisposable _sub;
        static BodyPartGrouper()
        {
            _bodyPartToTexts = _textToBodyParts
                                    .SelectMany(x => x.Value
                                                      .Select(y => (name: x.Key, bodyPart: y)))
                                    .GroupBy(x => x.bodyPart)
                                    .ToDictionary
                                    (
                                        x => x.Key,
                                        x => x.Select(x => x.name)
                                               .ToList()
                                               .AsEnumerable()
                                    );


            DefaultStyleKeyProperty.OverrideMetadata(typeof(BodyPartGrouper), new FrameworkPropertyMetadata(typeof(BodyPartGrouper)));
        }

        private static void PartsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BodyPartGrouper).PartsChanged();
        }

        private void PartsChanged()
        {
            _sub?.Dispose();

            if (BodyParts is null)
            {
                GroupedBodyParts = null;
                return;
            }

            GroupedBodyParts = _textToBodyParts.Select(x =>
                            (name: x.Key,
                            parts: BodyParts.Where(y => _bodyPartToTexts[y.Value].Contains(x.Key))))
                            .Select(x => new Selectable<string>(x.name)
                            {
                                IsSelected = x.parts.All(x => x.IsSelected)
                            }).ToList();

            CompositeDisposable sub = new();
            foreach (var groupedPart in GroupedBodyParts)
            {
                sub.Add(groupedPart.WhenAnyValue(x => x.IsSelected)
                                     .Subscribe(_ => RecalculateSelection()));
            }
            _sub = sub;
        }

        private void RecalculateSelection()
        {
            foreach (var part in GroupedBodyParts.SelectMany(x => _textToBodyParts[x.Value].Select(y => (part: y, isSelected: x.IsSelected)))
                                                 .GroupBy(x => x.part)
                                                 .Select(x => (part: x.Key, isSelected: x.Select(x => x.isSelected).Max())))
            {
                BodyParts.First(x => x.Value == part.part).IsSelected = part.isSelected;
            }
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _sub?.Dispose();
        }
    }
}
