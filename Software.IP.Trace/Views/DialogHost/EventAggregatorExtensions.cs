using Caliburn.Micro;
using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Software.IP.Trace.Views.DialogHost;

public static class EventAggregatorExtensions {
    public static IObservable<Guid> OpenDialog(this IEventAggregator eventAggregator, Guid operationId) =>
        eventAggregator.OpenDialog(operationId, false);

    public static IObservable<Guid> OpenDialog(this IEventAggregator eventAggregator, Guid operationId, bool closeOnClickAway) {
        var args = new DialogStateHandlerArgs(operationId, true, closeOnClickAway);
        return eventAggregator
            .PublishOnUIThreadAsync(args)
            .ToObservable()
            .Select(_ => operationId);
    }

    public static IObservable<Guid> CloseDialog(this IEventAggregator eventAggregator, Guid operationId) {
        var args = new DialogStateHandlerArgs(operationId, false);
        return eventAggregator
            .PublishOnUIThreadAsync(args)
            .ToObservable()
            .Select(_ => operationId);
    }
}
