using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Software.IP.Trace.Services.Ips;

namespace Software.IP.Trace.Views.FindResult {
    public class FindResultViewModel : Screen, IFindResultViewModel {
        private readonly IEventAggregator _eventAggregator;
        private readonly IpService _ipService;

        public FindResultViewModel(
            IEventAggregator eventAggregator,
            IpService ipService) {
            // fields
            _eventAggregator = eventAggregator;
            _ipService = ipService;
        }

        #region Commands

        public Task Save() => 
            _ipService.Add(this.IPAddress, this.Latitude, this.Longitude);

        public bool CanSave => !string.IsNullOrWhiteSpace(IPAddress);

        #endregion

        #region Properties

        public string IPAddress {
            get => _ipAddress;
            set => Set(ref _ipAddress, value);
        }
        private string _ipAddress = "";

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

        #endregion

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

        public Task HandleAsync(FindResultHandlerArgs message, CancellationToken cancellationToken) =>
            Observable
                .Return(message)
                .Do(msg => {
                    IPAddress = msg.Data.ip;
                    Latitude = msg.Data.latitude;
                    Longitude = msg.Data.longitude;
                })
                .Do(_ => NotifyOfPropertyChange(nameof(CanSave)))
                .ToTask(cancellationToken);

        #endregion
    }

    public interface IFindResultViewModel :
        IHandle<FindResultHandlerArgs> { }
}
