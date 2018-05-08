using AirlineDBMS.BackEnd;
using AirlineDBMS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

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
                InsertWorkOrderIntoDB(selected_equipment, issue_desc);
            }
        }

        private void InsertWorkOrderIntoDB(Equipment selected_equipment, String issue_desc)
        {
            DateTime today = DateTime.Today;
            String formattedDateTime = today.ToString("yyyyMMdd");

            MySqlDataReader result = DBManager.Query("INSERT INTO `work_order`(equipment_id,issue_desc,request_date)" +
                " VALUES(" + selected_equipment.GetId() + ",'" + issue_desc + "','" + formattedDateTime + "')");

            if (result.RecordsAffected > 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Successfully created work order for " + cbEquipment.Text + ": "
                    + "\"" + issue_desc + "\"");
            }
            else
            {
                ViewModels.MainVM.Instance.AddMessage("Failed to create work order for " + cbEquipment.Text + ": Internal error");
            }
        }
    }
}
