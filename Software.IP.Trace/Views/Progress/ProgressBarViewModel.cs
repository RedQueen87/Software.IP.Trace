using Caliburn.Micro;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Software.IP.Trace.Views.Progress {
    public class ProgressBarViewModel : Screen, IProgressBarViewModel {
        private readonly IEventAggregator _eventAggregator;

        public ProgressBarViewModel(
            IEventAggregator eventAggregator) {
            // fields
            _eventAggregator = eventAggregator;
        }
        
        #region Properties

        public string Title {
            get => _title;
            private set => Set(ref _title, value);
        }
        private string _title = "";

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

        public Task HandleAsync(ProgressBarTitleHandlerArgs message, CancellationToken cancellationToken) =>
            Observable
                .Return(message)
                .Do(_ => this.Title = message.Title)
                .ToTask(cancellationToken);

        #endregion
    }

    public interface IProgressBarViewModel :
        IHandle<ProgressBarTitleHandlerArgs> { }
}
