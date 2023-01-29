using Spents.EventSourcing.Domain.Entities;

namespace Spents.EventSourcing.Domain.Interfaces
{
    public interface IReceiptCreatedEventRepository
    {
        Task AddReceiptCreatedEvent(ReceiptCreatedEntity receipt);
    }
}
