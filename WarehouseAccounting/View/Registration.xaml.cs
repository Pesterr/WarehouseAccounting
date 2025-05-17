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


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем заполненность полей
            if (!AreAllFieldsFilled())
                return;

            // Получаем данные из формы
            var viewModel = new RegViewModel();
            var employee = viewModel.Employee;

            // Присваиваем значения из TextBox и PasswordBox
            employee.full_name = RegName.Text;
            employee.position = RegPosition.Text;
            employee.login = RegLogin.Text;
            employee.email = RegEmail.Text;
            employee.phone = RegPhone.Text;
            employee.password = RegPassword.Password;

            // Регистрация пользователя
            if (viewModel.RegisterUser())
            {
                MessageBox.Show("Регистрация успешна!");
                var authWindow = new Authorisation();
                authWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка регистрации. Попробуйте снова.");
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var authWindow = new Authorisation();
            authWindow.Show();
            this.Close();
        }
        private bool AreAllFieldsFilled()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(RegName.Text))
            {
                RegName.BorderBrush = Brushes.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(RegPosition.Text))
            {
                RegPosition.BorderBrush = Brushes.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(RegLogin.Text))
            {
                RegLogin.BorderBrush = Brushes.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(RegEmail.Text))
            {
                RegEmail.BorderBrush = Brushes.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(RegPhone.Text))
            {
                RegPhone.BorderBrush = Brushes.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(RegPassword.Password))
            {
                RegPassword.BorderBrush = Brushes.Red;
                isValid = false;
            }
            if (!isValid)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return isValid;
        }
    }
}