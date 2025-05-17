using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseAccounting.Model
{
    internal class WarehouseDB
    {
        DbConnection connection;

        private WarehouseDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Warehouse warehouse)
        {
            bool result = false;
            if (connection == null)

                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Warehouse` Values (0, @id, @location, @capacity);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("id", warehouse.id));
                cmd.Parameters.Add(new MySqlParameter("location", warehouse.location));
                cmd.Parameters.Add(new MySqlParameter("capacity", warehouse.capacity));
            }
            return result = true;

        }

        internal List<Warehouse> SelectAll()
        {
            List<Warehouse> warehouse = new List<Warehouse>();
            if (connection == null)
                return warehouse;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `location`, `capacity` from `Warehouse` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string location = dr.GetString("location");
                        int capacity = dr.GetInt32("capacity");

                        warehouse.Add(new Warehouse
                        {
                            id = id,
                            location = location,
                            capacity = capacity,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return warehouse;
        }

        internal bool Update(Warehouse edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Warehouse` set `location`=@location, `capacity`=@capacity where `id` = {edit.id}");
                mc.Parameters.Add(new MySqlParameter("location", edit.location));
                mc.Parameters.Add(new MySqlParameter("capacity", edit.capacity));
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


        internal bool Remove(Warehouse remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Warehouse` where `id` = {remove.id}");
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

        static WarehouseDB db;
        public static WarehouseDB GetDb()
        {
            if (db == null)
                db = new WarehouseDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
