﻿using AirlineDBMS.BackEnd;
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
        MainWindow mw = new MainWindow();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            mw.Show();
        }
    }
}
