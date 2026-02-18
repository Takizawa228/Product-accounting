namespace InventoryMaui.Models // Убедитесь, что это правильное имя
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
