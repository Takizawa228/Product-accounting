using InventoryMaui.Models;
using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace InventoryMaui.Service
{
    public class ApiServiceClients
    {//localhost:5154/Clients
        private readonly string _apiUrl = "http://localhost:5154/Clients/";
        private readonly HttpClient _httpClient;
        public ApiServiceClients()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<Clients?> GetClients(int id)
        {
            try
            {
                var res = await _httpClient.GetAsync($"client-info/{id}");
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadFromJsonAsync<Clients>();
                else
                    return null;

            }
            catch (Exception ex) {
                {
                    Console.WriteLine($"ошибка! {ex.Message}");
                    return null;
                }
            } 
        }
        public async Task<List<Clients>> GetAllClients()
        {          
            var response = await _httpClient.GetAsync("all-client");

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<Clients>>();

                return content ?? [];
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task<string> AddClientAsync(string name, int pass, DateOnly birth)
        {
            try
            {
                // Формируем URL с query параметрами
                var url = $"add-client?name={Uri.EscapeDataString(name)}&passport={pass}&birth={birth:yyyy-MM-dd}";

                // Отправляем POST-запрос без тела
                var response = await _httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Ошибка: {response.StatusCode} - {errorContent}";
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка при отправке запроса: {ex.Message}";
            }
        }
        public async Task<string> DeleteClient(int id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"delete-client/{id}");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return $"Удален: {content}";
                }
                else
                {
                    return $"Ошибка: {result.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка при отправке запроса {ex.Message}";
            }
        }
        public async Task UpdateClient(int id, string name, int passport)
        {
            var content = new Clients
            {
                FullName = name,
                Passport = passport
            };

            var response = await _httpClient.PutAsJsonAsync($"update-client/{id}", content);

            if (response.IsSuccessStatusCode)
                await response.Content.ReadAsStringAsync();
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task<List<string>> GetClient()
        {
            var res = await _httpClient.GetAsync("get-client");

            if(res.IsSuccessStatusCode)
            {
                
                string content = await res.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var clients = JsonSerializer.Deserialize
                    <List<string>>(content,options);

                return clients;
            }
            else
            {
                throw new Exception($"Ошибка API: {res.StatusCode}");
            }
        }
    }
}
