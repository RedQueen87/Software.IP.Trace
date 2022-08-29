using Caliburn.Micro;
using Software.IP.Trace.Views.DialogHost;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Software.IP.Trace.Views.Handler;

public class HandlerViewModel : Conductor<DialogViewModel>.Collection.OneActive, IHandlerViewModel {
    private readonly IEventAggregator _eventAggregator;

    public HandlerViewModel(IEventAggregator eventAggregator) {
        // fields
        _eventAggregator = eventAggregator;
    }

    #region Override

    protected override async Task OnActivateAsync(CancellationToken cancellationToken) =>
        await Observable
            .Return(Caliburn.Micro.IoC.Get<DialogViewModel>())
            .SelectMany(vm => ChangeActiveItemAsync(vm, true, cancellationToken).ToObservable())
            .Do(_ => _eventAggregator.SubscribeOnPublishedThread(this))
            .SelectMany(_ => base.OnActivateAsync(cancellationToken).ToObservable())
            .ToTask(cancellationToken);

    protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken) =>
        base.OnDeactivateAsync(close, cancellationToken)
            .ToObservable()
            .Where(_ => close)
            .Do(_ => _eventAggregator.Unsubscribe(this))
            .ToTask(cancellationToken);

    #endregion

    public Task HandleAsync(IHandlerArgs message, CancellationToken cancellationToken) {
        // TODO
        Debug.WriteLine(message, $"DEBUG.{nameof(IHandlerViewModel)}");
        return Task.FromResult(0);
    }
}

public interface IHandlerViewModel : IHandle<IHandlerArgs> { }