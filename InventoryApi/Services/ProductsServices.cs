using InventoryApi.Models;
using InventoryApi.repository;
using Microsoft.IdentityModel.Tokens;

namespace InventoryApi.Services
{
    public interface IProductsServices
    {
        Task<List<Products>> GetInfoProducts();
        Task<Products> GetInfoProduct(int id);
        Task AddProduct(string name, string category, string supplier, int quantity, decimal price);
        Task UptadeProduct(int id, string name, string category, string supplier, int quantity, decimal price);
        Task DeleteProduct(int id);
        Task<int> CheckQuantity(int id, int quantity);
        Task<decimal> SummProduct(int id, int quantity);
        Task<int> DecreaseProduct(int id, int quantity);
    }
    public class ProductsServices : IProductsServices
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsServices(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
        public async Task<List<Products>> GetInfoProducts()
        {
            var products = await _productsRepository.GetAllProductsAsync();
            return products;
        }
        public async Task<Products> GetInfoProduct(int id)
        {
            if(id < 0) 
                throw new ArgumentOutOfRangeException(nameof(id),"ID продукта должен быть положительным числом.");

            var product = await _productsRepository.GetProductByIdAsync(id);

            return product ?? throw new InvalidOperationException($"Продукт с ID {id} не найден.");
        }
        public async Task AddProduct(string name, string category, string supplier, int quantity, decimal price)
        {
            if(name.IsNullOrEmpty() || category.IsNullOrEmpty() || supplier.IsNullOrEmpty())
                throw new ArgumentNullException("Заполните пустые строки!");
            if(quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Количество продукта должен быть больше 0.");
            if(price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Цена должна быть положительным числом");

            await _productsRepository.Add(name, category, supplier, quantity, price);
        }
        public async Task UptadeProduct(int id, string name, string category, string supplier, int quantity, decimal price)
        {   
            //if (name.IsNullOrEmpty())
            //    throw new ArgumentNullException("Заполните пустые строки!");

            await GetInfoProduct(id);

            await _productsRepository.Update(id, name, category, supplier, quantity, price);
        }
        public async Task DeleteProduct(int id)
        {
            await GetInfoProduct(id);

            await _productsRepository.Delete(id); 
        }
        
        public async Task<int> CheckQuantity(int id, int quantity)
        {
            await GetInfoProduct(id);

            var check = await _productsRepository.CheckQuantity(id, quantity);

            return check;
        }
        public async Task<decimal> SummProduct(int id,int quantity)
        {
            await CheckQuantity(id, quantity);

            var summ = await _productsRepository.SummProduct(id, quantity); 
            
            return summ;
        }
        public async Task<int> DecreaseProduct(int id, int quantity)
        {
            var decrease = await _productsRepository.DecreaseProduct(id,quantity);

            return decrease;
        }
    }
}
