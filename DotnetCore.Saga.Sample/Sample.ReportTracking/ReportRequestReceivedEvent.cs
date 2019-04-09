using System;
using Sample.Contracts;

namespace Sample.ReportTracking
{
    public class ReportRequestReceivedEvent : IReportRequestReceivedEvent
    {
        private readonly ReportSagaState _reportSagaState;
        public ReportRequestReceivedEvent(ReportSagaState reportSagaState)
        {
            _reportSagaState = reportSagaState;
        }

        public Guid CorrelationId => _reportSagaState.CorrelationId;

        public string CustomerId => _reportSagaState.CustomerId;
        public string ReportId => _reportSagaState.ReportId;
        public DateTime RequestTime => _reportSagaState.RequestTime;
    }
}
