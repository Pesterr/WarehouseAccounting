using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using WarehouseAccounting.Model;
using WarehouseAccounting.ViewModel;

public class AddOrderViewModel : INotifyPropertyChanged
{
    private Orders _editedOrder;
    private Products _selectedProduct;
    private int _quantity;

    public Orders EditedOrder
    {
        get => _editedOrder;
        set
        {
            _editedOrder = value;
            OnPropertyChanged();
        }
    }

    public Products SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged();
        }
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            OnPropertyChanged();
        }
    }


    public ICommand SaveCommand { get; }
    private ObservableCollection<Products> _productsList;
    public ObservableCollection<Products> ProductsList
    {
        get => _productsList;
        set
        {
            _productsList = value;
            OnPropertyChanged();
        }
    }

    public AddOrderViewModel()
    {
        var products = ProductsDB.GetDb().SelectAll();
        if (products == null || products.Count == 0)
        {
            MessageBox.Show("Список товаров пуст.");
        }
        ProductsList = new ObservableCollection<Products>(ProductsDB.GetDb().SelectAll());
        MessageBox.Show($"Загружено товаров: {ProductsList.Count}"); // Проверка
        EditedOrder = new Orders(); // Инициализируем новый заказ
        SaveCommand = new RelayCommand(SaveOrder);
    }

    public void SaveOrder()
    {
        if (string.IsNullOrWhiteSpace(EditedOrder.client_name))
        {
            MessageBox.Show("Имя клиента не может быть пустым.");
            return;
        }

        if (SelectedProduct == null)
        {
            MessageBox.Show("Выберите товар.");
            return;
        }

        if (Quantity <= 0)
        {
            MessageBox.Show("Количество должно быть больше нуля.");
            return;
        }

        // Получаем товар
        var product = ProductsList.FirstOrDefault(p => p.product_id == SelectedProduct.product_id);

        if (product == null)
        {
            MessageBox.Show("Товар не найден.");
            return;
        }

        // Проверяем наличие
        if (!int.TryParse(product.unit, out int stock) || Quantity > stock)
        {
            MessageBox.Show($"Недостаточно товара на складе. Доступно: {product.unit}");
            return;
        }

        // Сохраняем данные заказа
        EditedOrder.order_product = SelectedProduct.product_id.ToString();
        EditedOrder.quantity = Quantity;

        if (!OrdersDB.GetDb().Insert(EditedOrder))
        {
            MessageBox.Show("Ошибка при добавлении заказа.");
            return;
        }

        // Списываем товар со склада
        product.unit = (stock - Quantity).ToString();

        if (!ProductsDB.GetDb().Update(product))
        {
            MessageBox.Show("Не удалось обновить остатки товара.");
            return;
        }

        MessageBox.Show("Заказ успешно создан!");

    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}