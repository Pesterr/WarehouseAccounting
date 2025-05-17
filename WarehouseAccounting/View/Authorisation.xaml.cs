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
    /// Логика взаимодействия для Authorisation.xaml
    /// </summary>
    public partial class Authorisation : Window
    {
        private AuthViewModel _viewModel;
        public Authorisation()
        {
            InitializeComponent();
            _viewModel = new AuthViewModel();
            DataContext = _viewModel;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем пароль из PasswordBox
            _viewModel.Employee.password = LogPassword.Password;

            if (_viewModel.AuthenticateUser())
            {
                MessageBox.Show($"Добро пожаловать, {_viewModel.Employee.full_name}!");
                Main mains = new Main();
                mains.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        private void RegistrationClick(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }
    }
}
