using Caliburn.Micro;
using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Software.IP.Trace.Views.Main;

public static class EventAggregatorExtensions {
    public static IObservable<Guid> OpenMain<T>(this IEventAggregator eventAggregator, Guid operationId) where T : Screen {
        var args = new OpenMainHandlerArgs(operationId, typeof(T));
        return eventAggregator
            .PublishOnUIThreadAsync(args)
            .ToObservable()
            .Select(_ => operationId);
    }
}