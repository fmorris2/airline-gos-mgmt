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
using MahApps.Metro.Controls;
using AirlineDBMS.ViewModels;
using AirlineDBMS.Models;

namespace AirlineDBMS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow instance;
        public MainWindow()
        {
            this.DataContext = MainVM.Instance;
            InitializeComponent();
            instance = this;
        }

        public void UpdateInterfaceForUserGroup()
        {
            User.Group group = User.instance.GetUserGroup();

            switch (group)
            {
                case User.Group.Auditor:
                    addEmpShiftButton.Visibility = Visibility.Hidden;
                    addBagClaimButton.Visibility = Visibility.Hidden;
                    addFuelOrderButton.Visibility = Visibility.Hidden;
                    addWorkOrderButton.Visibility = Visibility.Hidden;
                    break;
                case User.Group.Employee:
                    addEmpShiftButton.Visibility = Visibility.Hidden;
                    addBagClaimButton.Visibility = Visibility.Visible;
                    addFuelOrderButton.Visibility = Visibility.Visible;
                    addWorkOrderButton.Visibility = Visibility.Visible;
                    break;
                case User.Group.Manager:
                    addBagClaimButton.Visibility = Visibility.Visible;
                    addFuelOrderButton.Visibility = Visibility.Visible;
                    addWorkOrderButton.Visibility = Visibility.Visible;
                    addEmpShiftButton.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
