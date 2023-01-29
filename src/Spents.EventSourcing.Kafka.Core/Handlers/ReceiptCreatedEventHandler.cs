using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using Spents.Events.Events.v1;
using Spents.EventSourcing.Domain.Entities;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.Kafka.Core.Handlers
{
    public class ReceiptCreatedEventHandler : IMessageHandler<ReceiptCreatedEvent>
    {
        private readonly ILogger log;
        private readonly IReceiptEvents receiptCreatedEventRepository;
        public ReceiptCreatedEventHandler(ILogger log, IReceiptEvents receiptCreatedEventRepository)
        {
            this.log = log;
            this.receiptCreatedEventRepository = receiptCreatedEventRepository;
        }

        public async Task Handle(IMessageContext context, ReceiptCreatedEvent message)
        {
            var receipCreatedEntity = new ReceiptEventEntity
            {
                EventName = $"{ message.EventName}-{message.Version}",
                Id = message.Body.Id,
                Receipt = new Receipt(
                    message.Body.EstablishmentName, 
                    message.Body.ReceiptDate, 
                    message.Body.ReceiptItems
                    .Select(
                        x => new ReceiptItem(x.Id, x.Name, x.Quantity, x.ItemPrice, x.Observation)
                        )
                    )
            };

            await receiptCreatedEventRepository.AddReceiptCreatedEvent(receipCreatedEntity);

            this.log.Information(
                $"Kafka message received.",
                () => new
                {
                    message.Body
                });
        }
    }
}
