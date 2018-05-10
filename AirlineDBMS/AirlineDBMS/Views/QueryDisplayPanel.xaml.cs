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
using System.Windows.Shapes;

namespace AirlineDBMS.Views
{
    /// <summary>
    /// Interaction logic for QueryDisplayPanel.xaml
    /// </summary>
    public partial class QueryDisplayPanel : UserControl
    {
        public QueryDisplayPanel()
        {
            this.DataContext = ViewModels.QueryDisplayVM.Instance;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // use the table name of the current data set to figure out what we need to refresh
            ViewModels.QueryDisplayVM qvm = ViewModels.QueryDisplayVM.Instance;
            ViewModels.MainVM mainvm = ViewModels.MainVM.Instance;
            string t = qvm.QueryDisplayItemsSource.Table.TableName;
            switch (t)
            {
                case "Employees":
                    mainvm.EmployeesCommand.Execute();
                    break;
                case "EmployeeSched":
                    mainvm.EmployeeSchedCommand.Execute();
                    break;
                case "Bags":
                    mainvm.BagsCommand.Execute();
                    break;
                case "BagClaims":
                    mainvm.BagClaimsCommand.Execute();
                    break;
                case "Equipment":
                    mainvm.EquipmentCommand.Execute();
                    break;
                case "WorkOrders":
                    mainvm.WorkOrdersCommand.Execute();
                    break;
                case "Flights":
                    mainvm.FlightsCommand.Execute();
                    break;
                case "FuelOrders":
                    mainvm.FuelOrdersCommand.Execute();
                    break;
                default:
                    break;
            }
            }
        }
    }
