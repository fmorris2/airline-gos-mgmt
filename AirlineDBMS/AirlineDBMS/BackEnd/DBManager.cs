﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineDBMS.BackEnd
{

    /*
     * TODO:
     *   The goal of this class is to offer a single
     *   method of retrieving / updating data in the remote SQL
     *   database. We need to ensure one connection is used. The user
     *   will only be manipulating data for one purpose at a time, so only
     *   one connection is needed.
     *   
     *   We can go with the Singleton design pattern for this purpose, or simply
     *   go with a static-based approach. Not sure what I want to do yet.
     *   
     *   Still looking into the optimal method for connecting to an SQL DB, so this class
     *   is completely barren other than credentials at the moment. Also, for our class it won't
     *   matter, but in a production environment obviously we'd never handle authentication
     *   client-sided like this.
     *   
     *   @author Fred
     */
    static class DBManager
    {
        const string HOST = "vikingsoftware.org";
        const string USER = "rwallace";
        const string PASS = "oswego";
        const string DB_NAME = "airline_management";
        const string CONN_STRING = "Database="+DB_NAME+";Data Source="+HOST+";User Id="+USER+";Password="+PASS;

        private static MySqlConnection connection;

        /**
         * This is the primary method we'll use to execute queries on the DB.
         * It simply returns a MySqlDataReader object with the results of the
         * query
         */
        public static MySqlDataReader Query(String sql)
        {
            if(connection == null)
            {
                connection = new MySqlConnection(CONN_STRING);
                connection.Open();
            }

            Console.WriteLine("DBManager#query("+sql+")");
            MySqlCommand command = new MySqlCommand(sql, connection);
            return command.ExecuteReader();
        }
    }
}
