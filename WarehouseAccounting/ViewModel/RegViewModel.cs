using MySqlConnector;
using System.Windows;
using WarehouseAccounting.Model;

public class RegViewModel
{
    public Employees Employee { get; set; } = new Employees();

    public bool RegisterUser()
    {
        var db = DbConnection.GetDbConnection();
        if (!db.OpenConnection())
            return false;

        try
        {

            if (string.IsNullOrWhiteSpace(Employee.full_name) ||
                Employee.full_name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length != 3)
            {
                MessageBox.Show("ФИО должно содержать ровно 3 слова");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Employee.login) ||
                !System.Text.RegularExpressions.Regex.IsMatch(Employee.login, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Employee.password) || Employee.password.Length < 8)
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Employee.email) ||
                !System.Text.RegularExpressions.Regex.IsMatch(Employee.email, @"^[^@\s]+@[^@\s]+\.(com|ru)$"))
            {
                MessageBox.Show("Неверный формат почты. Пример: example@mail.com или example@mail.ru");
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(Employee.phone, @"^\+?[78 ]\d{10}$"))
            {
                MessageBox.Show("Неверный формат номера телефона");
                return false;
            }
            using (var checkCmd = db.CreateCommand("SELECT COUNT(*) FROM Employees WHERE login=@login"))
            {
                checkCmd.Parameters.AddWithValue("@login", Employee.login);
                var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return false;
                }

            }
            using (var cmd = db.CreateCommand(
                @"INSERT INTO Employees (full_name, position, phone, email, login, password) 
              VALUES (@full_name, @position, @phone, @email, @login, @password)"))
            {
                cmd.Parameters.AddWithValue("@full_name", Employee.full_name);
                cmd.Parameters.AddWithValue("@position", Employee.position);
                cmd.Parameters.AddWithValue("@phone", Employee.phone);
                cmd.Parameters.AddWithValue("@email", Employee.email);
                cmd.Parameters.AddWithValue("@login", Employee.login);
                cmd.Parameters.AddWithValue("@password", Employee.password);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        catch (MySqlException ex)
        {
            MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
            return false;
        }
        finally
        {
            db.CloseConnection();
        }
    }
}