using MySqlConnector;
using System.Data;
using System.Windows;
using WarehouseAccounting.Model;

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
                        Employee.id = Convert.ToInt32(reader["id"]);
                        Employee.full_name = reader.IsDBNull("full_name") ? string.Empty : reader.GetString("full_name");
                        Employee.position = reader.IsDBNull("position") ? string.Empty : reader.GetString("position");
                        Employee.phone = reader.IsDBNull("phone") ? string.Empty : reader.GetString("phone");
                        Employee.email = reader.IsDBNull("email") ? string.Empty : reader.GetString("email");
                        Employee.login = reader.IsDBNull("login") ? string.Empty : reader.GetString("login");
                        Employee.password = reader.IsDBNull("password") ? string.Empty : reader.GetString("password");

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