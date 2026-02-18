using InventoryMaui.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryMaui.Models
{
    [Table("Продукция")]
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public List<Transactions> transactions { get; set; } = []; //m:n
    }
}
