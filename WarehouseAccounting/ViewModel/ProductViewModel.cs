using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WarehouseAccounting.Model;

namespace WarehouseAccounting.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterProducts(); // Обновляем фильтрацию при изменении текста поиска
            }
        }
        private Products _selectedProduct;
        public Products SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }
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
        private ObservableCollection<Products> _filteredProductsList;
        public ObservableCollection<Products> FilteredProductsList
        {
            get => _filteredProductsList;
            set
            {
                _filteredProductsList = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }
        public ProductViewModel()
        {
            LoadData();

            AddCommand = new RelayCommand(AddProduct);
            DeleteCommand = new RelayCommand(DeleteProduct);
            RefreshCommand = new RelayCommand(LoadData);
        }
        private void LoadData()
        {
            var productsFromDb = ProductsDB.GetDb().SelectAll();
            ProductsList = new ObservableCollection<Products>(productsFromDb);
            FilterProducts(); // Применяем фильтр
        }
        private void AddProduct()
        {
            var newProduct = new Products
            {
                product_name = "Новый товар",
                category = "Категория",
                unit = "шт.",
                price = 0
            };

            if (ProductsDB.GetDb().Insert(newProduct))
            {
                LoadData(); // Перечитываем данные
                MessageBox.Show("Товар добавлен!");
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении товара.");
            }
        }
        private void DeleteProduct()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Выберите товар для удаления.");
                return;
            }

            if (ProductsDB.GetDb().Remove(SelectedProduct))
            {
                LoadData(); // Перечитываем данные
            }
            else
            {
                MessageBox.Show("Ошибка при удалении товара.");
            }
        }
        private void RefreshProducts()
        {
            LoadData(); // Просто перечитываем данные из БД
        }
        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProductsList = new ObservableCollection<Products>(ProductsList);
            }
            else
            {
                var filtered = ProductsList.Where(p =>
                    p.product_name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.category.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

                FilteredProductsList = new ObservableCollection<Products>(filtered);
            }
            OnPropertyChanged(nameof(FilteredProductsList));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
      
    }
}