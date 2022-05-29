using Prism.Events;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace Skeletonization.PresentationLayer.Shared.Reactive
{
    public static class ObservableExtensionsMethods
    {
        private static readonly List<IDisposable> _disposables = new();

        public static IDisposable Cashe(this IDisposable disposable)
        {
            _disposables.Add(disposable);
            return disposable;
        }

        public static IObservable<T> RecieveBlock<T>(this IObservable<T> observable, TimeSpan timeSpan)
        {
            return observable.Take(1)
                             .Merge(Observable.Empty<T>()
                                              .Delay(timeSpan))
                             .Repeat();
        }

        public static IObservable<(NotifyCollectionChangedAction action, T el)> ToObservable<T>(this ObservableCollection<T> collection)
        {
            return Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
            (
                act => (sender, e) => act(e),
                act => collection.CollectionChanged += act,
                act => collection.CollectionChanged -= act
            )
            .Select(x => x.Action switch
            {
                NotifyCollectionChangedAction.Add => (x.Action, items: x.NewItems),
                NotifyCollectionChangedAction.Remove => (x.Action, items: x.OldItems),
                _ => default
            }).WhereNotNull()
            .SelectMany(x => x.items.Cast<T>().Select(y => (x.Action, y)));
        }

        public static IObservable<T> ToObservable<T>(this ObservableCollection<T> collection, NotifyCollectionChangedAction action)
        {
            return collection.ToObservable()
                             .Where(x => x.action == action)
                             .Select(x => x.el);
        }

        public static IObservable<T> ToObservable<T>(this PubSubEvent<T> pubSub)
        {
            return Observable.Create<T>(observer => pubSub.Subscribe(observer.OnNext));
        }

        public static IObservable<Unit> ToObservable(this PubSubEvent pubSub)
        {
            return Observable.FromEvent
            (
                act => pubSub.Subscribe(act),
                act => pubSub.Unsubscribe(act)
            );
        }

        public static IObservable<MouseEventArgs> MouseMoveObservable(this UIElement uIElement)
        {
            return Observable.FromEvent<MouseEventHandler, MouseEventArgs>
            (
                act => (sender, e) => act(e),
                act => uIElement.MouseMove += act,
                act => uIElement.MouseMove -= act
            );
        }

        public static IObservable<MouseButtonEventArgs> MouseUpObservable(this UIElement uIElement)
        {
            return Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>
            (
                act => (sender, e) => act(e),
                act => uIElement.MouseLeftButtonUp += act,
                act => uIElement.MouseLeftButtonUp -= act
            );
        }

        public static IObservable<MouseButtonEventArgs> MouseDownObservable(this UIElement uIElement)
        {
            return Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>
            (
                act => (sender, e) => act(e),
                act => uIElement.MouseLeftButtonDown += act,
                act => uIElement.MouseLeftButtonDown -= act
            );
        }

        public static IObservable<SizeChangedEventArgs> SizeChangedObservable(this FrameworkElement uIElement)
        {
            return Observable.FromEvent<SizeChangedEventHandler, SizeChangedEventArgs>
            (
                act => (sender, e) => act(e),
                act => uIElement.SizeChanged += act,
                act => uIElement.SizeChanged -= act
            );
        }
    }
}
