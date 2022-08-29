using Software.IP.Trace.Services.IpStack;
using System;

namespace Software.IP.Trace.Views.FindResult {
    public class FindResultHandlerArgs {
        public FindResultHandlerArgs(Guid operationId, IpStackDO data) {
            // properties
            OperationId = operationId;
            Data = data;
        }

        public Guid OperationId { get; }
        public IpStackDO Data { get; }
    }
}
