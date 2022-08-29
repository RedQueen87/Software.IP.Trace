using System;

namespace Software.IP.Trace.Views.DialogHost;

public class DialogStateHandlerArgs {
    public DialogStateHandlerArgs(Guid operationId, bool isOpen, bool closeOnClickAway = false) {
        // properties
        OperationId = operationId;
        IsOpen = isOpen;
        CloseOnClickAway = closeOnClickAway;
    }

    public Guid OperationId { get; }
    public bool IsOpen { get; }
    public bool CloseOnClickAway { get; }
}