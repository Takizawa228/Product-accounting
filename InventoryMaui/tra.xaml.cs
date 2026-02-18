using InventoryMaui.Models;
using InventoryMaui.Service;
using System.Transactions;

namespace InventoryMaui;

public partial class tra : ContentPage
{
   
    private readonly ApiServiceTransactions _apiServiceTransactions; 
    private List<TransactionResponseDto> _transactionList = new List<TransactionResponseDto>();

    public tra()
	{
		InitializeComponent();

        _apiServiceTransactions = new ApiServiceTransactions();
        GetList();

    }
	private async void GetList()
	{
        TransactionView.IsVisible = false;

        try
        {
            var transaction = await _apiServiceTransactions.GetList();

            if (transaction != null && transaction.Any())
            {
                _transactionList = transaction;
                TransactionView.ItemsSource = _transactionList;

                TransactionView.IsVisible = true;
            }
            else
            {
                TransactionView.IsVisible = false;
            }

        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", $"Не удалось загрузить продукты: {ex.Message}", "OK");
            TransactionView.IsVisible = false;
        }
    }
}