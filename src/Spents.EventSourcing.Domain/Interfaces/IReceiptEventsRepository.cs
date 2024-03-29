﻿using Spents.Domain.Entities;
using Spents.Events.v1;

namespace Spents.EventSourcing.Domain.Interfaces
{
    public interface IReceiptEventsRepository
    {
        Task<IReadOnlyCollection<ReceiptEvent<Receipt>>> GetAllEventsAsync(Guid receiptId);
        Task AddReceiptCreatedEventAsync(ReceiptEvent<Receipt> receipt);
    }
}
