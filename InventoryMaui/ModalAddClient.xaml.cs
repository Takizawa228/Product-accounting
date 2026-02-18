using InventoryMaui.Models;
using InventoryMaui.Service;

namespace InventoryMaui;

public partial class ModalAddClient : ContentPage
{
    private readonly ApiServiceClients _apiService;
   
    public ModalAddClient()
	{
		InitializeComponent();
        _apiService = new ApiServiceClients();
	}
    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Получаем данные из полей ввода


        string name = full.Text?.Trim() ?? ""; // Добавил проверку на null

        // Проверяем, что имя не пустое
        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Ошибка", "Поле 'ФИО' обязательно для заполнения", "OK");
            return;
        }

        if (!int.TryParse(pass.Text, out int passValue) || passValue <= 0)
        {
            await DisplayAlert("Ошибка", "Некорректный номер паспорта", "OK");
            return;
        }

        DateOnly birthDate = DateOnly.FromDateTime(birth.Date.Value);


        // 6. Вызываем API
        var result = await _apiService.AddClientAsync(name, passValue, birthDate);

        // 7. Показываем результат
        await DisplayAlert("Результат", result, "OK");

    }
}