namespace Spents.EventSourcing.Domain.Entities
{
    public class ReceiptCreatedEntity
    {
        public string EventName { get; set; } = null!
        public Guid Id { get; set; }
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; } = null!;
    }

    public class ReceiptItem
    {
        public ReceiptItem(string name, short quantity, decimal itemPrice, string observation)
        {
            Name = name;
            Quantity = quantity;
            ItemPrice = itemPrice;
            Observation = observation;
        }

        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; }
    }
}
