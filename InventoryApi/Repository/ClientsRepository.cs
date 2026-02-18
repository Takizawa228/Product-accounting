using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.repository
{
    public interface IClientsRepository
    {
        Task<List<Clients>> GetClientsAsync();
        Task<Clients?> GetClientsByIdAsync(int id);
        Task<List<ClientResponseDto>> GetWithTran();
        Task<List<ClientResponseDto>> Get();
        Task Add(string name, int passport, DateOnly birth);
        Task Update(int id, string name, int passport);
        Task Delete(int id);
    }
    public class ClientsRepository : IClientsRepository
    {
        
        private readonly LearningDbContext _dbContext;

        public ClientsRepository(LearningDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Clients>> GetClientsAsync()
        {
            return await _dbContext.clients
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<ClientResponseDto>> Get()
        {
            return await _dbContext.clients
                .AsNoTracking()
                .Select(c => new ClientResponseDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Passport = c.Passport,
                    Birth = c.Birth
                })
                .ToListAsync();
        }
        public async Task<Clients?> GetClientsByIdAsync(int id)
        {
            return await _dbContext.clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        //вывод клиента вместе с транзакциями
        public async Task<List<ClientResponseDto>> GetWithTran()
        {
            var clientDtos = await _dbContext.clients
            .AsNoTracking() // Отключаем отслеживание, так как только читаем данные
            .Include(c => c.transactions) // EF Core загружает связанные транзакции (Lazy/Eager Loading)
            .Select(c => new ClientResponseDto
            {
                // Проекция полей клиента
                Id = c.Id,
                FullName = c.FullName,
                Passport = c.Passport,
                Birth = c.Birth,

                // Проекция списка транзакций (1:M)
                Transactions = c.transactions
                    .Select(t => new TransactionResponseDto
                    {
                        // Проекция полей транзакции
                        Id = t.Id,
                        Quantity = t.Quantity,
                        Sum = t.Sum,
                        Date = t.Date,
                        ClientId = t.ClientId,
                        WorkerId = t.WorkerId,
                    })
                    .ToList()
            })
            .ToListAsync();

            return clientDtos;
        }
        public async Task Add(string name, int passport, DateOnly birth)
        {
            var clientsModel = new Clients
            {
                FullName = name,
                Passport = passport,
                Birth = birth
            };

            await _dbContext.clients.AddAsync(clientsModel);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(int id, string name, int passport)
        {
            await _dbContext.clients
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.FullName, name)
                .SetProperty(c => c.Passport, passport));
        }
        public async Task Delete(int id)
        {
            await _dbContext.clients
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }
        public void UpdateExec(int id, string name)
        {
            _dbContext.Database.ExecuteSqlRaw("EXEC ClientUpdate @name, @id",
                new SqlParameter("@name", name),
                new SqlParameter("@id", id)
                );
        }
    }
}
