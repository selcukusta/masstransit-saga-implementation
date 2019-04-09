using System;
using Automatonymous;
using Sample.Contracts;

namespace Sample.ReportTracking
{

    public class ReportStateMachine : MassTransitStateMachine<ReportSagaState>
    {
        public State Submitted { get; private set; }
        public State Processed { get; private set; }

        public Event<IReportRequestReceivedEvent> ReportRequestReceived { get; private set; }
        public Event<IReportCreatedEvent> ReportCreated { get; private set; }
        public Event<IReportFailedEvent> ReportFailed { get; private set; }
        public Event<IReportRequestCommand> CreateReportCommandReceived { get; private set; }

        public ReportStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => CreateReportCommandReceived, cc => cc
                        .CorrelateBy(state => state.ReportId, context => context.Message.ReportId)
                        .SelectId(context => Guid.NewGuid()));
            Event(() => ReportRequestReceived, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => ReportCreated, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => ReportFailed, x => x.CorrelateById(context => context.Message.CorrelationId));

            During(Initial,
                When(CreateReportCommandReceived).Then(context =>
                {
                    context.Instance.CustomerId = context.Data.CustomerId;
                    context.Instance.RequestTime = context.Data.RequestTime;
                    context.Instance.ReportId = context.Data.ReportId;
                })
                .Publish(ctx => new ReportRequestReceivedEvent(ctx.Instance))
                .TransitionTo(Submitted)
                .ThenAsync(context => Console.Out.WriteLineAsync(context.Instance.ToString()))
            );

            During(Submitted, 
                When(ReportRequestReceived)
                .TransitionTo(Processed)
                .ThenAsync(context => Console.Out.WriteLineAsync(context.Instance.ToString())));

            During(Processed,
                When(ReportCreated).Then(context =>
                {
                    context.Instance.CustomerId = context.Data.CustomerId;
                    context.Instance.ReportId = context.Data.ReportId;

                    context.Instance.BlobUri = context.Data.BlobUri;
                    context.Instance.CreationTime = context.Data.CreationTime;
                })
                .Publish(ctx => new ReportCreatedEvent(ctx.Instance)).Finalize()
                .ThenAsync(context => Console.Out.WriteLineAsync(context.Instance.ToString())),

                When(ReportFailed).Then(context =>
                {
                    context.Instance.CustomerId = context.Data.CustomerId;
                    context.Instance.ReportId = context.Data.ReportId;

                    context.Instance.FaultMessage = context.Data.FaultMessage;
                    context.Instance.FaultTime = context.Data.FaultTime; 
                })
                .Publish(ctx => new ReportFailedEvent(ctx.Instance)).Finalize()
                .ThenAsync(context => Console.Out.WriteLineAsync(context.Instance.ToString()))
            );

            SetCompletedWhenFinalized();
        }
    }
}
