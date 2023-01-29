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
        private readonly IReceiptCreatedEventRepository receiptCreatedEventRepository;
        public ReceiptCreatedEventHandler(ILogger log, IReceiptCreatedEventRepository receiptCreatedEventRepository)
        {
            this.log = log;
            this.receiptCreatedEventRepository = receiptCreatedEventRepository;
        }

        public async Task Handle(IMessageContext context, ReceiptCreatedEvent message)
        {
            var receipCreatedEntity = new ReceiptCreatedEntity
            {
                EventName = $"{ message.EventName}-{message.Version}",
                Id = message.Body.Id,
                ReceiptDate = message.Body.ReceiptDate,
                EstablishmentName = message.Body.EstablishmentName,
                ReceiptItems = message.Body.ReceiptItems.Select(x => new ReceiptItem(x.Name, x.Quantity, x.ItemPrice, x.Observation))
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
