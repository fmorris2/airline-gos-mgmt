using AirlineDBMS.BackEnd;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    /// Interaction logic for BaggageClaimPanel.xaml
    /// </summary>
    public partial class BaggageClaimPanel : UserControl
    {
        public BaggageClaimPanel()
        {
            InitializeComponent();

            // List of delivery methods
            cbDeliveryMethod.ItemsSource = Enum.GetValues(typeof(DeliveryMethod)).Cast<DeliveryMethod>();
        }

        private enum DeliveryMethod {Air, Ground};

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(cbBagID.Text.Length == 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Please enter the ID of the bag you wish to create a claim for");
            }
            else if(cbDeliveryMethod.SelectedIndex == -1)
            {
                ViewModels.MainVM.Instance.AddMessage("Please select a delivery method for the claim of bag " + cbBagID.Text);
            }
            else //something is at least entered for each of the required fields
            {
                int bag_id = -1;
                Boolean parse_success = Int32.TryParse(cbBagID.Text, out bag_id);
                if(!parse_success)
                {
                    ViewModels.MainVM.Instance.AddMessage("Please enter a valid 32 bit integer in the bag id field");
                }
                else if(BagCanBeClaimed(bag_id))
                {
                    AddBagClaimToDB(bag_id, (DeliveryMethod)cbDeliveryMethod.SelectedValue);
                }
            }
            
        }

        private Boolean BagCanBeClaimed(int bag_id)
        {
            if (!BagExists(bag_id))
            {
                ViewModels.MainVM.Instance.AddMessage("Bag ID " + cbBagID.Text + " does not exist in the database");
                return false;
            }

            if (ClaimAlreadyExists(bag_id))
            {
                ViewModels.MainVM.Instance.AddMessage("A claim for bag ID " + cbBagID.Text + " already exists in the database");
                return false;
            }

            return true;
        }

        private Boolean BagExists(int bag_id)
        {
            MySqlDataReader reader = DBManager.Query("SELECT `id` FROM `bag` WHERE `id`=" + bag_id);
            if (reader == null) return false;
            Boolean bag_exists = reader.HasRows;
            reader.Close();
            return bag_exists;
        }

        private Boolean ClaimAlreadyExists(int bag_id)
        {
            MySqlDataReader reader = DBManager.Query("SELECT `id` FROM `baggage_claim` WHERE `bag_id`=" + bag_id);
            if (reader == null) return false;
            Boolean claim_exists = reader.HasRows;
            reader.Close();
            return claim_exists;
        }

        private void AddBagClaimToDB(int bag_id, DeliveryMethod deliv_method)
        {
            DateTime today = DateTime.Today;
            String formattedDateTime = today.ToString("yyyyMMdd");

            MySqlDataReader result = DBManager.Query("INSERT INTO `baggage_claim`(bag_id,request_date,delivery_method,current_status)" +
                " VALUES(" + bag_id + ",'" + formattedDateTime + "','" + deliv_method.ToString() + "',"
                    + "'waiting')");
            
            if (result != null && result.RecordsAffected > 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Successfully created baggage claim for bag ID " + bag_id 
                    + ", delivered by " + deliv_method.ToString());

                result.Close();
            }
            else
            {
                ViewModels.MainVM.Instance.AddMessage("Failed to create baggage claim for bag ID " + bag_id + ": Internal error");
            }

        }
    }
}
