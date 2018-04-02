using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineDBMS.BackEnd
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
        public static User Load(String username, String password)
        {
            System.Console.WriteLine("Load("+username+","+password+")");
            System.Console.WriteLine("DB Ping: " + DBManager.Ping());
            return null;
        }
    }
}
