using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.repository
{
    public interface IWorkesRepository
    {
        Task<List<WorkersResponseDto>> GetWorkersAsync();
        Task<Workers?> GetWorkerByIdAsync(int id);
        Task<List<WorkersResponseDto>> GetWorkersWith();
        Task AddWorkerAsync(string name, string post);
        Task UpdateWorkerAsync(int id, string fullname, string post);
        Task DeleteWorkerAsync(int id);
        //Task<List<Workers>> GetListWorkersAsync();
    }
    public class WorkesRepository : IWorkesRepository
    {
        private readonly LearningDbContext _dbContext;

        public WorkesRepository(LearningDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<WorkersResponseDto>> GetWorkersAsync()
        {
           return await _dbContext.workers
                .AsNoTracking()
                .Select(w => new WorkersResponseDto
                {
                    Id = w.Id,
                    FullName = w.FullName,
                    Post = w.Post
                })
                .ToListAsync();
        }
        public async Task<Workers?> GetWorkerByIdAsync(int id)
        {
            return await _dbContext.workers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        //вывод работника вместе с транзакциями
        public async Task<List<WorkersResponseDto>> GetWorkersWith()
        {
            return await _dbContext.workers
                .AsNoTracking()
                .Select(w => new WorkersResponseDto
                {
                    Id = w.Id,
                    FullName = w.FullName,
                    Post = w.Post,
                    transactions = w.transactions
                    .Select(t => new TransactionResponseDto
                    {
                        Id = t.Id,
                        Quantity = t.Quantity,
                        Sum = t.Sum
                    }).ToList()
                })
                .ToListAsync();
        }
        public async Task AddWorkerAsync(string name, string post)
        {
            var clientsModel = new Workers
            {
                FullName = name,
                Post = post
            };

            await _dbContext.workers.AddAsync(clientsModel);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateWorkerAsync(int id, string fullname,string post)
        {
            await _dbContext.workers
                .Where(w => w.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(w => w.FullName, fullname)
                .SetProperty(w => w.Post, post));
        }
        public async Task DeleteWorkerAsync(int id)
        {
            await _dbContext.workers
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync();
        }
        //public async Task<List<Workers>> GetListWorkersAsync()
        //{
        //   var str = await _dbContext.workers
        //        .Select(w => new Workers
        //        {
        //            Id = w.Id,
        //            FullName = w.FullName
        //        })
        //        .ToListAsync();
            
        //    return str;
        //}
        //public async Task<string> Get(int id)
        //{
        //    var str = await _dbContext.workers
        //        .FirstOrDefaultAsync(w => w.Id == id);

        //    return str == null ? throw new NullReferenceException("Ошибка") : $"{str.Id} {str.FullName}";
        //}
    }
}
