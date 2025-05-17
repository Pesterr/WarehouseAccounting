using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace WarehouseAccounting.Model
{
    class SuppliersDB
    {
        DbConnection connection;

        private SuppliersDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Suppliers suppliers)
        {
            bool result = false;
            if (connection == null)

                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Suppliers` Values (0, @id, @name, @contact_person, @phone, @address);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("id", suppliers.id));
                cmd.Parameters.Add(new MySqlParameter("shipment", suppliers.name));
                cmd.Parameters.Add(new MySqlParameter("product", suppliers.contact_person));
                cmd.Parameters.Add(new MySqlParameter("quantity", suppliers.phone));
                cmd.Parameters.Add(new MySqlParameter("price_per_unit", suppliers.address));
            }
            return result = true;

        }

        internal List<Suppliers> SelectAll()
        {
            List<Suppliers> suppliers = new List<Suppliers>();
            if (connection == null)
                return suppliers;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `name`, `contact_person`, `phone`, `address` from `Suppliers` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string name = dr.GetString("name");
                        string contact_person = dr.GetString("contact_person");
                        string phone = dr.GetString("phone");
                        string address = dr.GetString("address");

                        suppliers.Add(new Suppliers
                        {
                            id = id,
                            name = name,
                            contact_person = contact_person,
                            phone = phone,
                            address = address
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return suppliers;
        }

        internal bool Update(Suppliers edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Suppliers` set `name`=@name, `contact_person`=@contact_person, `phone`=@phone, `address`=@address where `id` = {edit.id}");
                mc.Parameters.Add(new MySqlParameter("name", edit.name));
                mc.Parameters.Add(new MySqlParameter("contact_person", edit.contact_person));
                mc.Parameters.Add(new MySqlParameter("phone", edit.phone));
                mc.Parameters.Add(new MySqlParameter("address", edit.address));

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


        internal bool Remove(Suppliers remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Suppliers` where `id` = {remove.id}");
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

        static SuppliersDB db;
        public static SuppliersDB GetDb()
        {
            if (db == null)
                db = new SuppliersDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
