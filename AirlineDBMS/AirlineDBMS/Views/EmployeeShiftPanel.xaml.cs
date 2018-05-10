using AirlineDBMS.BackEnd;
using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace AirlineDBMS.Views
{
    /// <summary>
    /// Interaction logic for EmployeeShiftPanel.xaml
    /// </summary>
    public partial class EmployeeShiftPanel : UserControl
    {
        public EmployeeShiftPanel()
        {
            InitializeComponent();
            shiftStartSelector.ShowDropDownButton = false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // $ sign before a string allows you to use {variablename} for display
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            int employee_id = -1;
            Boolean parse_success = Int32.TryParse(tbEmployeeID.Text, out employee_id);

            //massive block of if/else statements for checking input, could've abstracted out of this method however 
            //chose to do quick & dirty to get it done
            if (tbEmployeeID.Text.Length == 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Please enter an employee ID");
            }
            else if(!parse_success)
            {
                ViewModels.MainVM.Instance.AddMessage("Employee ID must be a valid 32 bit integer");
            }
            else if(dateSelector.SelectedDate == null)
            {
                ViewModels.MainVM.Instance.AddMessage("Please select a shift date");
            }
            else if(shiftStartSelector.Text != null && shiftStartSelector.Text.Length == 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Please select a shift start time");
            }
            else if(shiftEndSelector.Text != null && shiftEndSelector.Text.Length == 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Please select a shift end time");
            }
            else if(!EmployeeExists(employee_id))
            {
                ViewModels.MainVM.Instance.AddMessage($"Employee ID {employee_id} does not exist in the database");
            }
            else if(EmployeeAlreadyWorks(employee_id, dateSelector.SelectedDate.Value))
            {
                ViewModels.MainVM.Instance.AddMessage($"Employee ID {employee_id} already works on {dateSelector.SelectedDate.Value}");
            }
            else
            {
                InsertIntoDB(employee_id, dateSelector.SelectedDate.Value, shiftStartSelector.Value.Value, shiftEndSelector.Value.Value);
            }
        }

        private Boolean EmployeeAlreadyWorks(int emp_id, DateTime date)
        {
            MySqlDataReader reader = DBManager.Query($"SELECT `id` FROM `employee_shift` WHERE `employee_id`={emp_id}" +
                $" AND `date`='{date.ToString("yyyy-MM-dd")}'");

            if (reader == null) return false;
            Boolean already_works = reader.HasRows;
            reader.Close();
            return already_works;
        }

        private void InsertIntoDB(int employee_id, DateTime selected_date, DateTime shift_start, DateTime shift_end)
        {
            String formattedDate = selected_date.ToString("yyyy-MM-dd");
            String formattedStartTime = shift_start.ToString("HH:mm:ss");
            String formattedEndTime = shift_end.ToString("HH:mm:ss");

            MySqlDataReader result = DBManager.Query("INSERT INTO `employee_shift`(employee_id,`date`,shift_start,shift_end)" +
                " VALUES(" + employee_id + ",'" + formattedDate + "','" + formattedStartTime + "','"+formattedEndTime+"')");

            if (result != null && result.RecordsAffected > 0)
            {
                ViewModels.MainVM.Instance.AddMessage($"Successfully created employee shift for employee {employee_id} on" +
                    $" {selected_date} from {formattedStartTime} to {formattedEndTime}");

                result.Close();
            }
            else
            {
                ViewModels.MainVM.Instance.AddMessage($"Failed to create shift for employee ID {employee_id}: Internal error");
            }
        }

        private Boolean EmployeeExists(int employee_id)
        {
            MySqlDataReader reader = DBManager.Query("SELECT `id` FROM `employee` WHERE `id`=" + employee_id);
            if (reader == null) return false;
            Boolean emp_exists = reader.HasRows;
            reader.Close();
            return emp_exists;
        }
    }
}
