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
            cbDeliveryMethod.ItemsSource = new ObservableCollection<string>
            {
                "Air", "Ground"
            };
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            // Check all fields were filled in
            ViewModels.MainVM.Instance.AddMessage($"Claim for BagID \"{cbBagID.Text}\" has been created.");
        }
    }
}
