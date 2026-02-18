using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMaui.Models
{
    [Table("Сотрудники")]
    public class Workers
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Post { get; set; } = string.Empty;
        public List<Transactions> transactions { get; set; } = []; //1: много транзакций
    }
}
