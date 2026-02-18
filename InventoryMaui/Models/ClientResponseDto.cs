namespace InventoryMaui.Models
{
    public class ClientResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Passport { get; set; } = 0;
        public DateOnly Birth { get; set; }
        public List<TransactionResponseDto> transactions { get; set; } = [];//1: много транзакций 
    }
}
