using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMaui.Models
{
    [Table("Клиенты")]
    public class Clients
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Passport { get; set; } = 0;  
        public DateOnly Birth { get; set; }
        public List<Transactions> transactions { get; set; } = [];//1: много транзакций 
    }
}
