﻿using AirlineDBMS.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirlineDBMS.ViewModels
{
    class LoginVM : INotifyPropertyChanged
    {

        #region Constructor/Instance
        public LoginVM()
        {
            LoginCommand = new DelegateCommand(OnLogin, CanLogin);
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        #endregion

        #region Properties
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        private Visibility isLoginVisible = Visibility.Visible;
        public Visibility IsLoginVisible
        {
            get
            {
                return isLoginVisible;
            }

            set
            {
                if (isLoginVisible != value)
                {
                    isLoginVisible = value;
                    NotifyPropertyChanged("IsLoginVisible");
                }
            }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Command for the Load button in Loader Window
        /// </summary>
        public DelegateCommand LoginCommand { get; private set; }

        private void OnLogin()
        {
            try
            {
                // Load could return whether the user is valid or not
                Console.WriteLine("LoginVM#OnLogin()");

                // If valid remove login box
                if (AirlineDBMS.Models.User.LoadInstance(Username, Password))
                {
                    IsLoginVisible = Visibility.Collapsed;
                }
                else
                {
                    Console.WriteLine("Invalid login credentials - Please try again");
                }
                
            }
            catch (Exception exc)
            {
            }
        }
        private bool CanLogin()
        {
            return true;
        }

        #endregion

    }
}
