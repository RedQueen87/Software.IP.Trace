using System;

namespace Software.IP.Trace.Views.Main {
    public class OpenMainHandlerArgs {
        public OpenMainHandlerArgs(Guid operationId, Type viewModelType) {
            // properties
            OperationId = operationId;
            ViewModelType = viewModelType;
        }
        public Guid OperationId { get; }
        public Type ViewModelType { get; }
    }
}
