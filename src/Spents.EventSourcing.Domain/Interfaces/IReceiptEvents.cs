using Spents.EventSourcing.Domain.Entities;

namespace Spents.EventSourcing.Domain.Interfaces
{
    public interface IReceiptEvents
    {
        Task<IReadOnlyCollection<ReceiptEventEntity>> GetAllEvents(Guid receiptId);
        Task AddReceiptCreatedEvent(ReceiptEventEntity receipt);
    }
}
