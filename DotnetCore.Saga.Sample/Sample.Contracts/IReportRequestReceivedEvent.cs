using System;

namespace Sample.Contracts
{
    public interface IReportRequestReceivedEvent
    {
        Guid CorrelationId { get; }
        string CustomerId { get; }
        string ReportId { get; }
        DateTime RequestTime { get; }
    }
}