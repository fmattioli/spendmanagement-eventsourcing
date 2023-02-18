using MongoDB.Bson.Serialization.Attributes;
using Spents.Domain.Entities;
using Spents.Events.v1;

namespace Spents.EventSourcing.Domain.Entities
{
    public class ReceiptEventsEntity : ReceiptEventCreated
    {
        public ReceiptEventsEntity(Receipt receipt) : base(receipt)
        {
            Id = receipt.Id;
        }

        [BsonId]
        public Guid Id { get; set; }
    }
}
