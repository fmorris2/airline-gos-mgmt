using AirlineDBMS.BackEnd;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineDBMS.Models
{
    public class Equipment
    {
        public static List<Equipment> loadedEquipment;

        private int id;
        private string name;
        private string status;

        public Equipment(int id, string name, string status)
        {
            this.id = id;
            this.name = name;
            this.status = status;
        }

        public static void LoadEquipment()
        {
            MySqlDataReader result = DBManager.Query("SELECT * FROM `equipment`");
            loadedEquipment = new List<Equipment>();

            while(result.Read())
            {
                Equipment loaded = new Equipment(result.GetInt32("id"), result.GetString("name"), result.GetString("status"));
                loadedEquipment.Add(loaded);
            }

            result.Close();
            Console.WriteLine("Loaded " + loadedEquipment.Count + " pieces of equipment");
        }

        public int GetId()
        {
            return id;
        }

        public String GetName()
        {
            return name;
        }

        override public string ToString()
        {
            return "(" + id + ") " + name;
        }       
    }
}
