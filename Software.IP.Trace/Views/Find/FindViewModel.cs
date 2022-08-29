using Caliburn.Micro;
using FluentValidation;
using FluentValidation.Results;
using Software.IP.Trace.Common.Helpers;
using Software.IP.Trace.Views.DialogHost;
using Software.IP.Trace.Views.FindResult;
using Software.IP.Trace.Views.Modal;
using Software.IP.Trace.Views.Progress;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Software.IP.Trace.Services.IpStack;

namespace Software.IP.Trace.Views.Find;

public class FindViewModel : Conductor<Screen>.Collection.OneActive, IDisposable {
    private readonly IEventAggregator _eventAggregator;
    private readonly IIpStackService _ipStackService;
    private readonly IValidator<FindViewModel> _searchCommandValidator = new SearchCommandValidator();
    private ValidationResult _searchCommandValidation;
    private readonly CompositeDisposable _vmDisposable = new();

    public FindViewModel(
        IEventAggregator eventAggregator,
        IIpStackService ipStackService) {
        // fields
        _eventAggregator = eventAggregator;
        _ipStackService = ipStackService;
        _searchCommandValidation = _searchCommandValidator.Validate(this);
    }

    #region Commands

    public async Task Search() {
        await Observable
            .Return(Guid.NewGuid())
            // load modal
            .SelectMany(_eventAggregator.OpenModal<ProgressBarViewModel>)
            // show dialog with modal
            .SelectMany(_eventAggregator.OpenDialog)
            // run commands
            .SelectMany(Run)
            // unload modal
            .SelectMany(_eventAggregator.CloseModal)
            // hide empty dialog
            .SelectMany(_eventAggregator.CloseDialog)
            .ToTask();
    }

    public bool CanSearch => _searchCommandValidation.IsValid;

    #endregion

    #region Properties

    public string IPAddress {
        get => _ipAddress.Value;
        set => this.Set(_ipAddress, value, nameof(IPAddress), nameof(CanSearch));
    }
    private readonly BehaviorSubject<string> _ipAddress = new("134.201.250.155");

    public string ApiKey {
        get => _apiKey.Value;
        set => this.Set(_apiKey, value, nameof(ApiKey), nameof(CanSearch));
    }
    private readonly BehaviorSubject<string> _apiKey = new(Consts.DefaultApiKey);

    #endregion

    #region Override

    protected override void OnViewLoaded(object view) {
        base.OnViewLoaded(view);
        Observable
            .Merge(_ipAddress, _apiKey)
            .Do(_ => Validate())
            .Subscribe()
            .AddToCompositeDisposable(_vmDisposable);
    }

    protected override async Task OnActivateAsync(CancellationToken cancellationToken) =>
        await Observable
            .Return(Caliburn.Micro.IoC.Get<FindResultViewModel>())
            .SelectMany(vm => ChangeActiveItemAsync(vm, true, cancellationToken).ToObservable())
            .Do(_ => _eventAggregator.SubscribeOnPublishedThread(this))
            .SelectMany(_ => base.OnActivateAsync(cancellationToken).ToObservable())
            .ToTask(cancellationToken);

    protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken) =>
        base.OnDeactivateAsync(close, cancellationToken)
            .ToObservable()
            .Where(_ => close)
            .Do(_ => _eventAggregator.Unsubscribe(this))
            .Do(_ => _vmDisposable.Dispose())
            .ToTask(cancellationToken);

    #endregion

    private IObservable<Guid> Run(Guid operationId) =>
        Observable
            .Return(new SearchCommandStateDO(operationId, _eventAggregator, _ipStackService))
            .UpdateProgressBarTitle("Searching...")
            .DelaySec(1)
            // search
            .Search(this.IPAddress, this.ApiKey)
            .ShowResult()
            // catch
            .Select(state => state.OperationId)
            .Catch<Guid, Exception>(ex => {
                Debug.WriteLine(ex);
                return Observable.Return(operationId);
            });

    private void Validate() => _searchCommandValidation = _searchCommandValidator.Validate(this);

    public void Dispose() {
        _vmDisposable.Dispose();
        _ipAddress.Dispose();
        _apiKey.Dispose();
    }
}