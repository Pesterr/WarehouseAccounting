using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseAccounting.Model
{
    internal class ShipmentItemsDB
    {
        DbConnection connection;

        private ShipmentItemsDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(ShipmentItems shipmentItems)
        {
            bool result = false;
            if (connection == null)

                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `ShipmentItems` Values (0, @id, @shipment, @product, @quantity, @price_per_unit);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("id", shipmentItems.id));
                cmd.Parameters.Add(new MySqlParameter("shipment", shipmentItems.shipment));
                cmd.Parameters.Add(new MySqlParameter("product", shipmentItems.product));
                cmd.Parameters.Add(new MySqlParameter("quantity", shipmentItems.quantity));
                cmd.Parameters.Add(new MySqlParameter("price_per_unit", shipmentItems.price_per_unit));
            }
            return result = true;

        }

        internal List<ShipmentItems> SelectAll()
        {
            List<ShipmentItems> shipmentItems = new List<ShipmentItems>();
            if (connection == null)
                return shipmentItems;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `shipment`, `product`, `quantity`, `price_per_unit` from `ShipmentItems` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int shipment = dr.GetInt32("shipment");
                        string product = dr.GetString("product");
                        int quantity = dr.GetInt32("quantity");
                        decimal price_per_unit = dr.GetInt32("price_per_unit");

                        shipmentItems.Add(new ShipmentItems
                        {
                            id = id,
                            shipment = shipment,
                            product = product,
                            quantity = quantity,
                            price_per_unit = price_per_unit
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return shipmentItems;
        }

        internal bool Update(ShipmentItems edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `ShipmentItems` set `shipment`=@shipment, `product`=@product, `quantity`=@quantity, `price_per_unit`=@price_per_unit where `id` = {edit.id}");
                mc.Parameters.Add(new MySqlParameter("shipment", edit.shipment));
                mc.Parameters.Add(new MySqlParameter("product", edit.product));
                mc.Parameters.Add(new MySqlParameter("quantity", edit.quantity));
                mc.Parameters.Add(new MySqlParameter("price_per_unit", edit.price_per_unit));

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


        internal bool Remove(ShipmentItems remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `ShipmentItems` where `id` = {remove.id}");
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

        static ShipmentItemsDB db;
        public static ShipmentItemsDB GetDb()
        {
            if (db == null)
                db = new ShipmentItemsDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
