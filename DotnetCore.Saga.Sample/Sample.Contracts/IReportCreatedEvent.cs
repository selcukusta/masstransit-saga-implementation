using System;

namespace Sample.Contracts
{
    public interface IReportCreatedEvent
    {
        Guid CorrelationId { get; }
        string CustomerId { get; }
        string ReportId { get; }
        string BlobUri { get; }
        DateTime CreationTime { get; }
    }
}
