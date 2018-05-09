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
            if(tbEmployeeID.Text.Length == 0)
            {
                ViewModels.MainVM.Instance.AddMessage("Please enter an employee ID");
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
        }
    }
}
