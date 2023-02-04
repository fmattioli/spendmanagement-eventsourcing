using Spents.Domain.Entities;
using Spents.EventSourcing.Domain.ValueObjects;

namespace Spents.EventSourcing.Domain.Entities
{
    public class ReceiptEventsEntity
    {
        public Guid Id { get; set; }
        public ReceiptEventStatus EventStatus { get; set; } 
        public Receipt ReceiptBody { get; set; } = null!;
    }
}
