using KafkaFlow;
using KafkaFlow.TypedHandler;

using Microsoft.AspNetCore.Mvc;

using Serilog;

using Spents.Events.Events.v1;

namespace Spents.EventSourcing.Kafka.Core.Handlers
{
    public class ReceiptCreatedEventHandler : IMessageHandler<ReceiptCreatedEvent>
    {
        private readonly ILogger log;
        public ReceiptCreatedEventHandler(ILogger log)
        {
            this.log = log;
        }

        public Task Handle(IMessageContext context, ReceiptCreatedEvent message)
        {
            this.log.Information(
                $"Kafka message received.",
                () => new
                {
                    message.Body
                });

            return Task.CompletedTask;
        }
    }
}
