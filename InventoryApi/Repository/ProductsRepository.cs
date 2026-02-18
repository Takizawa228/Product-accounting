using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.repository
{
    public interface IProductsRepository
    {
        Task<List<Products>> GetAllProductsAsync();
        Task<Products?> GetProductByIdAsync(int id);
        Task<List<Products>> GetAllWithProductsAsync();
        Task Add(string name, string category, string supplier, int quantity, decimal price);
        Task Update(int id, string name, string category, string supplier, int quantity, decimal price);
        Task Delete(int id);
        Task<int> CheckQuantity(int id, int quantity);
        Task<decimal> SummProduct(int id, int quantity);
        Task<int> DecreaseProduct(int id, int quantity);
    }
    public class ProductsRepository : IProductsRepository
    {
        private LearningDbContext _dbContext;

        public ProductsRepository(LearningDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<List<Products>> GetAllProductsAsync()
        {
           return await _dbContext.products
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Products>> GetAllWithProductsAsync()
        {
            return await _dbContext.products
                .AsNoTracking()
                .Include(p => p.transactions)
                .ToListAsync();
        }
        public async Task<Products?> GetProductByIdAsync(int id)
        {
            return await _dbContext.products.
                AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task Add(string name, string category, string supplier, int quantity, decimal price)
        {
            var productsModel = new Products
            {
                Name = name,
                Category = category,
                Supplier = supplier,
                Quantity = quantity,
                Price = price
            };
            await _dbContext.products.AddAsync(productsModel);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(int id, string name, string category, string supplier, int quantity, decimal price)
        {
            await _dbContext.products
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Name, name)
                .SetProperty(p => p.Category, category)
                .SetProperty(p => p.Supplier, supplier)
                .SetProperty(p => p.Quantity, quantity)
                .SetProperty(p => p.Price, price));
        }
        public async Task Delete(int id)
        {
            await _dbContext.products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task<int> CheckQuantity(int id,int quantity)
        {
            var product = await _dbContext.products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => p.Quantity)
                .FirstOrDefaultAsync();

            if (quantity > product)
                throw new ArgumentOutOfRangeException("Ошибка");
            else
                return product;
        }
        public async Task<decimal> SummProduct(int id,int quantity)
        {
            var product = await _dbContext.products
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Ошибка");
             
            decimal summ = quantity * product.Price;
            return summ;
        }
        public async Task<int> DecreaseProduct(int id,int quantity)
        {
            var product = await _dbContext.products
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Ошибка");

            product.Quantity -= quantity;

            await _dbContext.SaveChangesAsync();

            return product.Quantity;
        }
    }
}

