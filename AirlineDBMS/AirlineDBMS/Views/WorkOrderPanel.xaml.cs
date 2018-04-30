using AirlineDBMS.Models;
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ViewModels.MainVM.Instance.AddMessage($"Work Order created for Equipment ID \"{cbEquipment.Text}\".");
        }
    }
}
