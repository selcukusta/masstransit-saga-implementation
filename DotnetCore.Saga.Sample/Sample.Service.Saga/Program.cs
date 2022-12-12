using System;
using MassTransit;
using Sample.Contracts;
using Sample.ReportTracking;
using System.Threading.Tasks;
using System.Threading;

namespace Sample.Service.Saga
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var sagaStateMachine = new ReportStateMachine();
            var repository = new InMemorySagaRepository<ReportSagaState>();
            var bus = BusConfigurator.ConfigureBus((cfg) =>
            {
                cfg.ReceiveEndpoint(RabbitMqConstants.SagaQueue, e =>
                {
                    e.StateMachineSaga(sagaStateMachine, repository);
                });
            });
            await bus.StartAsync(CancellationToken.None);
            Console.WriteLine("Saga active.. Press enter to exit");
            Console.ReadLine();
            await bus.StopAsync(CancellationToken.None);
        }
    }
}
