using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using WarehouseAccounting.Model;
using WarehouseAccounting.View;
using WarehouseAccounting.ViewModel;

public class OrderViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Orders> _orders;
    private Orders _selectedOrder;

    public ObservableCollection<Orders> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
            OnPropertyChanged();
        }
    }

    public Orders SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            _selectedOrder = value;
            OnPropertyChanged();
        }
    }

    public ICommand EditOrderCommand { get; }
    public ICommand DeleteOrderCommand { get; }
    public ICommand RefreshOrderCommand { get; }
    public OrderViewModel()
    {
        RefreshOrderCommand = new RelayCommand(RefreshOrders);
        // Убираем лишний вызов FindResource — он не обязателен здесь
        EditOrderCommand = new RelayCommand<object>(EditOrder);
        DeleteOrderCommand = new RelayCommand<object>(DeleteOrder);

        LoadOrders(); // Загружаем данные
    }

    public void RefreshOrders()
    {
        LoadOrders(); // Обновляем список
    }

    public void LoadOrders()
    {
        var ordersFromDb = OrdersDB.GetDb().SelectAll();
        Orders = new ObservableCollection<Orders>(ordersFromDb);

    }

    private void EditOrder(object param)
    {
        if (param is Orders selected)
        {
            var editWindow = new EditOrderWindow(selected);
            if (editWindow.ShowDialog() == true)
            {
                OrdersDB.GetDb().Update(editWindow.EditedOrder);
                RefreshOrders();
            }
        }
    }

    private void DeleteOrder(object param)
    {
        if (param is Orders selected)
        {
            var result = MessageBox.Show(
                "Как удалить заказ?\n\n" +
                "«Да» — Отменить заказ (товары вернутся на склад)\n" +
                "«Нет» — Просто удалить заказ",
                "Подтверждение удаления",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes: // Отмена заказа
                    CancelOrder(selected);
                    break;

                case MessageBoxResult.No: // Просто удалить
                    RemoveOrder(selected);
                    break;

                case MessageBoxResult.Cancel:
                    return;
            }

            LoadOrders(); // Обновляем список после изменения
        }
    }

    private void RemoveOrder(Orders order)
    {
        if (MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            OrdersDB.GetDb().Remove(order);
            MessageBox.Show("Заказ успешно удалён.");
        }
    }

    private void CancelOrder(Orders order)
    {
        int productId;
        if (!int.TryParse(order.order_product, out productId))
        {
            MessageBox.Show("Ошибка: некорректный ID товара.");
            return;
        }

        var product = ProductsDB.GetDb().SelectAll().FirstOrDefault(p => p.product_id == productId);
        if (product == null)
        {
            MessageBox.Show("Товар не найден.");
            return;
        }

        if (!int.TryParse(product.unit, out int currentStock))
        {
            MessageBox.Show("Не удалось прочитать количество товара.");
            return;
        }

        product.unit = (currentStock + order.quantity).ToString();

        if (!ProductsDB.GetDb().Update(product))
        {
            MessageBox.Show("Ошибка при обновлении остатков на складе.");
            return;
        }

        OrdersDB.GetDb().Remove(order);
        MessageBox.Show("Заказ отменён. Товары возвращены на склад.");

        // Опционально: обновляем список товаров
        var mainWindow = Application.Current.MainWindow as Main;
        var productViewModel = mainWindow?.ProductsTabItem.DataContext as ProductViewModel;
        productViewModel?.LoadData();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}