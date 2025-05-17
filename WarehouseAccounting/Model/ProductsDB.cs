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

        public bool Insert(Products products)
        {
            if (connection == null || !connection.OpenConnection())
                return false;

            try
            {
                string query = @"
            INSERT INTO `Products` (product_name, category, unit, price)
            VALUES (@product_name, @category, @unit, @price);
            SELECT LAST_INSERT_ID();";

                using (var cmd = connection.CreateCommand(query))
                {
                    cmd.Parameters.AddWithValue("@product_name", products.product_name);
                    cmd.Parameters.AddWithValue("@category", products.category);
                    cmd.Parameters.AddWithValue("@unit", products.unit);
                    cmd.Parameters.AddWithValue("@price", products.price);

                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedId))
                    {
                        products.product_id = insertedId; // Обновляем ID
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
                var mc = connection.CreateCommand($"update `Products` set `product_name`=@product_name, `category`=@category, `unit`=@unit, `price`=@price where `product_id` = {edit.product_id}");
                mc.Parameters.Add(new MySqlParameter("product_name", edit.product_name));
                mc.Parameters.Add(new MySqlParameter("category", edit.category));
                mc.Parameters.Add(new MySqlParameter("unit", edit.unit));
                mc.Parameters.Add(new MySqlParameter("price", edit.price));

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
