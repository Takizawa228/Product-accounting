using InventoryMaui.Models;
using InventoryMaui.Service;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace InventoryMaui;

public partial class NewProducts : ContentPage
{
    private readonly ApiServiceWorker _apiWorker;
    private List<WorkersResponseDto> _workersList = new List<WorkersResponseDto>();
    //private List<TransactionResponseDto> _transactionResponses = new List<TransactionResponseDto>();
    public NewProducts()
	{
		InitializeComponent();  
        _apiWorker = new ApiServiceWorker();

        LoadClientsAsync();
        LoadClientsWithAsync();
    }
    private void Clean()
    {
        id.Text = string.Empty;
        idd.Text = string.Empty;
        id_delete.Text = string.Empty;
    }
    private async void LoadClientsAsync()
    {
        workersCollectionView.IsVisible = true;

        try
        {
            var workers = await _apiWorker.GetAllWorkers();

            if (workers != null && workers.Any())
            {
                _workersList = workers;
                workersCollectionView.ItemsSource = _workersList;

                workersCollectionView.IsVisible = true;
            }
            else
            {
                _workersList = new List<WorkersResponseDto>();
                workersWithView.ItemsSource = _workersList;
                workersCollectionView.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", $"Не удалось загрузить работников: {ex.Message}", "OK");
            workersCollectionView.IsVisible = false;
        }
    }
    private async void LoadClientsWithAsync()
    {
        workersWithView.IsVisible = true;

        try
        {
            var workers = await _apiWorker.GetWith();

            if (workers != null && workers.Any())
            {
                _workersList = workers;
                workersWithView.ItemsSource = _workersList;

                workersWithView.IsVisible = true;
            }
            else
            {
                _workersList = new List<WorkersResponseDto>();
                workersWithView.ItemsSource = _workersList;
                workersWithView.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", $"Не удалось загрузить работников: {ex.Message}", "OK");
            workersWithView.IsVisible = false;
        }
    }
    private async void AddWorker(object sender, EventArgs e)
    {
        try
        {
            string name = nameWorker.Text ?? throw new ArgumentNullException();
            string item = PickerItem.SelectedItem?.ToString() ?? throw new ArgumentNullException();

            if (name.Length > 100) throw new OverflowException();

            await _apiWorker.AddWorker(name,item);
            LoadClientsAsync();
            Clean();
        }
        catch(ArgumentNullException)
        {
            await DisplayAlertAsync("Ошибка", "Пустая строка!", "Ок");
        }
        catch(OverflowException)
        {
            await DisplayAlertAsync("Ошибка", "Максимальная длина 100 символов!", "Ок");
        }
    }
    private async void UptadeWorker(object sender, EventArgs e)
    {
        try
        {   int.TryParse(id.Text, out var idValue);
            string name = idd.Text;
            string item = PickerItemUp.SelectedItem?.ToString() ?? throw new ArgumentNullException();

            await _apiWorker.UptadeWorker(idValue,name, item);

            LoadClientsAsync();
            Clean();
        }
        catch (ArgumentNullException)
        {
            await DisplayAlertAsync("Ошибка", "Пустая строка!", "Ок");
        }
        catch (OverflowException)
        {
            await DisplayAlertAsync("Ошибка", "Максимальная длина 100 символов!", "Ок");
        }
    }
    private async void DeleteWorker(object sender, EventArgs e)
    {
        try
        {
            int.TryParse(id_delete.Text, out var idValue);
            
            await _apiWorker.DeleteWorker(idValue);

            LoadClientsAsync();
            Clean();
        }
        catch (ArgumentNullException)
        {
            await DisplayAlertAsync("Ошибка", "Пустая строка!", "Ок");
        }
    }
    private async void OnClientsSelected(object sender, SelectionChangedEventArgs e)
    {
        var response = (CollectionView)sender;

        var res = response.SelectedItem as WorkersResponseDto;

        bool operation = await DisplayAlertAsync("Выберите действие", $"Что хотите сделать?", "Удалить", "Изменить");
        await DisplayAlertAsync("Уведомление", "Вы выбрали: " + (operation ? "Удалить" : "Изменить"), "OK");

        if (operation == false)
        {
            id.Text = res.Id.ToString();
            idd.Text = res.FullName.ToString();
            //passport.Text = res.Passport.ToString();
            //birthUp.Date = res.Birth.ToDateTime();
        }
        else if (operation == true)
            id_delete.Text = res.Id.ToString();
        else
            await DisplayAlertAsync("Ошибка", "Ошибка приложения!", "OK");
    }
}