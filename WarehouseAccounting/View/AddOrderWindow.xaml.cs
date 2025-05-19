using System.Windows;
using WarehouseAccounting.ViewModel;

namespace WarehouseAccounting.View
{
    public partial class AddOrderWindow : Window
    {
        private readonly OrderViewModel _orderViewModel;
        private readonly AddOrderViewModel _viewModel;

        public AddOrderWindow(OrderViewModel orderViewModel)
        {
            InitializeComponent();
            _orderViewModel = orderViewModel;
            _viewModel = new AddOrderViewModel();
            DataContext = _viewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveOrder(); // Сохраняем заказ

            // Получаем главную вкладку
            var mainWindow = Application.Current.MainWindow as Main;

            // Обновляем список заказов
            var orderViewModel = mainWindow?.MainTabControl.DataContext as OrderViewModel;
            orderViewModel?.RefreshOrders();

            // Обновляем список товаров
            var productViewModel = mainWindow?.ProductsTabItem.DataContext as ProductViewModel;
            productViewModel?.LoadData();

            DialogResult = true;
            Close();
        }
        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AddOrderViewModel;
            if (viewModel?.ProductsList == null || viewModel.ProductsList.Count == 0)
            {
                MessageBox.Show("Список товаров пуст");
            }
            else
            {
                string list = string.Join(Environment.NewLine, viewModel.ProductsList.Select(p => p.product_name));
                MessageBox.Show(list, "Список товаров");
            }
        }
    }
}