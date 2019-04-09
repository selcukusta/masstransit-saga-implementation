namespace Sample.Contracts
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "rabbitmq://localhost/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string ReportRequestServiceQueue = "registerorder.service";
        public const string SagaQueue = "saga.service";
    }
}
