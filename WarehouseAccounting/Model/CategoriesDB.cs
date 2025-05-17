using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WarehouseAccounting.Model;

namespace WarehouseAccounting.Model
{
    internal class CategoriesDB
    {
        DbConnection connection;

        private CategoriesDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Categories categories)
        {
            bool result = false;
            if (connection == null)

                return result;
            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Categories` Values (0, @id, @name, @description);select LAST_INSERT_ID();");


                cmd.Parameters.Add(new MySqlParameter("id", categories.id));
                cmd.Parameters.Add(new MySqlParameter("name", categories.name));
                cmd.Parameters.Add(new MySqlParameter("description", categories.description));
            }
            return result=true;
            
        }

        internal List<Categories> SelectAll()
        {
            List<Categories> categories = new List<Categories>();
            if (connection == null)
                return categories;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `name`, `description` from `Categories` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string name = dr.GetString("name");
                        string description = dr.GetString("description");
                       
                        categories.Add(new Categories
                        {
                            id = id,
                            name = name,
                            description = description,
                            
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return categories;
        }

        internal bool Update(Categories edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Categories` set `name`=@name, `description`=@description where `id` = {edit.id}");
                mc.Parameters.Add(new MySqlParameter("name", edit.name));
                mc.Parameters.Add(new MySqlParameter("description", edit.description));
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


        internal bool Remove(Categories remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Categories` where `id` = {remove.id}");
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

        static CategoriesDB db;
        public static CategoriesDB GetDb()
        {
            if (db == null)
                db = new CategoriesDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
