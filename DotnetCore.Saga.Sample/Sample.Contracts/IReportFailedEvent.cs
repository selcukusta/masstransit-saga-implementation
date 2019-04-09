using System;

namespace Sample.Contracts
{
    public interface IReportFailedEvent
    {
        Guid CorrelationId { get; }
        string CustomerId { get; }
        string ReportId { get; }
        string FaultMessage { get; }
        DateTime FaultTime { get; }
    }
}
