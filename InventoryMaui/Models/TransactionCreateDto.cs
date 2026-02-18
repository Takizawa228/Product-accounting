namespace InventoryMaui.Models 
{
    public class TransactionCreateDto
    {
        public int Quantity { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public int WorkerId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}
