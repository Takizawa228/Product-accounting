using InventoryMaui.Models;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace InventoryMaui.Service
{
    public partial class ApiServiceTransactions
    {
        private readonly string _apiUrl = "http://localhost:5154/Transactions/";
        private readonly HttpClient _httpClient;
        
        public ApiServiceTransactions()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }
        public async Task<List<TransactionResponseDto>> GetList()
        {
            var response = await _httpClient.GetAsync("all-transactions");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<TransactionResponseDto>>();

                return content ?? [];
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task<TransactionResponseDto> Add(TransactionCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("add-transaction", dto);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<TransactionResponseDto>();

                return content;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
    }
}
