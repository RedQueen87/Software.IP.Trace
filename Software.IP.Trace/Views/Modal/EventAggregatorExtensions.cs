using Caliburn.Micro;
using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Software.IP.Trace.Views.Modal;

public static class EventAggregatorExtensions {
    public static IObservable<Guid> OpenModal<T>(this IEventAggregator eventAggregator, Guid operationId) where T : Screen {
        var args = new OpenModalHandlerArgs<T>(operationId);
        return eventAggregator
            .PublishOnUIThreadAsync(args)
            .ToObservable()
            .Select(_ => operationId);
    }

    public static IObservable<Guid> CloseModal(this IEventAggregator eventAggregator, Guid operationId) {
        var args = new CloseModalHandlerArgs(operationId);
        return eventAggregator
            .PublishOnUIThreadAsync(args)
            .ToObservable()
            .Select(_ => operationId);
    }
}
