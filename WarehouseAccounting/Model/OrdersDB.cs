using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseAccounting.Model
{
    internal class OrdersDB
    {
        DbConnection connection;

        private OrdersDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Orders orders)
        {
            bool result = false;
            if (connection == null)

                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Orders` Values (0, @id, @client_name, @employee, @date, @status);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("id", orders.order_id));
                cmd.Parameters.Add(new MySqlParameter("client_name", orders.client_name));
                cmd.Parameters.Add(new MySqlParameter("employee", orders.employee));
                cmd.Parameters.Add(new MySqlParameter("date", orders.date));
                cmd.Parameters.Add(new MySqlParameter("status", orders.status));
            }
            return result = true;

        }

        internal List<Orders> SelectAll()
        {
            List<Orders> orders = new List<Orders>();
            if (connection == null)
                return orders;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `client_name`, `employee`, `date`, `status` from `Orders` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string client_name = dr.GetString("client_name");
                        string employee = dr.GetString("employee");
                        DateTime date = dr.GetDateTime("date");
                        string status = dr.GetString("status");

                        orders.Add(new Orders
                        {
                            order_id = id,
                            client_name = client_name,
                            employee = employee,
                            date = date,
                            status = status
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return orders;
        }

        internal bool Update(Orders edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Orders` set `client_name`=@client_name, `employee`=@employee, `date`=@date, `status`=@status where `id` = {edit.order_id}");
                mc.Parameters.Add(new MySqlParameter("client_name", edit.client_name));
                mc.Parameters.Add(new MySqlParameter("employee", edit.employee));
                mc.Parameters.Add(new MySqlParameter("date", edit.date));
                mc.Parameters.Add(new MySqlParameter("status", edit.status));

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


        internal bool Remove(Orders remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Orders` where `id` = {remove.order_id}");
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

        static OrdersDB db;
        public static OrdersDB GetDb()
        {
            if (db == null)
                db = new OrdersDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
