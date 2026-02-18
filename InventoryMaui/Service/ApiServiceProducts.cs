using InventoryMaui.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace InventoryMaui.Service
{
    public partial class ApiServiceProducts
    {
        private readonly string _apiUrl = "http://localhost:5154/Products/";
        private readonly HttpClient _httpClient;
        public ApiServiceProducts()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_apiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<Products>> GetAllProducts()
        {
            try
            {
                var res = await _httpClient.GetStringAsync("all-products");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<List<Products>>(res, options) ?? new List<Products>();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка! {ex.Message}");
                return new List<Products>();
            }
        }
        public async Task<Products> GetId(int id)
        {
            var response = await _httpClient.GetAsync($"product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string content = await response.Content.ReadAsStringAsync();

                var product = JsonSerializer.Deserialize<Products>(content, options);

                return product ?? throw new Exception("Не удалось десериализовать продукт");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task AddProduct(string name,string category,string supplier,int quantity,decimal price)
        {
            var product = new Products
            {
                Name = name,
                Category = category,
                Supplier = supplier,
                Quantity = quantity,
                Price = price
            };

            JsonContent content = JsonContent.Create(product);

            var response = await _httpClient.PostAsync("add-product", content);
                
            if(response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }      
        }
        public async Task UptadeProduct(int id, string name, string category, string supplier, int quantity, decimal price)
        {
            var product = new Products
            {
                Name = name,
                Category = category,
                Supplier = supplier,
                Quantity = quantity,
                Price = price
            };

            JsonContent content = JsonContent.Create(product);

            var response = await _httpClient.PutAsync($"uptade-product/{id}", content);

            if (response.IsSuccessStatusCode)
                await response.Content.ReadAsStringAsync();
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"delete-product/{id}");

            if(response.IsSuccessStatusCode)
                await response.Content.ReadAsStringAsync();
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task<int> CheckQuantity(int id, int quantity)
        {
            var response = await _httpClient.GetAsync($"check-stock/{id}/{quantity}");

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                return quantity;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task<decimal> SummGet(int id, int quantity)
        {
            var response = await _httpClient.GetAsync($"summ/{id}/{quantity}");

            if (response.IsSuccessStatusCode)
            {
                string stringResult = await response.Content.ReadAsStringAsync();

                decimal result = JsonSerializer.Deserialize<decimal>(stringResult);
                
                return result;  
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
        public async Task<int> DecreaseGet(int id, int quantity)
        {
            var response = await _httpClient.PutAsync($"decrease/{id}/{quantity}",null);

            if (response.IsSuccessStatusCode)
            {
                //ReadFromJsonAsync получаем строку и сразу десериализуем в нужный нам тип
                int result = await response.Content.ReadFromJsonAsync<int>();

                return result;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Ошибка: {response.StatusCode}. Сообщение: {errorMessage}");
            }
        }
    }
}
