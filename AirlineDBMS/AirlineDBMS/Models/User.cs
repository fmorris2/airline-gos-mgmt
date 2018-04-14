using AirlineDBMS.BackEnd;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineDBMS.Models
{
    /*
     * This class will serve as the model for the User information.
     * It will also contain a static Load(username, pass) method which
     * can be used to create and return a relevant User object for a
     * designated username and password.
     * 
     * @author Fred
     */
    class User
    {
        public static User instance = null;

        private int id;
        private int user_group;
        private String username;
        private String password;

        public User(int id, int user_group, string username, string password)
        {
            this.id = id;
            this.user_group = user_group;
            this.username = username;
            this.password = password;
        }

        public static User Load(string username, string password)
        {
            User toReturn = null;

            MySqlDataReader result = DBManager.query("SELECT * FROM `user` WHERE LOWER(`username`)='" + username.ToLower() 
                + "' AND LOWER(`password`)='" + password.ToLower() + "'");
            if (result.Read())
            {
                toReturn = new User(result.GetInt32("id"), result.GetInt16("user_group"), result.GetString("username"), result.GetString("password"));
            }

            result.Close();
            return toReturn;
        }

        public static bool LoadInstance(string username, string password)
        {
            instance = Load(username, password);
            return instance != null;
        }
    }
}
