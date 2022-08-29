using Caliburn.Micro;
using Software.IP.Trace.Views.Modal;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Software.IP.Trace.Views.Main;

namespace Software.IP.Trace.Views.DialogHost;

public class DialogViewModel : Conductor<MainViewModel>.Collection.OneActive, IDialogViewModel {
    private readonly IEventAggregator _eventAggregator;

    public DialogViewModel(
        IEventAggregator eventAggregator,
        ModalViewModel modalViewModel) {
        // fields
        _eventAggregator = eventAggregator;
        // properties
        this.Modal = modalViewModel;
    }

    #region Properties

    public ModalViewModel Modal { get; }

    public bool IsOpen {
        get => _isOpen;
        set => this.Set(ref _isOpen, value);
    }
    private bool _isOpen;

    public bool CloseOnClickAway {
        get => _closeOnClickAway;
        set => this.Set(ref _closeOnClickAway, value);
    }
    private bool _closeOnClickAway;

    #endregion

    #region Override

    protected override async Task OnActivateAsync(CancellationToken cancellationToken) =>
        await Observable
            .Return(Caliburn.Micro.IoC.Get<MainViewModel>())
            .SelectMany(vm => ChangeActiveItemAsync(vm, true, cancellationToken).ToObservable())
            .Do(_ => _eventAggregator.SubscribeOnPublishedThread(this))
            .SelectMany(_ => ScreenExtensions.TryActivateAsync(this.Modal, cancellationToken).ToObservable())
            .SelectMany(_ => base.OnActivateAsync(cancellationToken).ToObservable())
            .ToTask(cancellationToken);
    
    protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken) =>
        base.OnDeactivateAsync(close, cancellationToken)
            .ToObservable()
            .SelectMany(_ => ScreenExtensions.TryDeactivateAsync(this.Modal, close, cancellationToken).ToObservable())
            .Where(_ => close)
            .Do(_ => _eventAggregator.Unsubscribe(this))
            .ToTask(cancellationToken);

    #endregion

    #region Handle

    public Task HandleAsync(DialogStateHandlerArgs message, CancellationToken cancellationToken) =>
        Observable
            .Return(message)
            .Do(msg => {
                IsOpen = msg.IsOpen;
                CloseOnClickAway = msg.CloseOnClickAway;
            })
            .ToTask(cancellationToken);

    #endregion
}

public interface IDialogViewModel : IHandle<DialogStateHandlerArgs> { }