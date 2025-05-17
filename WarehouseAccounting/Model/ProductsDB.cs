using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseAccounting.Model
{
    internal class ProductsDB
    {
        DbConnection connection;

        private ProductsDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Products product)
        {
            if (connection == null || !connection.OpenConnection())
                return false;

            try
            {
                // Проверяем, существует ли уже товар с таким именем
                var checkCmd = connection.CreateCommand("SELECT COUNT(*) FROM `Products` WHERE `product_name` = @name");
                checkCmd.Parameters.AddWithValue("@name", product.product_name);

                int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (existingCount > 0)
                {
                    MessageBox.Show("Товар с таким названием уже существует.");
                    return false;
                }

                // Если товара нет — добавляем
                string query = @"
            INSERT INTO `Products` (product_name, category, unit, price)
            VALUES (@product_name, @category, @unit, @price);
            SELECT LAST_INSERT_ID();";

                using (var cmd = connection.CreateCommand(query))
                {
                    cmd.Parameters.AddWithValue("@product_name", product.product_name);
                    cmd.Parameters.AddWithValue("@category", product.category);
                    cmd.Parameters.AddWithValue("@unit", product.unit);
                    cmd.Parameters.AddWithValue("@price", product.price);

                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedId))
                    {
                        product.product_id = insertedId;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}");
            }
            finally
            {
                connection.CloseConnection();
            }

            return false;
        }
        public bool ProductExists(string productName, int excludeId = -1)
        {
            if (connection == null || !connection.OpenConnection())
                return false;

            try
            {
                string query = "SELECT COUNT(*) FROM `Products` WHERE `product_name` = @name";
                if (excludeId != -1)
                {
                    query += " AND `product_id` != @excludeId";
                }

                using (var cmd = connection.CreateCommand(query))
                {
                    cmd.Parameters.AddWithValue("@name", productName);
                    if (excludeId != -1)
                    {
                        cmd.Parameters.AddWithValue("@excludeId", excludeId);
                    }

                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке существования товара: {ex.Message}");
                return false;
            }
        }
        internal List<Products> SelectAll()
        {
            List<Products> products = new List<Products>();
            if (connection == null)
                return products;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT `product_id`, `product_name`, `category`, `unit`, `price` FROM `Products` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return products;
        }

        internal bool Update(Products edit)
        {
            bool result = false;

            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                // Проверяем, есть ли уже товар с таким именем (кроме текущего)
                if (ProductExists(edit.product_name, edit.product_id))
                {
                    MessageBox.Show("Товар с таким названием уже существует.");
                    return false;
                }

                var mc = connection.CreateCommand(
                    "UPDATE `Products` SET `product_name`=@product_name, `category`=@category, `unit`=@unit, `price`=@price WHERE `product_id` = @id");

                mc.Parameters.Add(new MySqlParameter("product_name", edit.product_name));
                mc.Parameters.Add(new MySqlParameter("category", edit.category));
                mc.Parameters.Add(new MySqlParameter("unit", edit.unit));
                mc.Parameters.Add(new MySqlParameter("price", edit.price));
                mc.Parameters.Add(new MySqlParameter("id", edit.product_id));

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


        internal bool Remove(Products remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Products` where `product_id` = {remove.product_id}");
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

        static ProductsDB db;
        public static ProductsDB GetDb()
        {
            if (db == null)
                db = new ProductsDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
