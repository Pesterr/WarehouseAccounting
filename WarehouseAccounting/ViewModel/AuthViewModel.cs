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
    public class AuthViewModel
    {
        public Employees Employee { get; set; } = new Employees();

        public bool AuthenticateUser()
        {
            var db = DbConnection.GetDbConnection();
            if (!db.OpenConnection())
                return false;

            try
            {
                using (var cmd = db.CreateCommand("SELECT * FROM Employees WHERE login=@login AND password=@password"))
                {
                    cmd.Parameters.AddWithValue("@login", Employee.login);
                    cmd.Parameters.AddWithValue("@password", Employee.password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Employee.full_name = reader["full_name"].ToString();
                            Employee.position = reader["position"].ToString();
                            Employee.phone = reader["phone"].ToString();
                            Employee.email = reader["email"].ToString();
                            return true;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}");
            }
            finally
            {
                db.CloseConnection();
            }
            return false;
        }
    }
}