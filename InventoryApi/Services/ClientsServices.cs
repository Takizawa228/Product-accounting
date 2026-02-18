using InventoryApi.Models;
using InventoryApi.repository;

namespace InventoryApi.Services
{
    public interface IClientsServices
    {
        Task<List<Clients>> GetAllClient();
        Task<string> GetInfoClient(int id);
        Task<string> AddClient(string name, int passport, DateOnly birth);
        Task UpdateClient(int id, string name, int passport);
        Task<string> DeleteClient(int id);
        Task<List<ClientResponseDto>> GetListTran();
        Task<List<ClientResponseDto>> Get();
    }
    public class ClientsServices : IClientsServices
    {
        private readonly IClientsRepository _cleintsRepository;

        public ClientsServices(IClientsRepository cleintsRepository) 
        {
            _cleintsRepository = cleintsRepository;
        }
        public async Task<List<Clients>> GetAllClient()
        {
            var client = await _cleintsRepository.GetClientsAsync();
            return client;
        }
        public async Task<string> GetInfoClient(int id)
        {
            var client = await _cleintsRepository.GetClientsByIdAsync(id);
            return $"Name : {client?.FullName} {client?.Passport} {client?.Birth}";
        }
        public async Task<string> AddClient(string name, int passport, DateOnly birth)
        {
            await _cleintsRepository.Add(name, passport, birth);
            return $"Добавлен новый клиент: {name}";
        }
        public async Task UpdateClient(int id, string name, int passport)
        {
            await _cleintsRepository.Update(id, name, passport);
        }
        public async Task<string> DeleteClient(int id)
        {
            await _cleintsRepository.Delete(id);
            return $"Удален клиент: {id}";
        }
        public async Task<List<ClientResponseDto>> GetListTran()
        {
            var clients = await _cleintsRepository.GetWithTran();

            return clients;
        }
        public async Task<List<ClientResponseDto>> Get()
        {
            var clients = await _cleintsRepository.Get();

            return clients;
        }
    }
}
