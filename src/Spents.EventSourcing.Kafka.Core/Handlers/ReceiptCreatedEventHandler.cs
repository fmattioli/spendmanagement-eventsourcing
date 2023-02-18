using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using Spents.Events.v1;
using Spents.EventSourcing.Domain.Entities;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.Kafka.Core.Handlers
{
    public class ReceiptCreatedEventHandler : IMessageHandler<ReceiptEventCreated>
    {
        private readonly ILogger log;
        private readonly IReceiptEvents receiptCreatedEventRepository;
        public ReceiptCreatedEventHandler(ILogger log, IReceiptEvents receiptCreatedEventRepository)
        {
            this.log = log;
            this.receiptCreatedEventRepository = receiptCreatedEventRepository;
        }

        public async Task Handle(IMessageContext context, ReceiptEventCreated message)
        {
            if (message.Body is not null)
            {
                await receiptCreatedEventRepository.AddReceiptCreatedEvent(new ReceiptEventsEntity(message.Body));

                this.log.Information(
                    $"Kafka message received and processed.",
                    () => new
                    {
                        message.Body
                    });
            }
        }
    }
}
