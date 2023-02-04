using Spents.EventSourcing.Domain.Entities;

namespace Spents.EventSourcing.Domain.Interfaces
{
    public interface IReceiptEvents
    {
        Task<IReadOnlyCollection<ReceiptEventsEntity>> GetAllEvents(Guid receiptId);
        Task AddReceiptCreatedEvent(ReceiptEventsEntity receipt);
    }
}
