using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using Spents.Events.v1;
using Spents.EventSourcing.Domain.Entities;
using Spents.EventSourcing.Domain.Interfaces;
using Spents.EventSourcing.Domain.ValueObjects;

namespace Spents.EventSourcing.Kafka.Core.Handlers
{
    public class ReceiptCreatedEventHandler : IMessageHandler<ReceiptEvents>
    {
        private readonly ILogger log;
        private readonly IReceiptEvents receiptCreatedEventRepository;
        public ReceiptCreatedEventHandler(ILogger log, IReceiptEvents receiptCreatedEventRepository)
        {
            this.log = log;
            this.receiptCreatedEventRepository = receiptCreatedEventRepository;
        }

        public async Task Handle(IMessageContext context, ReceiptEvents message)
        {
            await receiptCreatedEventRepository.AddReceiptCreatedEvent(
                new ReceiptEventsEntity
                {
                    EventStatus = (ReceiptEventStatus)message.ReceiptStatus,
                    ReceiptBody = message.Receipt
                });

            this.log.Information(
                $"Kafka message received and processed.",
                () => new
                {
                    message.Receipt
                });
        }
    }
}
