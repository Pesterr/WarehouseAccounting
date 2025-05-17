using MySqlConnector;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseAccounting.Model;
using WarehouseAccounting.ViewModel;


namespace WarehouseAccounting.View
{
    public partial class Registration : Window
    {
        private RegViewModel _viewModel;
        public Registration()
        {
            InitializeComponent();
            if (!DbConnection.GetDbConnection().TestConnection())
            {
                MessageBox.Show("Нет подключения к базе данных!");
                Close();
                return;
            }
            _viewModel = new RegViewModel();
            DataContext = _viewModel;
        }
        private void AuthorisationClick(object sender, RoutedEventArgs e)
        {
            var authWindow = new Authorisation();
            authWindow.Show();
            this.Close();
        }

        
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Employee.password = RegPassword.Password;

            try
            {
                bool success = await Task.Run(() => _viewModel.RegisterUser());

                if (success)
                {
                    MessageBox.Show("Регистрация прошла успешно!");
                    var authWindow = new Authorisation();
                    authWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var authWindow = new Authorisation();
            authWindow.Show();
            this.Close();
        }
    }
}