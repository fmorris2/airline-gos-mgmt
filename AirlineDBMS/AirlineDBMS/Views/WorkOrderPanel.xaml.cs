using AirlineDBMS.BackEnd;
using AirlineDBMS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for WorkOrderPanel.xaml
    /// </summary>
    public partial class WorkOrderPanel : UserControl
    {
        public WorkOrderPanel()
        {
            InitializeComponent();
            Equipment.LoadEquipment();
            cbEquipment.ItemsSource = Equipment.loadedEquipment;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Equipment selected_equipment = (Equipment)cbEquipment.SelectedItem;
            String issue_desc = issueDescBox.Text;

            if (selected_equipment == null)
            {
                ViewModels.MainVM.Instance.AddMessage("Please select a piece of equipment to create a work order for");
            }
            else if(issue_desc.Length == 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Please input an " +
                    "appropriate description for the issue with " + selected_equipment.GetName());
            }
            else
            {
                DateTime today = DateTime.Today;
                String formattedDateTime = today.ToString("yyyyMMdd");

                MySqlDataReader result = DBManager.Query("INSERT INTO `work_order`(equipment_id,issue_desc,request_date)" +
                    " VALUES("+selected_equipment.GetId()+",'"+issue_desc+"','"+formattedDateTime+"')");

                if(result.RecordsAffected > 0)
                {
                    ViewModels.MainVM.Instance.AddMessage("Successfully created work order for " + cbEquipment.Text + ": "
                        + "\""+issue_desc+"\"");
                }
                else
                {
                    ViewModels.MainVM.Instance.AddMessage("Failed to create work order for " + cbEquipment.Text + ": Internal error");
                }
            }
        }
    }
}
