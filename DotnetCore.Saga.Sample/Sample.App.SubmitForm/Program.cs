using System;
using System.Threading.Tasks;
using Sample.Contracts;
using System.Linq;
namespace Sample.App.SubmitForm
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var requestedReportId = args.ElementAtOrDefault(0);
            if (string.IsNullOrWhiteSpace(requestedReportId))
            {
                Console.WriteLine("Please enter any report id!");
                requestedReportId = Console.ReadLine();
            }
            var bus = BusConfigurator.ConfigureBus();
            var sendToUri = new Uri($"{RabbitMqConstants.RabbitMqUri}{RabbitMqConstants.SagaQueue}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);
            await endPoint.Send<IReportRequestCommand>(new ReportRequestCommand
            {
                CustomerId = "customer-1234",
                ReportId = requestedReportId,
                RequestTime = DateTime.Now
            });
            Console.WriteLine("Message is sent!");
            Environment.Exit(0);
        }
    }

    class ReportRequestCommand : IReportRequestCommand
    {
        public string CustomerId { get; set; }
        public string ReportId { get; set; }
        public DateTime RequestTime { get; set; }
    }
}
