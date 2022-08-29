using Caliburn.Micro;
using Software.IP.Trace.Views.Progress;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;
using System.Reactive.Threading.Tasks;
using System.Threading;
using Software.IP.Trace.Common.Helpers;

namespace Software.IP.Trace.Views.Modal;

public class ModalViewModel : Conductor<Screen>.Collection.OneActive, IModalViewModel {
    private readonly IEventAggregator _eventAggregator;

    public ModalViewModel(IEventAggregator eventAggregator) {
        // fields
        _eventAggregator = eventAggregator;
    }

    #region Override

    protected override async Task OnActivateAsync(CancellationToken cancellationToken) {
        _eventAggregator.SubscribeOnPublishedThread(this);
        await base.OnActivateAsync(cancellationToken);
    }

    protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken) {
        if (close)
            _eventAggregator.Unsubscribe(this);

        return base.OnDeactivateAsync(close, cancellationToken);
    }

    #endregion

    #region Handle

    public Task HandleAsync(OpenModalHandlerArgs<ProgressBarViewModel> message, CancellationToken cancellationToken) =>
        Observable
            .Return(message)
            .SelectMany(msg => ChangeActiveItem(msg.ModalType).ToObservable())
            .ToTask(cancellationToken);

    public Task HandleAsync(CloseModalHandlerArgs message, CancellationToken cancellationToken) =>
        Observable
            .Return(message)
            .SelectMany(_ => this.CloseItemAsync(ActiveItem, cancellationToken).ToObservable())
            .ToTask(cancellationToken);

    #endregion

    private async Task ChangeActiveItem(Type viewType) {
        if (ActiveItem?.GetType().CanCastTo(viewType) ?? false)
            return;

        await this!.CloseItemAsync(ActiveItem);
        var vm = (Screen)Caliburn.Micro.IoC.GetInstance(viewType, null);
        await this.ChangeActiveItemAsync(vm, true);
    }
}

public interface IModalViewModel :
    IHandle<OpenModalHandlerArgs<ProgressBarViewModel>>,
    IHandle<CloseModalHandlerArgs> { }