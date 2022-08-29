using Caliburn.Micro;
using Software.IP.Trace.Views.Handler;
using System;

namespace Software.IP.Trace.Views.Modal;
public class OpenModalHandlerArgs<T> : IHandlerArgs where T : Screen {
    public OpenModalHandlerArgs(Guid operationId) {
        // properties
        OperationId = operationId;
    }

    public Guid OperationId { get; }
    public Type ModalType => typeof(T);
}

public class CloseModalHandlerArgs : IHandlerArgs {
    public CloseModalHandlerArgs(Guid operationId) {
        // properties
        OperationId = operationId;
    }

    public Guid OperationId { get; }
}
