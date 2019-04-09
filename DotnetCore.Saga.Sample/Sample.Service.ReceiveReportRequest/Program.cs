using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Sample.Contracts;

namespace Sample.Service.ReceiveReportRequest
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.ReportRequestServiceQueue, e =>
                {
                    e.Consumer<ReportRequestReceivedConsumer>();
                });
            });

            await bus.StartAsync(CancellationToken.None);
            Console.WriteLine("Listening for report requests.. Press enter to exit");
            Console.ReadLine();
            await bus.StopAsync(CancellationToken.None);
        }
    }

    public class ReportRequestReceivedConsumer : IConsumer<IReportRequestReceivedEvent>
    {
        public async Task Consume(ConsumeContext<IReportRequestReceivedEvent> context)
        {
            var reportId = context.Message.ReportId;
            await Console.Out.WriteLineAsync($"Report request is received, report id is; {reportId}. Correlation Id: {context.Message.CorrelationId}");
            //Get report from Db, file, etc...
            if (reportId.StartsWith("report-", StringComparison.Ordinal))
            {
                await context.Publish<IReportCreatedEvent>(new
                {
                    context.Message.CorrelationId,
                    context.Message.CustomerId,
                    context.Message.ReportId,
                    BlobUri = "https://google.com",
                    CreationTime = DateTime.Now
                });
            }
            else
            {
                await context.Publish<IReportFailedEvent>(new
                {
                    context.Message.CorrelationId,
                    context.Message.CustomerId,
                    context.Message.ReportId,
                    FaultMessage = "Report name is invalid! Please retry again!",
                    FaultTime = DateTime.Now
                });
            }
        }
    }
}
