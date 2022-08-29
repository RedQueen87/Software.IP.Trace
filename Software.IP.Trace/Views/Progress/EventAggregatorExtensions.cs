using Caliburn.Micro;
using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Software.IP.Trace.Views.Progress {
    public static class EventAggregatorExtensions {
        public static IObservable<Guid> UpdateProgressBarTitle(this IEventAggregator eventAggregator, Guid operationId, string title) {
            var args = new ProgressBarTitleHandlerArgs(operationId, title);
            return eventAggregator
                .PublishOnUIThreadAsync(args)
                .ToObservable()
                .Select(_ => operationId);
        }
    }
}
