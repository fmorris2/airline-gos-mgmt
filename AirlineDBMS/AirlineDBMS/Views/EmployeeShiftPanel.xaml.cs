using System;
using System.Windows;
using System.Windows.Controls;
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
        }

        private string empID = "";
        public string EmpID
        {
            get
            {
                return empID;
            }
            set
            {
                empID = value;
            }
        }

        private DateTime startDate = DateTime.Today;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
            }
        }

        // $ sign before a string allows you to use {variablename} for display
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            ViewModels.MainVM.Instance.AddMessage($"Employee \"{tbEmployeeID.Text}\" shift has been updated.");
            Console.WriteLine("Submit Employee Shift: EmpId: " + EmpID + ", startDate: " + StartDate + ", endDate: " + EndDate);
        }
    }
}
