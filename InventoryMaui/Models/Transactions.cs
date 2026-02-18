using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMaui.Models
{
    [Table("Транзакции")]
    public class Transactions
    {
        public int Id { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal Sum { get; set; } = 0;
        public DateTime Date { get; set; }
        public int ClientId { get; set; }//fk
        public int WorkerId { get; set; }//fk
        public Clients? clients { get; set; }
        public Workers? workers { get; set; }
        public List<Products> products { get; set; } = [];
    }
}