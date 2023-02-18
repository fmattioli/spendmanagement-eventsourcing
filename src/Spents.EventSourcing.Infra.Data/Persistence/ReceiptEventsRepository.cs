using MongoDB.Driver;
using Spents.EventSourcing.Domain.Entities;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.Infra.Data.Persistence
{
    public class ReceiptEventsRepository : IReceiptEvents
    {
        private readonly IMongoCollection<ReceiptEventsEntity> _receiptCollection;
        public ReceiptEventsRepository(IMongoDatabase database)
        {
            _receiptCollection = database.GetCollection<ReceiptEventsEntity>("receipts-events");
        }

        public async Task AddReceiptCreatedEvent(ReceiptEventsEntity receipt)
        {
            await _receiptCollection.InsertOneAsync(receipt);
        }

        public async Task<IReadOnlyCollection<ReceiptEventsEntity>> GetAllEvents(Guid receiptId)
        {
            var filter = Builders<ReceiptEventsEntity>.Filter.Where(x => x.Id == receiptId);
            return await _receiptCollection.Find(filter).ToListAsync();
        }
    }
}
