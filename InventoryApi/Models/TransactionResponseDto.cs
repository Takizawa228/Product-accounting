namespace InventoryApi.Models
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public int WorkerId { get; set; }
        public string WorkerName { get; set; } = string.Empty;

        // Вместо сущностей Products, возвращаем список легких DTO
        public List<ProductResponseDto> Products { get; set; } = [];
    }
}
