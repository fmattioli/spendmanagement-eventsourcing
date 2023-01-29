using MongoDB.Driver;
using Spents.EventSourcing.Domain.Entities;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.Infra.Data.Persistence
{
    public class ReceiptEventsRepository : IReceiptEvents
    {
        private readonly IMongoCollection<ReceiptEventEntity> _receiptCollection;
        public ReceiptEventsRepository(IMongoDatabase database)
        {
            _receiptCollection = database.GetCollection<ReceiptEventEntity>("receipts-events");
        }

        public async Task AddReceiptCreatedEvent(ReceiptEventEntity receipt)
        {
            await _receiptCollection.InsertOneAsync(receipt);
        }

        public async Task<IReadOnlyCollection<ReceiptEventEntity>> GetAllEvents(Guid receiptId)
        {
            var filter = Builders<ReceiptEventEntity>.Filter.Eq(x => x.Id, receiptId);
            return await _receiptCollection.Find(filter).ToListAsync();
            
        }
    }
}
