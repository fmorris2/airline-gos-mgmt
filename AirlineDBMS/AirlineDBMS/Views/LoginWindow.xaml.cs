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
        public LoginWindow()
        {                
            InitializeComponent();
            LoginVM viewModel = new LoginVM();
            this.DataContext = viewModel;
        }

        /*
         * This is the event handler for clicking the login button
         * Need to verify user input (username / pass fields), which
         * at the moment is a simple length check.
         * 
         * We then disable the login button for the course of the user
         * validation logic. Not sure how C# handles more button clicks
         * if the first one isn't completely handled, so this is my attempt
         * at throttling event calls (won't be the fastest since we're remotely
         * connecting to the DB)
         * 
         * We will then call the staic Load method in the User class to load up the user.
         * If the user is successfully loaded, we'll have to store the user object somewhere (Singleton / static variable?)
         * and load up the main view. Otherwise, display a status message and do nothing
         * 
         * @author Fred
         */
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if (usernameField.Text.Length > 0 && passwordField.Text.Length > 0)
            {
                loginButton.IsEnabled = false;
                System.Console.WriteLine("Validate");
                AirlineDBMS.BackEnd.User.Load(usernameField.Text, passwordField.Text);
                loginButton.IsEnabled = true;
            }
            else
            {
                System.Console.WriteLine("Bad Input");
            }
        }
    }
}
