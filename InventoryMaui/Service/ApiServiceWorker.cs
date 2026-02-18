using InventoryMaui.Models;
using Microsoft.Maui.Controls.Platform;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace InventoryMaui.Service
{   
    public class ApiServiceWorker
    {
        private readonly string _apiUrl = "http://localhost:5154/Workers/";
        private readonly HttpClient _httpClient;
        public ApiServiceWorker()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<WorkersResponseDto>> GetAllWorkers()
        {
            var response = await _httpClient.GetAsync("all-workers");

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<WorkersResponseDto>>();
                
                return content ?? [];
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task<List<WorkersResponseDto>> GetWith()
        {
            var response = await _httpClient.GetAsync("all-with");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<WorkersResponseDto>>();

                return content ?? [];
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {error}");
            }
        }
        public async Task AddWorker(string name, string post)
        {
            var worker = new Workers
            {
                FullName = name,
                Post = post
            };
            JsonContent content = JsonContent.Create(worker);

            var response = await _httpClient.PostAsync("add-worker", content);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync();
        }
        public async Task UptadeWorker(int id,string name, string post)
        {
            var worker = new Workers
            {   
                Id = id,
                FullName = name,
                Post = post
            };
            JsonContent content = JsonContent.Create(worker);

            var response = await _httpClient.PutAsync($"uptade-worker/{id}", content);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync();
        }
        public async Task DeleteWorker(int id)
        {
            var responce = await _httpClient.DeleteAsync($"delete-worker/{id}");

            responce.EnsureSuccessStatusCode();

            await responce.Content.ReadAsStringAsync();
        }
        public async Task<List<string>> GetWorkers()
        {
            var response = await _httpClient.GetAsync("get-workers");
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                //var res = response.Content.ReadFromJsonAsync<List<string>>();
                string content = await response.Content.ReadAsStringAsync();

                var workers = JsonSerializer.Deserialize<List<string>>(content, options);

                return workers;
            }
            else
            {
                throw new Exception($"Ошибка API: {response.StatusCode}");
            }    
        }
    }
}
