using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Sample.Contracts
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Durable = true;
                cfg.PrefetchCount = 1;
                cfg.PurgeOnStartup = true;
                var host = cfg.Host(new Uri(RabbitMqConstants.RabbitMqUri), hst =>
                {
                    hst.Username(RabbitMqConstants.UserName);
                    hst.Password(RabbitMqConstants.Password);
                });
                registrationAction?.Invoke(cfg, host);
            });
        }
    }
}
