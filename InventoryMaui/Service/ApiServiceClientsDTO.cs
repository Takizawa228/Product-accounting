using InventoryMaui.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InventoryMaui.Service
{
    public class ApiServiceClientsDTO
    {
        private readonly string _apiUrl = "http://localhost:5154/Clients/";
        private readonly HttpClient _httpClient;
        public ApiServiceClientsDTO()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<ClientResponseDto>> GetWithTran()
        {
            var response = await _httpClient.GetAsync("client-tran");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<ClientResponseDto>>();

                return content ?? [];
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        
        





        public async Task<List<ClientResponseDto>> Get()
        {
            var clients = await _httpClient.GetAsync("tran");

            if(clients.IsSuccessStatusCode)
            {
                var content = await clients.Content.ReadFromJsonAsync<List<ClientResponseDto>>();

                return content ?? [];
            }
            else
            {
                var errorMessage = await clients.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {clients.StatusCode}. Сообщение: {errorMessage}");
            }    
        }
    }
}
