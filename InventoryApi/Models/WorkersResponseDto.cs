namespace InventoryApi.Models
{
    public class WorkersResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Post { get; set; } = string.Empty;
        public List<TransactionResponseDto> transactions { get; set; } = [];//1: много транзакций 
    }
}
