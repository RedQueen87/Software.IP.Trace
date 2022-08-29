using Caliburn.Micro;
using Software.IP.Trace.Views.Progress;
using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Software.IP.Trace.Services.IpStack;
using Software.IP.Trace.Views.FindResult;

namespace Software.IP.Trace.Views.Find;

public static class SearchCommandObservableExtensions {
    public static IObservable<SearchCommandStateDO> Search(this IObservable<SearchCommandStateDO> @this, string ip, string apiKey) => 
        @this
            .SelectMany(state => state
            .IpStackService
            .GetAsync(ip, apiKey)
            .ToObservable()
            .Do(result => state.Result = result)
            .Select(_ => state));

    public static IObservable<SearchCommandStateDO> ShowResult(this IObservable<SearchCommandStateDO> @this) =>
        @this
            .SelectMany(state => state
                .EventAggregator
                .PublishOnUIThreadAsync(new FindResultHandlerArgs(state.OperationId, state.Result))
                .ToObservable()
                .Select(_ => state));

    public static IObservable<SearchCommandStateDO> UpdateProgressBarTitle(this IObservable<SearchCommandStateDO> @this, string title) => @this
        .SelectMany(state => state
            .EventAggregator
            .UpdateProgressBarTitle(state.OperationId, title)
            .Select(_ => state));
}

public class SearchCommandStateDO {
    public SearchCommandStateDO(Guid operationId, IEventAggregator eventAggregator, IIpStackService ipStackService) {
        // properties
        OperationId = operationId;
        EventAggregator = eventAggregator;
        IpStackService = ipStackService;
    }

    public Guid OperationId { get; }
    public IEventAggregator EventAggregator { get; }
    public IIpStackService IpStackService { get; }
    public IpStackDO? Result { get; set; }
}