using Spents.Domain.Entities;
using Spents.Events.v1;

namespace Spents.EventSourcing.Domain.Interfaces
{
    public interface IReceiptEvents
    {
        Task<IReadOnlyCollection<ReceiptEvent<Receipt>>> GetAllEvents(Guid receiptId);
        Task AddReceiptCreatedEvent(ReceiptEvent<Receipt> receipt);
    }
}
