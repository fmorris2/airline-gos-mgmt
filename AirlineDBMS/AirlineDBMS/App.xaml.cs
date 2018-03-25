using AirlineDBMS.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AirlineDBMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        LoginWindow lw = new LoginWindow();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            lw.DataContext = ViewModels.LoginVM.Instance;
            lw.Show();
        }
    }
}
