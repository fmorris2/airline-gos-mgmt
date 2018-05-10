using AirlineDBMS.BackEnd;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private Group user_group;
        private String username;
        private String password;

        public User(int id, int user_group, string username, string password)
        {
            this.id = id;
            this.user_group = (Group)user_group;
            this.username = username;
            this.password = password;
        }

        public enum Group {Auditor, Employee, Manager}

        public static User Load(string username, string password)
        {
            // sanitize db input
            username = Regex.Replace(username, @"[\r\n\x00\x1a\\'`""]", @"\$0");
            // sanitize db input
            password = Regex.Replace(password, @"[\r\n\x00\x1a\\'`""]", @"\$0");

            User toReturn = null;

            MySqlDataReader result = DBManager.Query("SELECT * FROM `user` WHERE LOWER(`username`)='" + username.ToLower() 
                + "' AND LOWER(`password`)='" + password.ToLower() + "'");

            if (result == null) return null;

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

        public Group GetUserGroup()
        {
            return user_group;
        }
    }
}
