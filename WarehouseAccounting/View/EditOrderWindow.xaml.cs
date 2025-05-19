using System.Collections.ObjectModel;
using System.Windows;
using WarehouseAccounting.Model;

namespace WarehouseAccounting.View
{
    public partial class EditOrderWindow : Window
    {
        public Orders EditedOrder { get; }
        public ObservableCollection<Products> ProductsList { get; }

        public EditOrderWindow(Orders order)
        {
            InitializeComponent();
            EditedOrder = order;

            // Загружаем список товаров
            ProductsList = new ObservableCollection<Products>(ProductsDB.GetDb().SelectAll());

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}