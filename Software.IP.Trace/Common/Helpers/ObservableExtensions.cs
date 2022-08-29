using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Software.IP.Trace.Common.Helpers;

public static class ObservableExtensions {
    public static IObservable<TSource> DelaySec<TSource>(this IObservable<TSource> source, int seconds) =>
        source.Delay(TimeSpan.FromSeconds(seconds));

    public static void AddToCompositeDisposable(this IDisposable source, CompositeDisposable compositeDisposable) =>
        compositeDisposable.Add(source);
}