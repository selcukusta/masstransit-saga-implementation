using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Sample.Contracts;

namespace Sample.Service.ReportFailed
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, RabbitMqConstants.ReportRequestServiceQueue, e =>
                {
                    e.Consumer<ReportFailedConsumer>();
                });
            });

            await bus.StartAsync(CancellationToken.None);
            Console.WriteLine("Listening for report requests.. Press enter to exit");
            Console.ReadLine();
            await bus.StopAsync(CancellationToken.None);
        }
    }

    public class ReportFailedConsumer : IConsumer<IReportFailedEvent>
    {
        public async Task Consume(ConsumeContext<IReportFailedEvent> context)
        {
            var reportId = context.Message.ReportId;
            await Console.Out.WriteLineAsync($"Report operation is failed! Report Id: {reportId}. Fault Message: {context.Message.FaultMessage}. Correlation Id: {context.Message.CorrelationId}");
            //Send mail, push notification, etc...
        }
    }
}
