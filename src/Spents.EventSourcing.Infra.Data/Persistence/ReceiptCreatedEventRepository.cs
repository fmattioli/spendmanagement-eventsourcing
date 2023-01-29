using MongoDB.Driver;

using Spents.EventSourcing.Domain.Entities;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.Infra.Data.Persistence
{
    public class ReceiptCreatedEventRepository : IReceiptCreatedEventRepository
    {
        private readonly IMongoCollection<ReceiptCreatedEntity> _receiptCollection;
        public ReceiptCreatedEventRepository(IMongoDatabase database)
        {
            _receiptCollection = database.GetCollection<ReceiptCreatedEntity>("receipts-events");
        }

        public async Task AddReceiptCreatedEvent(ReceiptCreatedEntity receipt)
        {
            await _receiptCollection.InsertOneAsync(receipt);
        }
    }
}
