using InventoryMaui.Models;
using InventoryMaui.Service;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
namespace InventoryMaui;

public partial class Next : ContentPage
{
	private readonly ApiServiceProducts _apiServiceProducts;
    private readonly ApiServiceClients _apiServiceClients;
    private readonly ApiServiceWorker _apiServiceWorker;
    private readonly ApiServiceTransactions _apiServiceTransactions;
    private List<Products> _productList = new List<Products>();
    public Next()
	{
		InitializeComponent();

		_apiServiceProducts = new ApiServiceProducts();
        _apiServiceClients = new ApiServiceClients();
        _apiServiceWorker = new ApiServiceWorker();
        _apiServiceTransactions = new ApiServiceTransactions();

        AddClients();
        AddWorkers();
        ListProducts();

        idUp.IsVisible = false;
    }
    private void Clean()
    {
        idUp.Text = string.Empty;
        nameUp.Text = string.Empty;
        nameSuppUp.Text = string.Empty;
        quantityUp.Text = string.Empty;
        priceUp.Text = string.Empty;
    }
    private async void OnWorkerSelected(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        
        var selectedProduct = collectionView.SelectedItem as Products;

        bool operation = await DisplayAlertAsync("Выберите действие", $"Что хотите сделать?", "Удалить", "Изменить");
        await DisplayAlertAsync("Уведомление", "Вы выбрали: " + (operation ? "Удалить" : "Изменить"), "OK");

        if (operation == false)
        {
            idUp.Text = selectedProduct.Id.ToString();
            nameUp.Text = selectedProduct.Name;

            nameSuppUp.Text = selectedProduct.Supplier;
            quantityUp.Text = selectedProduct.Quantity.ToString();
            priceUp.Text = selectedProduct.Price.ToString();
        }
        else if (operation == true)
            id_delete.Text = selectedProduct.Id.ToString();
        else
            await DisplayAlertAsync("Ошибка", "Ошибка приложения!", "OK");
    }
    private async void AddClients()
    {
        var names = await _apiServiceClients.GetAllClients();

        foreach (var name in names)
        {
            GetClients.Items.Add($"Id: {name.Id} имя {name.FullName}");
        }
    }
    private async void AddWorkers()
    {
        var names = await _apiServiceWorker.GetAllWorkers();

        foreach (var name in names)
        {
            GetWorkers.Items.Add($"Id: {name.Id} имя {name.FullName}");
        }
    }
    private async void ListProducts()
    {
        //Показываем индикатор загрузки и скрываем CollectionView

        productsCollectionView.IsVisible = false;
        noDataLabel.IsVisible = false;

        try
        {
            var products = await _apiServiceProducts.GetAllProducts();

            if (products != null && products.Any())
            {
                _productList = products;
                productsCollectionView.ItemsSource = _productList;

                productsCollectionView.IsVisible = true;
            }
            else
            {
                noDataLabel.IsVisible = true;
                productsCollectionView.IsVisible = false;
            }
            
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ошибка", $"Не удалось загрузить продукты: {ex.Message}", "OK");
            noDataLabel.IsVisible = true; // Показываем сообщение об ошибке
            productsCollectionView.IsVisible = false;
        }
    }
    private async void Btn_AddProduct(object sender, EventArgs e)
    {
        try
        {
            string name = nameProduct.Text;
            var category = PickerItem.SelectedItem?.ToString();
            string supp = nameSupp.Text;
            int.TryParse(quantity.Text, out int num);
            decimal.TryParse(price.Text, out decimal pricep);

            await _apiServiceProducts.AddProduct(name, category, supp, num, pricep);
            await DisplayAlertAsync("Добавлен продукт", "Успех", "Ок");
        }
        catch (HttpRequestException httpEx)
        {
            await DisplayAlertAsync("Ошибка", httpEx.Message, "Ок");
        }
    }
    private async void Btn_UpdateProduct(object sender, EventArgs e)
    {
        try
        {
            int.TryParse(idUp.Text, out int num);
            string nameP = nameUp.Text;
            var picker = PickerItemUp.SelectedItem?.ToString();
            string supp = nameSuppUp.Text;
            int.TryParse(quantityUp.Text, out int count);
            decimal.TryParse(priceUp.Text, out decimal price);
            await _apiServiceProducts.UptadeProduct(num, nameP, picker, supp, count, price);

            await DisplayAlertAsync($"Изменен продукт под id: {num}", "Успех", "Ок");
            ListProducts();
            Clean();
        }
        catch(HttpRequestException httpEx)
        {
            await DisplayAlertAsync("Ошибка", httpEx.Message, "Ок");
        }
    }
    private async void Btn_DeleteProduct(object sender, EventArgs e)
    {
        try
        {
            int.TryParse(id_delete.Text, out int num);

            var item = await _apiServiceProducts.GetId(num);
            
            bool result = await DisplayAlertAsync("Подтвердить действие", $"Вы хотите удалить элемент?\n {item.Name}", "Да", "Нет");
            await DisplayAlertAsync("Уведомление", "Вы выбрали: " + (result ? "Удалить" : "Отменить"), "OK");

            if (result == true)
                await _apiServiceProducts.DeleteProduct(num);
            //await DisplayAlertAsync($"Удален продукт под id: {num}", "Успех", "Ок");
            Clean();
        }
        catch(HttpRequestException httpEx)
        {
            await DisplayAlertAsync("Ошибка", httpEx.Message, "Ок");
        }
    }
    private async void Btn_AddTran(object sender, EventArgs e)
    {
        try
        {
            int.TryParse(GetQuantityTran.Text, out int numQuantity);
            DateTime date = (DateTime)GetDataTran.Date;

            var clients = GetClients.SelectedItem?.ToString();
            string[] c = clients.Split();
            int.TryParse(c[1], out int IdClient);

            var workers = GetWorkers.SelectedItem?.ToString();
            string[] w = workers.Split();
            int.TryParse(w[1], out int IdWorker);

            int.TryParse(GetIdTran.Text, out int numId);
            List<int> ids = [numId];

            await _apiServiceProducts.CheckQuantity(numId, numQuantity);
            var result = await _apiServiceProducts.SummGet(numId, numQuantity);

            var dto = new TransactionCreateDto
            {
                Quantity = numQuantity,
                Sum = result,
                Date = date,
                ClientId = IdClient,
                WorkerId = IdWorker,
                ProductIds = new List<int> { numId }
            };
            await _apiServiceTransactions.Add(dto);
            await _apiServiceProducts.DecreaseGet(numId, numQuantity);

            await DisplayAlertAsync("Проведена транзакция,Успешно!", $"\nПокупатель {c[3]}\nСотрудник {w[3]}" +
                $"\nНа сумму: {result}", "Ок");
            Clean();
        }
        catch (HttpRequestException httpEx)
        {
            await DisplayAlertAsync("Ошибка", httpEx.Message, "Ок");
        }
    }
}