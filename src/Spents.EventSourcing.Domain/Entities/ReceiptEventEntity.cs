namespace Spents.EventSourcing.Domain.Entities
{
    public class ReceiptEventEntity
    {
        public Guid Id { get; set; }
        public string EventName { get; set; } = null!;
        public Receipt Receipt { get; set; } = null!;
    }

    public class Receipt
    {
        public Receipt(string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems)
        {
            EstablishmentName = establishmentName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }

        public string EstablishmentName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; }
    }

    public class ReceiptItem
    {
        public ReceiptItem(Guid id, string name, short quantity, decimal itemPrice, string observation)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ItemPrice = itemPrice;
            Observation = observation;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; }
    }
}
