using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using Spents.Domain.Entities;
using Spents.Events.v1;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.Kafka.Core.Handlers
{
    public class ReceiptCreatedEventHandler : IMessageHandler<ReceiptEvent<Receipt>>
    {
        private readonly ILogger log;
        private readonly IReceiptEventsRepository receiptCreatedEventRepository;
        public ReceiptCreatedEventHandler(ILogger log, IReceiptEventsRepository receiptCreatedEventRepository)
        {
            this.log = log;
            this.receiptCreatedEventRepository = receiptCreatedEventRepository;
        }

        public async Task Handle(IMessageContext context, ReceiptEvent<Receipt> message)
        {
            if (message is not null)
            {
                await receiptCreatedEventRepository.AddReceiptCreatedEvent(message);

                this.log.Information(
                    $"Kafka message received and processed.",
                    () => new
                    {
                        message
                    });
            }
        }
    }
}
