using InventoryApi.Models;
using InventoryApi.repository;

namespace InventoryApi.Services
{
    public interface ITransactionsServices
    {
        Task<List<TransactionResponseDto>> GetList();
        Task<TransactionResponseDto> Add(TransactionCreateDto dto);
    }
    public class TransactionsServices : ITransactionsServices
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsServices(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }
        public async Task<List<TransactionResponseDto>> GetList()
        {
            var transaction = await _transactionsRepository.GetTransactionsAsync();

            return transaction;
        }
  
        public async Task<TransactionResponseDto> Add(TransactionCreateDto dto)
        {
            var transaction = await _transactionsRepository.Add(dto);

            return transaction;
        }
    }
}
