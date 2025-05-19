using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WarehouseAccounting.Model;

namespace WarehouseAccounting.ViewModel
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private Employees _currentUser;
        public Employees CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        public ProfileViewModel(Employees user)
        {
            CurrentUser = user ?? new Employees(); // Подстраховка
            SaveCommand = new RelayCommand(SaveProfile);
        }

        private void SaveProfile()
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.full_name) ||
                string.IsNullOrWhiteSpace(CurrentUser.position) ||
                string.IsNullOrWhiteSpace(CurrentUser.phone) ||
                string.IsNullOrWhiteSpace(CurrentUser.email))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
                return;
            }

            // Проверка номера телефона
            if (!System.Text.RegularExpressions.Regex.IsMatch(CurrentUser.phone, @"^\+?[78]\d{10}$"))
            {
                MessageBox.Show("Неверный формат номера телефона");
                return;
            }

            // Проверка email
            if (!System.Text.RegularExpressions.Regex.IsMatch(CurrentUser.email, @"^[^@\s]+@[^@\s]+\.(com|ru)$"))
            {
                MessageBox.Show("Неверный формат email");
                return;
            }

            // Если указан старый пароль — проверяем его
            if (!string.IsNullOrWhiteSpace(OldPassword) ||
                !string.IsNullOrWhiteSpace(NewPassword) ||
                !string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                var db = EmployeesDB.GetDb();
                var storedUser = db.SelectAll().FirstOrDefault(u => u.id == CurrentUser.id);

                if (storedUser?.password != OldPassword)
                {
                    MessageBox.Show("Старый пароль неверен");
                    return;
                }

                if (NewPassword != ConfirmPassword)
                {
                    MessageBox.Show("Новые пароли не совпадают");
                    return;
                }

                if (NewPassword.Length < 8)
                {
                    MessageBox.Show("Пароль должен быть не менее 8 символов");
                    return;
                }

                CurrentUser.password = NewPassword;
            }

            var employeesDB = EmployeesDB.GetDb();
            if (employeesDB.Update(CurrentUser))
            {
                MessageBox.Show("Данные успешно обновлены");
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}