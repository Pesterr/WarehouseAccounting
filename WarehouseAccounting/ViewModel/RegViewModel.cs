using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarehouseAccounting.Model;

namespace WarehouseAccounting.ViewModel
{
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
                // Проверка существования логина
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

                // Регистрация нового пользователя
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
}