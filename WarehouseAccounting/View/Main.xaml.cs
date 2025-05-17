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
        public Main(Employees currentUser)
        {
            InitializeComponent();

            ProfileViewModel = new ProfileViewModel(currentUser);
            DataContext = this;
            // Создаём ProductViewModel
            ProductViewModel = new ProductViewModel();

            // Устанавливаем DataContext для вкладки "Товары"
            ProductsTabItem.DataContext = ProductViewModel;
        }

        private void ProductAdd(object sender, RoutedEventArgs e)
        {
            var productsAdd = new ProductsAdd();
            productsAdd.Show();

        }
        private void OrderAdd(object sender, RoutedEventArgs e)
        {
            
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProducts.SelectedItem is Products selectedProduct)
            {
                var editWindow = new ProductEditWindow(selectedProduct);
                if (editWindow.ShowDialog() == true)
                {
                    ((ProductViewModel)DataContext).RefreshCommand.Execute(null);
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
    }
}
