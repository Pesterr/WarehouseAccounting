using MySqlConnector;
using System.Windows;
using WarehouseAccounting.Model;

internal class ProductsDB
{
    DbConnection connection;
    private ProductsDB(DbConnection db)
    {
        this.connection = db;
    }
    public bool Insert(Products products)
    {
        bool result = false;
        if (connection == null)
            return result;
        if (connection.OpenConnection())
        {
            var cmd = connection.CreateCommand(
                "INSERT INTO Products (product_name, category, unit, price) VALUES (@product_name, @category, @unit, @price)");
            cmd.Parameters.Add(new MySqlParameter("product_name", products.product_name));
            cmd.Parameters.Add(new MySqlParameter("category", products.category));
            cmd.Parameters.Add(new MySqlParameter("unit", products.unit));
            cmd.Parameters.Add(new MySqlParameter("price", products.price));
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.CloseConnection();
            }
        }
        return result;
    }
    internal List<Products> SelectAll()
    {
        List<Products> products = new List<Products>();
        if (connection == null || !connection.OpenConnection())
        {
            MessageBox.Show("Не удалось подключиться к базе данных.");
            return products;
        }

        try
        {
            var command = connection.CreateCommand("SELECT `product_id`, `product_name`, `category`, `unit`, `price` FROM `Products`");
            using (MySqlDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    int id = dr.GetInt32(0);
                    string name = dr.GetString("product_name");
                    string category = dr.GetString("category");
                    string unit = dr.GetString("unit");
                    decimal price = dr.GetDecimal("price");
                    products.Add(new Products
                    {
                        product_id = id,
                        product_name = name,
                        category = category,
                        unit = unit,
                        price = price
                    });
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}");
            return products;
        }
        finally
        {
            connection.CloseConnection();
        }

        if (products.Count == 0)
        {
            MessageBox.Show("В базе данных нет товаров.");
        }

        return products;
    }
    internal bool Update(Products edit)
    {
        bool result = false;
        if (connection == null)
            return result;
        if (connection.OpenConnection())
        {
            var mc = connection.CreateCommand(
                "UPDATE Products SET product_name = @product_name, category = @category, unit = @unit, price = @price WHERE product_id = @product_id");
            mc.Parameters.Add(new MySqlParameter("product_name", edit.product_name));
            mc.Parameters.Add(new MySqlParameter("category", edit.category));
            mc.Parameters.Add(new MySqlParameter("unit", edit.unit));
            mc.Parameters.Add(new MySqlParameter("price", edit.price));
            mc.Parameters.Add(new MySqlParameter("product_id", edit.product_id));
            try
            {
                mc.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.CloseConnection();
            }
        }
        return result;
    }
    internal bool Remove(Products remove)
    {
        bool result = false;
        if (connection == null)
            return result;
        if (connection.OpenConnection())
        {
            var mc = connection.CreateCommand("DELETE FROM Products WHERE product_id = @product_id");
            mc.Parameters.Add(new MySqlParameter("product_id", remove.product_id));
            try
            {
                mc.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.CloseConnection();
            }
        }
        return result;
    }
    static ProductsDB db;
    public static ProductsDB GetDb()
    {
        if (db == null)
            db = new ProductsDB(DbConnection.GetDbConnection());
        return db;
    }
}