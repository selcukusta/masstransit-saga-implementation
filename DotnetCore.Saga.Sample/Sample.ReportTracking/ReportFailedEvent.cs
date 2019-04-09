using System;
using Sample.Contracts;

namespace Sample.ReportTracking
{
    public class ReportFailedEvent : IReportFailedEvent
    {
        private readonly ReportSagaState _reportSagaState;
        public ReportFailedEvent(ReportSagaState reportSagaState)
        {
            _reportSagaState = reportSagaState;
        }

        public Guid CorrelationId => _reportSagaState.CorrelationId;

        public string CustomerId => _reportSagaState.CustomerId;
        public string ReportId => _reportSagaState.ReportId;
        public string FaultMessage => _reportSagaState.FaultMessage;
        public DateTime FaultTime => _reportSagaState.FaultTime;
    }
}
