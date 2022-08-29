using Software.IP.Trace.Views.Handler;
using System;

namespace Software.IP.Trace.Views.Progress;

public class ProgressBarTitleHandlerArgs : IHandlerArgs {
    public ProgressBarTitleHandlerArgs(Guid operationId, string title) {
        // properties
        OperationId = operationId;
        Title = title;
    }

    public Guid OperationId { get; }
    public string Title { get; set; }
}