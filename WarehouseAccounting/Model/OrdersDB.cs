using MySqlConnector;
using System.Windows;
using WarehouseAccounting.Model;

internal class OrdersDB
{
    DbConnection connection;
    private OrdersDB(DbConnection db)
    {
        this.connection = db;
    }

    public bool Insert(Orders orders)
    {
        if (connection == null || !connection.OpenConnection())
            return false;

        try
        {
            var cmd = connection.CreateCommand(
                "INSERT INTO Orders (client_name, date, order_product, quantity) VALUES (@client_name, @date, @order_product, @quantity); SELECT LAST_INSERT_ID();");
            cmd.Parameters.Add(new MySqlParameter("client_name", orders.client_name));
            cmd.Parameters.Add(new MySqlParameter("date", orders.date));
            cmd.Parameters.Add(new MySqlParameter("order_product", orders.order_product));
            cmd.Parameters.Add(new MySqlParameter("quantity", orders.quantity));

            object lastId = cmd.ExecuteScalar();
            if (lastId != null)
            {
                orders.order_id = Convert.ToInt32(lastId);
                return true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            connection.CloseConnection();
        }

        return false;
    }

    internal List<Orders> SelectAll()
    {
        List<Orders> orders = new List<Orders>();
        if (connection == null || !connection.OpenConnection())
            return orders;

        try
        {
            var command = connection.CreateCommand("SELECT `id`, `client_name`, `date`, `order_product`, `quantity` FROM `Orders` ");
            using (MySqlDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string client_name = dr.GetString("client_name");
                    string order_product = dr.GetString("order_product");
                    DateTime date = dr.GetDateTime("date");
                    int quantity = dr.GetInt32("quantity");

                    orders.Add(new Orders
                    {
                        order_id = id,
                        client_name = client_name,
                        order_product = order_product,
                        date = date,
                        quantity = quantity
                    });
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            connection.CloseConnection();
        }

        return orders;
    }

    internal bool Update(Orders edit)
    {
        if (connection == null || !connection.OpenConnection())
            return false;
        try
        {
            var mc = connection.CreateCommand(
                "UPDATE Orders SET client_name = @client_name, date = @date, order_product = @order_product, quantity = @quantity WHERE id = @id");
            mc.Parameters.Add(new MySqlParameter("client_name", edit.client_name));
            mc.Parameters.Add(new MySqlParameter("date", edit.date));
            mc.Parameters.Add(new MySqlParameter("order_product", edit.order_product));
            mc.Parameters.Add(new MySqlParameter("quantity", edit.quantity));
            mc.Parameters.Add(new MySqlParameter("id", edit.order_id)); // Добавляем параметр @id
            mc.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            connection.CloseConnection();
        }
        return false;
    }

    internal bool Remove(Orders remove)
    {
        if (connection == null || !connection.OpenConnection())
            return false;

        try
        {
            var mc = connection.CreateCommand($"DELETE FROM Orders WHERE id = {remove.order_id}");
            mc.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            connection.CloseConnection();
        }

        return false;
    }

    static OrdersDB db;
    public static OrdersDB GetDb()
    {
        if (db == null)
            db = new OrdersDB(DbConnection.GetDbConnection());
        return db;
    }
}