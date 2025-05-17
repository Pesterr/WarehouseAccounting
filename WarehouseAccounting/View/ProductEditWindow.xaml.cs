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
using WarehouseAccounting.Model;

namespace WarehouseAccounting.View
{
    public partial class ProductEditWindow : Window
    {
        public Products CurrentProduct { get; set; }

        public ProductEditWindow(Products product)
        {
            InitializeComponent();
            CurrentProduct = product;
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentProduct.product_name))
            {
                MessageBox.Show("Название товара не может быть пустым.");
                return;
            }

            if (ProductsDB.GetDb().Update(CurrentProduct))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении изменений.");
            }
        }
    }
}
