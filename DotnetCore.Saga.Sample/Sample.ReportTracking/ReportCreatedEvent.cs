using System;
using Sample.Contracts;

namespace Sample.ReportTracking
{
    public class ReportCreatedEvent : IReportCreatedEvent
    {
        private readonly ReportSagaState _reportSagaState;
        public ReportCreatedEvent(ReportSagaState reportSagaState)
        {
            _reportSagaState = reportSagaState;
        }

        public Guid CorrelationId => _reportSagaState.CorrelationId;

        public string CustomerId => _reportSagaState.CustomerId;
        public string ReportId => _reportSagaState.ReportId;

        public string BlobUri => _reportSagaState.BlobUri;

        public DateTime CreationTime => _reportSagaState.CreationTime;
    }
}
