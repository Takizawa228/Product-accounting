namespace InventoryApi.Models
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; }   
    }
}
