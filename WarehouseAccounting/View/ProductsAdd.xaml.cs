using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WarehouseAccounting.Model;
using WarehouseAccounting.ViewModel;

namespace WarehouseAccounting.View
{
    public partial class ProductsAdd : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Products CurrentProduct { get; set; }
        public ICommand AddCommand { get; }

        public ProductsAdd()
        {
            InitializeComponent();

            CurrentProduct = new Products();
            AddCommand = new RelayCommand(AddProduct);
            DataContext = this;
        }
        private void ClearFields()
        {
            CurrentProduct.product_name = string.Empty;
            CurrentProduct.category = string.Empty;
            CurrentProduct.unit = string.Empty;
            CurrentProduct.price = 0;

            // Уведомляем интерфейс об изменениях
            OnPropertyChanged(nameof(CurrentProduct));
        }

        private void AddProduct()
        {
            var db = ProductsDB.GetDb();
            if (db.Insert(CurrentProduct))
            {
                MessageBox.Show("Товар успешно добавлен!");
                ClearFields();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении товара.");
            }
        }
    }
}