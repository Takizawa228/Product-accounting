using InventoryMaui.Models;
using InventoryMaui.Service;
using System.Threading.Tasks;

namespace InventoryMaui
{
    public partial class MainPage : ContentPage
    {

        private readonly ApiServiceClients _apiService;
        private readonly ApiServiceClientsDTO _apiServiceClientsDTO;
        private List<ClientResponseDto> _clientsListDTO = new List<ClientResponseDto>();
        public MainPage()
        {   
            InitializeComponent();

            _apiService = new ApiServiceClients();
            _apiServiceClientsDTO = new ApiServiceClientsDTO();
            LoadClientsAsync();
            ClientsWithTran();
            //скрыл поле
            IdUpdate.IsVisible = false;

        }
        private async void LoadClientsAsync()
        {
            ClientView.IsVisible = true;

            try
            {
                var clients = await _apiServiceClientsDTO.Get();

                if (clients != null && clients.Any())
                {
                    _clientsListDTO = clients;

                    ClientView.ItemsSource = _clientsListDTO;
                    ClientView.IsVisible = true;
                }
                else
                {
                    _clientsListDTO = new List<ClientResponseDto>();
                    ClientView.ItemsSource = _clientsListDTO;
                    ClientView.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Ошибка", $"Не удалось загрузить клиентов: {ex.Message}", "OK");
                ClientView.IsVisible = false;
            }
        }
        private async void ClientsWithTran()
        {
            try
            {
                var clients = await _apiServiceClientsDTO.GetWithTran();

                if (clients != null && clients.Any())
                {
                    _clientsListDTO = clients;

                    ClientCollectionView.ItemsSource = _clientsListDTO;

                    ClientCollectionView.IsVisible = true;
                }
                else
                {
                    _clientsListDTO = new List<ClientResponseDto>();
                    ClientCollectionView.ItemsSource = _clientsListDTO;
                    ClientCollectionView.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Ошибка", $"Не удалось загрузить клиентов: {ex.Message}", "OK");

                ClientCollectionView.IsVisible = false;
            }
        }
        private async void OpenWindow(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync("//Next");
        }

        private async void AddClient(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ModalAddClient());
            //popmodalasync
            if(new ModalAddClient() != null)
                LoadClientsAsync();
        }
        private async void DeleteClient_btn(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(IdDelete.Text, out int id);
                
                bool result = await DisplayAlertAsync("Подтвердить действие", $"Вы хотите удалить элемент?", "Да", "Нет");
                await DisplayAlertAsync("Уведомление", "Вы выбрали: " + (result ? "Удалить" : "Отменить"), "OK");

                if (result == true)
                    await _apiService.DeleteClient(id);

                LoadClientsAsync();
                Clean();
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlertAsync("Title", ex.Message, "Okay");
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(IdUpdate.Text, out int id);
                string name = fullname.Text;
                int.TryParse(passport.Text, out int pass);

                await _apiService.UpdateClient(id, name, pass);
                await DisplayAlertAsync("Результат", "Успех", "OK");

                LoadClientsAsync();
                Clean();

            }
            catch (HttpRequestException ex)
            {
                await DisplayAlertAsync("Title", ex.Message, "Okay");
            }
        }

        private async void OnClientsSelected(object sender, SelectionChangedEventArgs e)
        {
            var test = (CollectionView)sender;

            var res = test.SelectedItem as ClientResponseDto;

            bool operation = await DisplayAlertAsync("Выберите действие", $"Что хотите сделать?", "Удалить", "Изменить");
            await DisplayAlertAsync("Уведомление", "Вы выбрали: " + (operation ? "Удалить" : "Изменить"), "OK");

            if(operation == false)
            {
                IdUpdate.Text = res.Id.ToString();
                fullname.Text = res.FullName.ToString();
                passport.Text = res.Passport.ToString();
                //birthUp.Date = res.Birth.ToDateTime();
            }
            else if(operation == true)
                IdDelete.Text = res.Id.ToString();
            else
                await DisplayAlertAsync("Ошибка", "Ошибка приложения!", "OK");
        }
        private async void BtnGetTransactions(object sender, EventArgs e)
        {
            ClientsWithTran();
        }
        private void StackLayout_BindingContextChanged_1(object sender, EventArgs e)
        {

        }
        private void Clean()
        {
            IdUpdate.Text = string.Empty;
            fullname.Text = string.Empty;
            passport.Text = string.Empty;
        }
    }
}
