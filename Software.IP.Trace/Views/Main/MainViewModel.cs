using Caliburn.Micro;
using Software.IP.Trace.Common.Helpers;
using Software.IP.Trace.Views.Find;
using Software.IP.Trace.Views.List;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Software.IP.Trace.Views.Main;

public class MainViewModel : Conductor<Screen>.Collection.OneActive, IMainViewModel {
    private readonly IEventAggregator _eventAggregator;

    public MainViewModel(IEventAggregator eventAggregator) {
        // fields
        _eventAggregator = eventAggregator;
    }

    #region Commands

    public void Close() => Application.Current.Shutdown();

    public Task OpenFind() =>
        Observable
            .Return(Caliburn.Micro.IoC.Get<FindViewModel>())
            .SelectMany(vm => ChangeActiveItemAsync(vm, true).ToObservable())
            .Do(_ => {
                NotifyOfPropertyChange(nameof(CanOpenFind));
                NotifyOfPropertyChange(nameof(CanOpenList));
            })
            .ToTask();

    public bool CanOpenFind => !ActiveItem.GetType().CanCastTo<FindViewModel>();

    public Task OpenList() =>
        Observable
            .Return(Caliburn.Micro.IoC.Get<ListViewModel>())
            .SelectMany(vm => ChangeActiveItemAsync(vm, true).ToObservable())
            .Do(_ => {
                NotifyOfPropertyChange(nameof(CanOpenFind));
                NotifyOfPropertyChange(nameof(CanOpenList));
            })
            .ToTask();

    public bool CanOpenList => !ActiveItem.GetType().CanCastTo<ListViewModel>();

    #endregion

    #region Override

    protected override async Task OnActivateAsync(CancellationToken cancellationToken) =>
        await this
            .OpenFind()
            .ToObservable()
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

    #region Handle

    public Task HandleAsync(OpenMainHandlerArgs message, CancellationToken cancellationToken) {
        if (message.ViewModelType.CanCastTo<FindViewModel>())
            return OpenFind();

        if (message.ViewModelType.CanCastTo<ListViewModel>())
            return OpenList();

        Debugger.Break();
        return Task.CompletedTask;
    }

    #endregion
}

public interface IMainViewModel :
    IHandle<OpenMainHandlerArgs> { }