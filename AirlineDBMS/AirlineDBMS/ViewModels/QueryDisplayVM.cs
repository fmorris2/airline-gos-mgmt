using AirlineDBMS.BackEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirlineDBMS.ViewModels
{
    class QueryDisplayVM : INotifyPropertyChanged
    {
        private static object lockObj = new object();
        private static volatile QueryDisplayVM instance;

        #region Constructor/Instance
        public static QueryDisplayVM Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new QueryDisplayVM();
                    }
                }
                return instance;
            }
        }

        public QueryDisplayVM()
        {
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        #endregion

        #region Properties

        private DataView queryDisplayItemsSource;

        public DataView QueryDisplayItemsSource
        {
            get
            {
                return queryDisplayItemsSource;
            }
            set
            {
                if (queryDisplayItemsSource != value)
                {
                    // clear the textbox at the bottom if changing queries
                    if (value == null)
                        QueryTextBox = "";

                    queryDisplayItemsSource = value;
                    NotifyPropertyChanged("QueryDisplayItemsSource");
                }
            }
        }

        private Visibility queryDisplayVisible = Visibility.Collapsed;
        public Visibility QueryDisplayVisible
        {
            get
            {
                return queryDisplayVisible;
            }
            set
            {
                if (queryDisplayVisible != value)
                {
                    // toggle highlight of menu item based on value
                    queryDisplayVisible = value;
                    NotifyPropertyChanged("QueryDisplayVisible");
                }
            }
        }

        // How many results to limit the table displays too
        private int limit = 100;
        public int LIMIT
        {
            get { return limit; }
            set
            {
                if (limit != value)
                {
                    limit = value;
                    NotifyPropertyChanged("LIMIT");
                }
            }
        }

        // Fill up the query list combo box
        private ObservableCollection<string> queryListSource = new ObservableCollection<string>
        {
            "Select a Query", "Unoperational Eqp.", "Large Fuel Orders", "June Employees", "Bags in Transit",
            
        };
        public ObservableCollection<string> QueryListSource
        {
            get
            {
                return queryListSource;
            }
        }

        private string selectedQuery;
        public string SelectedQuery
        {
            get { return selectedQuery; }
            set
            {
                if (selectedQuery != value)
                {
                    selectedQuery = value;
                }
                
                switch (selectedQuery)
                {
                    case "Unoperational Eqp.":
                        Instance.UnoperationalEqp();
                        break;
                    case "June Employees":
                        Instance.JuneEmployees();
                        break;
                    case "Large Fuel Orders":
                        Instance.LargeFuelOrders();
                        break;
                    case "Bags in Transit":
                        Instance.BagsInTransit();
                        break;
                    default:
                        break;
                }
            }
        }

        private string queryTextBox = "";
        public string QueryTextBox
        {
            get { return queryTextBox; }
            set
            {
                if (queryTextBox != value)
                {
                    queryTextBox = value;
                    NotifyPropertyChanged("QueryTextBox");
                }
            }
        }
        #endregion

        #region QueryMethods
        public void UnoperationalEqp()
        {
            // Get query from DB
            string query = "SELECT `equipment`.`name`, `work_order`.`issue_desc` \n";
            query += "FROM `equipment` INNER JOIN `work_order` ON `work_order`.`equipment_id` = `equipment`.`id` \n";
            query += "WHERE `equipment`.`status` != 'operational' ";

            DataView result = DBManager.GetTableData(query + $" LIMIT {LIMIT}");

            if (result != null && result.Count > 0)
            {
                // Set table name
                result.Table.TableName = "UnoperationalEqp";
                Instance.QueryDisplayItemsSource = null;
                Instance.QueryDisplayItemsSource = result;
                // Show the query
                Instance.QueryTextBox = query;
                //Display Query Panel
                MainVM.Instance.ShowView(MainVM.MenuItem.query_display);

                MainVM.Instance.AddMessage("This query gets all of the non-operational equipment, and the issue descriptions.");
            }
            else
            {
                MainVM.Instance.AddMessage("Query results are empty.");
            }

        }

        public void JuneEmployees()
        {
            // Get query from DB
            string query = "SELECT `employee`.`first_name`, `employee`.`last_name`, `employee_shift`.`date`, `employee_shift`.`shift_start` \n";
            query += "FROM `employee` INNER JOIN `employee_shift` ON `employee_shift`.`employee_id` = `employee`.`id` \n";
            query += "WHERE `employee_shift`.`date` = '2018-6-1' ";
            DataView result = DBManager.GetTableData(query + $" LIMIT {LIMIT}");

            if (result != null && result.Count > 0)
            {
                // Set table name
                result.Table.TableName = "JuneEmployees";
                Instance.QueryDisplayItemsSource = null;
                Instance.QueryDisplayItemsSource = result;
                // Show the query
                Instance.QueryTextBox = query;
                //Display Query Panel
                MainVM.Instance.ShowView(MainVM.MenuItem.query_display);

                MainVM.Instance.AddMessage("This query gets all the Employees that are working on 6/1/2018.");
            }
            else
            {
                MainVM.Instance.AddMessage("Query results are empty.");
            }

        }

        public void LargeFuelOrders()
        {
            // Get query from DB
            string query = "SELECT `flight`.`flight_num`, `fuel_order`.`type`, `fuel_order`.`amount_gallons`, `flight`.`scheduled_departure` FROM `fuel_order` \n";
            query += "INNER JOIN `flight` ON `fuel_order`.`flight_id` = `flight`.`id` \n";
            query += "WHERE `flight`.`scheduled_departure` < DATE_ADD(CURDATE(), INTERVAL 7 DAY) \n";
            query += "AND `flight`.`scheduled_departure` > CURDATE() AND `fuel_order`.`amount_gallons` > 30";
            DataView result = DBManager.GetTableData(query + $" LIMIT {LIMIT}");

            if (result != null && result.Count > 0)
            {
                // Set table name
                result.Table.TableName = "LargeFuelOrders";
                Instance.QueryDisplayItemsSource = null;
                Instance.QueryDisplayItemsSource = result;
                // Show the query
                Instance.QueryTextBox = query;
                //Display Query Panel
                MainVM.Instance.ShowView(MainVM.MenuItem.query_display);

                MainVM.Instance.AddMessage("This query gets all flights in the next week, with fuel orders over 30 gallons.");
            }
            else
            {
                MainVM.Instance.AddMessage("Query results are empty.");
            }

        }
        

        public void BagsInTransit()
        {
            // Get query from DB
            string query = "SELECT `bag`.`passenger_name`, `bag`.`weight_lbs`, `baggage_claim`.`delivery_method`, `baggage_claim`.`current_status`, `flight`.`actual_departure` \n";
            query += "FROM `bag` INNER JOIN `baggage_claim` ON `bag`.`id` = `baggage_claim`.`bag_id` INNER JOIN `flight` ON `bag`.`flight_id` = `flight`.`id` \n";
            query += "WHERE `baggage_claim`.`delivery_method` = 'air' AND `baggage_claim`.`current_status` = 'in transit' AND `flight`.`actual_departure` > DATE_SUB(CURDATE(), INTERVAL 7 DAY) AND `flight`.`actual_departure` < CURDATE() ";
            DataView result = DBManager.GetTableData(query + $" LIMIT {LIMIT}");

            if (result != null && result.Count > 0)
            {
                // Set table name
                result.Table.TableName = "BagsInTransit";
                Instance.QueryDisplayItemsSource = null;
                Instance.QueryDisplayItemsSource = result;
                // Show the query
                Instance.QueryTextBox = query;
                //Display Query Panel
                MainVM.Instance.ShowView(MainVM.MenuItem.query_display);

                MainVM.Instance.AddMessage("This query gets passengers and bag weights, for baggage claims which are in transit through the air, from flights last week.");
            }
            else
            {
                MainVM.Instance.AddMessage("Query results are empty.");
            }

        }


        #endregion

    }
}
