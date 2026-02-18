namespace InventoryApi.Models
{
    public class ClientCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public int Passport { get; set; } = 0;
        public DateOnly Birth { get; set; }
    }
}
