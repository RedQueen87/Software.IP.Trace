using Caliburn.Micro;
using Software.IP.Trace.Services.Ips;
using Software.IP.Trace.Views.Find;
using Software.IP.Trace.Views.Main;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Software.IP.Trace.Views.List {
    public class ListViewModel : Screen {
        private readonly IEventAggregator _eventAggregator;
        private readonly IIpService _ipService;

        public ListViewModel(
            IEventAggregator eventAggregator,
            IIpService ipService) {
            // fields
            _eventAggregator = eventAggregator;
            _ipService = ipService;
            // properties
            IpItems = new BindableCollection<IpItemViewModel>();
        }

        #region Commands

        public Task Reload() =>
             _ipService
                .List()
                .ToObservable()
                .Do(_ => IpItems.Clear())
                .Where(list => list.Any())
                .SelectMany(list => list.ToObservable())
                .Select(dataObject => new IpItemViewModel {
                    IP = dataObject.Ip,
                    Latitude = dataObject.Latitude,
                    Longitude = dataObject.Longitude
                })
                .Do(vm => this.IpItems.Add(vm))
                .ToTask();

        public Task Add() => Observable
            .Return(Guid.NewGuid())
            .SelectMany(operationId => _eventAggregator.OpenMain<FindViewModel>(operationId))
            .ToTask();

        public Task Delete() =>
            Observable
                .Return(this.IpItems)
                .SelectMany(items => items
                    .ToObservable()
                    .Where(ip => ip.IsSelected)
                    .Select(ip => ip.IP)
                    .Do(ip => _ipService.Delete(ip)))
                .LastOrDefaultAsync()
                .SelectMany(_ => Reload().ToObservable())
                .ToTask();

        #endregion

        #region Properties

        public BindableCollection<IpItemViewModel> IpItems { get; set; }

        #endregion

        #region Override

        protected override async Task OnActivateAsync(CancellationToken cancellationToken) {
            _eventAggregator.SubscribeOnPublishedThread(this);
            await Reload();
            await base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken) {
            if (close)
                _eventAggregator.Unsubscribe(this);

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion
    }

    public class IpItemViewModel : PropertyChangedBase {
        public bool IsSelected {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }
        private bool _isSelected;

        public string IP {
            get => _ip;
            set => Set(ref _ip, value);
        }
        private string _ip = "";

        public double Latitude {
            get => _latitude;
            set => Set(ref _latitude, value);
        }
        private double _latitude;

        public double Longitude {
            get => _longitude;
            set => Set(ref _longitude, value);
        }
        private double _longitude;
    }
}
