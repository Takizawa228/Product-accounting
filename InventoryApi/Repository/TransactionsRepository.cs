using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryApi.repository
{
    public interface ITransactionsRepository
    {
        Task<List<TransactionResponseDto>> GetTransactionsAsync();
        Task<TransactionResponseDto> Add(TransactionCreateDto dto);
        Task<Transactions?> GetId(int id);
        Task Delete(int id);
        Task<List<TransactionResponseDto>> GetDate(DateTime date1, DateTime date2);
    }
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly LearningDbContext _dbContext;
        public TransactionsRepository(LearningDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<List<TransactionResponseDto>> GetTransactionsAsync()
        {
            var transactions = await _dbContext.transactions
            .Include(t => t.products)
            .Include(t => t.clients)
            .Include(t => t.workers)
            .ToListAsync();

            return transactions.Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                Quantity = t.Quantity,
                Sum = t.Sum,
                Date = t.Date,
                ClientId = t.ClientId,
                ClientName = t.clients?.FullName ?? "Не указан",
                WorkerId = t.WorkerId,
                WorkerName = t.workers?.FullName ?? "Не указан",
                Products = t.products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Supplier = p.Supplier,
                    Price = p.Price
                }).ToList()
            }).ToList();
        }
        public async Task<Transactions?> GetId(int id)
        {
            return await _dbContext.transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
        }
        public async Task<TransactionResponseDto> Add(TransactionCreateDto dto)
        {
            var products = await _dbContext.products
                .Where(t => dto.ProductIds.Contains(t.Id))
                .ToListAsync();
           
            if (products.Count != dto.ProductIds.Count) 
                throw new Exception($"Нет такого продукта под id: {dto.ProductIds}");
            
            var transactions = new Transactions
            {
                Quantity = dto.Quantity,
                Sum = dto.Sum,
                Date = dto.Date,
                ClientId = dto.ClientId,
                WorkerId = dto.WorkerId,
                products = products
                
            };

            await _dbContext.transactions.AddAsync(transactions);
            await _dbContext.SaveChangesAsync();

            return new TransactionResponseDto
            {
                Id = transactions.Id,
                Date = transactions.Date,
                Sum = transactions.Sum,
                Products = transactions.products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Supplier = p.Supplier,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList()
            };
        }
        public async Task Delete(int id)
        {
            await _dbContext.transactions
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync();
        }
        public async Task<List<TransactionResponseDto>> GetDate(DateTime date1,DateTime date2)
        {
            var transactions = await _dbContext.transactions
                .AsNoTracking()
                .Include(t => t.products)
                .Include(t => t.clients)
                .Include(t => t.workers)
                .ToListAsync();

            var filter = transactions.Where(d => d.Date >= date1 && d.Date <= date2);

            return filter.Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                Quantity = t.Quantity,
                Sum = t.Sum,
                Date = t.Date,
                ClientId = t.ClientId,
                ClientName = t.clients?.FullName ?? "Нет данных",
                WorkerId = t.WorkerId,
                WorkerName = t.workers?.FullName ?? "Нет данных",
                Products = t.products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Supplier = p.Supplier,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList()
            }).ToList();
        }
    }
}
