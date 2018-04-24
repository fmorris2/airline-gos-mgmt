using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
