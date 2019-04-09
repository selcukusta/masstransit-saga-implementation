using System;

namespace Sample.Contracts
{
    public interface IReportRequestCommand
    {
        string CustomerId { get; set; }
        string ReportId { get; set; }
        DateTime RequestTime { get; set; }
    }
}
