using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseAccounting.Model
{
    internal class EmployeesDB
    {
        DbConnection connection;

        private EmployeesDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Employees employees)
        {
            bool result = false;
            if (connection == null)

                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Employees` Values (0, @id, @full_name, @position, @phone, @email, @login, @password);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("id", employees.id));
                cmd.Parameters.Add(new MySqlParameter("name", employees.full_name));
                cmd.Parameters.Add(new MySqlParameter("id", employees.position));
                cmd.Parameters.Add(new MySqlParameter("id", employees.phone));
                cmd.Parameters.Add(new MySqlParameter("id", employees.email));
                cmd.Parameters.Add(new MySqlParameter("id", employees.login));
                cmd.Parameters.Add(new MySqlParameter("id", employees.password));
            }
            return result = true;

        }

        internal List<Employees> SelectAll()
        {
            List<Employees> employees = new List<Employees>();
            if (connection == null)
                return employees;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `full_name`, `position`, `phone`, `email`, `login`, `password` from `Employees` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string full_name = dr.GetString("full_name");
                        string position = dr.GetString("position");
                        string phone = dr.GetString("phone");
                        string email = dr.GetString("email");
                        string login = dr.GetString("login");
                        string password = dr.GetString("password");

                        employees.Add(new Employees
                        {
                            id = id,
                            full_name = full_name,
                            position = position,
                            phone = phone,
                            email = email,
                            login = login,
                            password = password

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return employees;
        }

        internal bool Update(Employees edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Employees` set `full_name`=@full_name, `position`=@position, `phone=@phone`, `email=@email`, `login=@login`, `password=@password`, where `id` = {edit.id}");
                mc.Parameters.Add(new MySqlParameter("full_name", edit.full_name));
                mc.Parameters.Add(new MySqlParameter("position", edit.position));
                mc.Parameters.Add(new MySqlParameter("phone", edit.phone));
                mc.Parameters.Add(new MySqlParameter("email", edit.email));
                mc.Parameters.Add(new MySqlParameter("login", edit.login));
                mc.Parameters.Add(new MySqlParameter("password", edit.password));
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Employees remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Employees` where `id` = {remove.id}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static EmployeesDB db;
        public static EmployeesDB GetDb()
        {
            if (db == null)
                db = new EmployeesDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
