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
        private void ProductAdd(object sender, RoutedEventArgs e)
        {
            var productsAdd = new ProductsAdd();
            productsAdd.Show();

        }
        private void OrderAdd(object sender, RoutedEventArgs e)
        {
            var orderAdd = new OrdersAdd();
            orderAdd.Show();
        }
    }
}
