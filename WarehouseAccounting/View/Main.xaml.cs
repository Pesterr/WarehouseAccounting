using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WarehouseAccounting.ViewModel;
using WarehouseAccounting.Model;

namespace WarehouseAccounting.View
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Main : Window
    {
        
        public Main()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();

        }

        public ProductViewModel ProductViewModel { get; set; }

        public ProfileViewModel ProfileViewModel { get; set; }
        private OrderViewModel _orderViewModel;

        private readonly ProductViewModel _productViewModel;
        private readonly ProfileViewModel _profileViewModel;

        public Main(Employees currentUser)
        {
            InitializeComponent();

            // Создаём ViewModel'и с правильными данными
            _productViewModel = new ProductViewModel();
            _orderViewModel = new OrderViewModel();
            _profileViewModel = new ProfileViewModel(currentUser); // Передаем пользователя

            // Устанавливаем DataContext для вкладок
            ProductsTabItem.DataContext = _productViewModel;
            MainTabControl.DataContext = _orderViewModel;
            ProfileTabItem.DataContext = _profileViewModel; // Теперь привязываем профиль к данным
            DataContext = this;
        }

        private void ProductAdd(object sender, RoutedEventArgs e)
        {
            var productsAdd = new ProductsAdd();
            productsAdd.Show();

        }
        private void OrderAdd(object sender, RoutedEventArgs e)
        {
            var orderViewModel = MainTabControl.DataContext as OrderViewModel;
            if (orderViewModel != null)
            {
                var addOrderWindow = new AddOrderWindow(orderViewModel);
                if (addOrderWindow.ShowDialog() == true)
                {
                    orderViewModel.RefreshOrders(); // Обновляем список заказов
                }
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProducts.SelectedItem is Products selectedProduct)
            {
                var editWindow = new ProductEditWindow(selectedProduct);
                if (editWindow.ShowDialog() == true)
                {
                    
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования.");
            }
        }
        
        private void OldPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                ((ProfileViewModel)DataContext).OldPassword = passwordBox.Password;
            }
        }

        private void NewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                ((ProfileViewModel)DataContext).NewPassword = passwordBox.Password;
            }
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                ((ProfileViewModel)DataContext).ConfirmPassword = passwordBox.Password;
            }
        }
        private void LoadOrders()
        {
            MainTabControl.DataContext = new OrderViewModel();
        }
    }
}
