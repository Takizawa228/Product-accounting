using InventoryApi.Models;
using InventoryApi.repository;
using Microsoft.IdentityModel.Tokens;

namespace InventoryApi.Services
{
    public interface IWorkersServices
    {
        Task<List<WorkersResponseDto>> GetInfoWorker();
        Task<List<WorkersResponseDto>> GetWithTran();
        Task<string> GetInfoWorker(int id);
        Task AddNewWorker(string name, string post);
        Task UpdateWorker(int id, string fullname, string post);
        Task DeleteWorker(int id);

    }
    public class WorkersServices : IWorkersServices
    {
        private readonly IWorkesRepository _workesRepository;

        public WorkersServices(IWorkesRepository workesRepository) 
        {
            _workesRepository = workesRepository;
        }
        public async Task<List<WorkersResponseDto>> GetInfoWorker()
        {
            var worker = await _workesRepository.GetWorkersAsync();

            return worker;
        }
        public async Task<List<WorkersResponseDto>> GetWithTran()
        {
            var worker = await _workesRepository.GetWorkersWith();

            return worker;
        }
        public async Task<string> GetInfoWorker(int id)
        {
            if(id <= 0)
                throw new Exception($"Неверный идентификатор: {id}");
  
            var worker = await _workesRepository.GetWorkerByIdAsync(id) ?? 
                throw new Exception($"Не найден работник с id: {id}");

            return $"Работник {worker.FullName}, должность {worker.Post}";
        }
        public async Task AddNewWorker(string name, string post)
        {
            if(name.IsNullOrEmpty())
                throw new Exception($"Пустая строка: {name}");
            if(post.IsNullOrEmpty())
                throw new Exception($"Пустая строка: {post}");
            if(name.Length > 100)
                throw new ArgumentOutOfRangeException($"Превышена максимальная длина: {name}");

            await _workesRepository.AddWorkerAsync(name, post);
        }
        public async Task UpdateWorker(int id, string fullname, string post)
        {
            if (id <= 0)
                throw new ArgumentException($"Неверный идентификатор: {id}");
            if (fullname.IsNullOrEmpty())
                throw new ArgumentException($"Пустая строка {fullname}");
            if(post.IsNullOrEmpty())
                throw new ArgumentException($"Пустая строка {post}");
            if (fullname.Length > 100)
                throw new ArgumentOutOfRangeException($"Превышена максимальная длина: {fullname}");

            var worker = await _workesRepository.GetWorkerByIdAsync(id) ?? 
                throw new Exception($"Не найден работник с id: {id}");

            await _workesRepository.UpdateWorkerAsync(id, fullname, post);
        }
        public async Task DeleteWorker(int id)
        {
            if (id <= 0)
                throw new ArgumentException($"Неверный идентификатор: {id}");

            var worker = await _workesRepository.GetWorkerByIdAsync(id) ??
                throw new Exception($"Не найден работник с id: {id}");

            await _workesRepository.DeleteWorkerAsync(id);
        }
    }
}
