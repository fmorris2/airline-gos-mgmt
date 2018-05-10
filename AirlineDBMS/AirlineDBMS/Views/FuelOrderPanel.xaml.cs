using AirlineDBMS.BackEnd;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirlineDBMS.Views
{
    /// <summary>
    /// Interaction logic for FuelOrderPanel.xaml
    /// </summary>
    public partial class FuelOrderPanel : UserControl
    {
        public FuelOrderPanel()
        {
            InitializeComponent();

            fTypeBox.ItemsSource = new ObservableCollection<string>
            {
                "Jet A-1", "Jet B"
            };
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(tbFlightID.Text.Length == 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Please enter the ID of the flight you wish to create a fuel order for");
            }
            else if(fTypeBox.SelectedIndex == -1)
            {
                ViewModels.MainVM.Instance.AddMessage("Please select a fuel type");
            }
            else if(amountSpinner.Value <= 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Fuel amount must be > 0");
            }
            else
            {
                AddFuelOrderToDB();
            }
        }

        private void AddFuelOrderToDB()
        {
            int flight_id = -1;
            Boolean parse_success = Int32.TryParse(tbFlightID.Text, out flight_id);
            if(!parse_success)
            {
                ViewModels.MainVM.Instance.AddMessage("FLight ID must be a valid 32 bit integer");
            }
            else if(!FlightExists(flight_id))
            {
                ViewModels.MainVM.Instance.AddMessage("Flight ID " + flight_id + " does not exist!");
            }
            else //insert into DB
            {
                HandleInsertionIntoDB(flight_id);
            }
        }

        private void HandleInsertionIntoDB(int flight_id)
        {
            MySqlDataReader result = DBManager.Query("INSERT INTO `fuel_order`(flight_id,type,amount_gallons)" +
                " VALUES(" + flight_id + ",'" + fTypeBox.SelectedValue + "'," + amountSpinner.Value + ")");

            if (result != null && result.RecordsAffected > 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Successfully created fuel order for flight ID " + flight_id
                    + ": " + amountSpinner.Value + " gallons of " + fTypeBox.SelectedValue);

                result.Close();
            }
            else
            {
                ViewModels.MainVM.Instance.AddMessage("Failed to create fuel order for flight ID " + flight_id + ": Internal error");
            }
        }

        private Boolean FlightExists(int flight_id)
        {
            MySqlDataReader reader = DBManager.Query("SELECT `id` FROM `flight` WHERE `id`=" + flight_id);
            if (reader == null) return false;
            Boolean flight_exists = reader.HasRows;
            reader.Close();
            return flight_exists;
        }
    }
}
