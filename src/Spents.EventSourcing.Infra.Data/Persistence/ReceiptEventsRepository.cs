using MongoDB.Driver;
using Spents.Domain.Entities;
using Spents.Events.v1;
using Spents.EventSourcing.Domain.Interfaces;

namespace Spents.EventSourcing.Infra.Data.Persistence
{
    public class ReceiptEventsRepository : IReceiptEventsRepository
    {
        private readonly IMongoCollection<ReceiptEvent<Receipt>> _receiptCollection;
        public ReceiptEventsRepository(IMongoDatabase database)
        {
            _receiptCollection = database.GetCollection<ReceiptEvent<Receipt>>("receipts-events");
        }

        public async Task AddReceiptCreatedEvent(ReceiptEvent<Receipt> receipt)
        {
            await _receiptCollection.InsertOneAsync(receipt);
        }

        public async Task<IReadOnlyCollection<ReceiptEvent<Receipt>>> GetAllEvents(Guid receiptId)
        {
            var filter = Builders<ReceiptEvent<Receipt>>.Filter.Where(x => x.Id == receiptId);
            return await _receiptCollection.Find(filter).ToListAsync();
        }
    }
}
