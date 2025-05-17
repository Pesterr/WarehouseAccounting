using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseAccounting.Model
{
    internal class IncomingShipmentsDB
    {
        DbConnection connection;

        private IncomingShipmentsDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(IncomingShipments incomingShipments)
        {
            bool result = false;
            if (connection == null)

                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `IncomingShipments` Values (0, @id, @supplier, @category, @shipment_date);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("id", incomingShipments.id));
                cmd.Parameters.Add(new MySqlParameter("supplier", incomingShipments.supplier));
                cmd.Parameters.Add(new MySqlParameter("category", incomingShipments.category));
                cmd.Parameters.Add(new MySqlParameter("shipment_date", incomingShipments.shipment_date));
            }
            return result = true;

        }

        internal List<IncomingShipments> SelectAll()
        {
            List<IncomingShipments> incomingShipments = new List<IncomingShipments>();
            if (connection == null)
                return incomingShipments;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `supplier`, `category`, `shipment_date` from `IncomingShipments` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string supplier = dr.GetString("supplier");
                        string category = dr.GetString("category");
                        DateTime shipmment_date = dr.GetDateTime("shipment_date");

                        incomingShipments.Add(new IncomingShipments
                        {
                            id = id,
                            supplier = supplier,
                            category = category,
                            shipment_date = shipmment_date
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return incomingShipments;
        }

        internal bool Update(IncomingShipments edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `IncomingShipments` set `supplier`=@supplier, `category`=@category, `shipment_date`=@shipment_date where `id` = {edit.id}");
                mc.Parameters.Add(new MySqlParameter("supplier", edit.supplier));
                mc.Parameters.Add(new MySqlParameter("category", edit.category));
                mc.Parameters.Add(new MySqlParameter("shipment_date", edit.shipment_date));

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


        internal bool Remove(IncomingShipments remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `IncomingShipments` where `id` = {remove.id}");
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

        static IncomingShipmentsDB db;
        public static IncomingShipmentsDB GetDb()
        {
            if (db == null)
                db = new IncomingShipmentsDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
