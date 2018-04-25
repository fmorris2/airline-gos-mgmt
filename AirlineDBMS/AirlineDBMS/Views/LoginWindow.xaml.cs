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
using MahApps.Metro.Controls;
using AirlineDBMS.ViewModels;

namespace AirlineDBMS.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        private LoginVM viewModel = new LoginVM();

        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        /**
         * These two methods are here to set the username & pass as the user is typing.
         * The reason for this is that originally they weren't being set until the text box
         * lost focus. So when Enter key support was implemented, it would throw a null pointer if
         * they didn't click off of the last text box before pressing enter. There is probably a better
         * way to do it, but since I don't know C#, this is what I came up with.
         * 
         * -Fred
         */
        public void Username_Changed(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            viewModel.Username = textBox.Text;
        }

        public void Password_Changed(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            viewModel.Password = textBox.Text;
        }
    }
}
