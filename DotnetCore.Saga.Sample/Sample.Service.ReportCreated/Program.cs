using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Sample.Contracts;

namespace Sample.Service.ReportCreated
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.ReportRequestServiceQueue, e =>
                {
                    e.Consumer<ReportCreatedConsumer>();
                });
            });

            await bus.StartAsync(CancellationToken.None);
            Console.WriteLine("Listening for report requests.. Press enter to exit");
            Console.ReadLine();
            await bus.StopAsync(CancellationToken.None);
        }
    }

    public class ReportCreatedConsumer : IConsumer<IReportCreatedEvent>
    {
        public async Task Consume(ConsumeContext<IReportCreatedEvent> context)
        {
            var reportId = context.Message.ReportId;
            await Console.Out.WriteLineAsync($"Report operation is succeeded! Report Id: {reportId}. Blob Uri: {context.Message.BlobUri}. Correlation Id: {context.Message.CorrelationId}");
            //Send mail, push notification, etc...

        }
    }
}
