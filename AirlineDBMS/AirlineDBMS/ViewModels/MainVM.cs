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
    class MainVM : INotifyPropertyChanged
    {
        public readonly BackgroundWorker loadWorker = new BackgroundWorker();
        private static object lockObj = new object();
        private static volatile MainVM instance;

        #region Constructor/Instance
        public static MainVM Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new MainVM();
                    }
                }
                return instance;
            }
        }

        public MainVM()
        {
            RegisterCommands();
            loadWorker.DoWork += loadWorker_DoWork;
            loadWorker.RunWorkerCompleted += loadWorker_RunWorkerCompleted;
        }

        private void RegisterCommands()
        {
            AddEmpShiftCommand = new DelegateCommand(OnEmpShiftMenu, CanEmpShiftMenu);
            NewBagClaimCommand = new DelegateCommand(OnBagClaimMenu, CanBagClaimMenu);
            NewWorkOrderCommand = new DelegateCommand(OnWorkOrderMenu, CanWorkOrderMenu);
            NewFuelOrderCommand = new DelegateCommand(OnFuelOrderMenu, CanFuelOrderMenu);

            SearchCommand = new DelegateCommand(OnSearch, CanSearch);
        }
        #endregion

        #region workers
        private void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void loadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
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

        private Visibility bagClaimVisible = Visibility.Collapsed;
        public Visibility BagClaimVisible
        {
            get
            {
                return bagClaimVisible;
            }
            set
            {
                if (bagClaimVisible != value)
                {
                    bagClaimVisible = value;
                    NotifyPropertyChanged("BagClaimVisible");
                }
            }
        }
        private Visibility empShiftVisible = Visibility.Collapsed;
        public Visibility EmpShiftVisible
        {
            get
            {
                return empShiftVisible;
            }
            set
            {
                if (empShiftVisible != value)
                {
                    empShiftVisible = value;
                    NotifyPropertyChanged("EmpShiftVisible");
                }
            }
        }
        private Visibility workOrderVisible = Visibility.Collapsed;
        public Visibility WorkOrderVisible
        {
            get
            {
                return workOrderVisible;
            }
            set
            {
                if (workOrderVisible != value)
                {
                    workOrderVisible = value;
                    NotifyPropertyChanged("WorkOrderVisible");
                }
            }
        }
        private Visibility fuelOrderVisible = Visibility.Collapsed;
        public Visibility FuelOrderVisible
        {
            get
            {
                return fuelOrderVisible;
            }
            set
            {
                if (fuelOrderVisible != value)
                {
                    // toggle highlight of menu item based on value
                    fuelOrderVisible = value;
                    NotifyPropertyChanged("FuelOrderVisible");
                }
            }
        }

        #endregion

        #region Commands
        // Employee Shift menu button select
        public DelegateCommand AddEmpShiftCommand { get; private set; }

        private void OnEmpShiftMenu()
        {
            ShowMenuItem("NewEmployeeShift");
        }
        private bool CanEmpShiftMenu()
        {
            return true;
        }

        // At Bag Claim menu button select
        public DelegateCommand NewBagClaimCommand { get; private set; }

        private void OnBagClaimMenu()
        {
            ShowMenuItem("NewBaggageClaim");
        }
        private bool CanBagClaimMenu()
        {
            return true;
        }

        // Create new work order menu button
        public DelegateCommand NewWorkOrderCommand { get; private set; }

        private void OnWorkOrderMenu()
        {
            ShowMenuItem("NewWorkOrder");
        }
        private bool CanWorkOrderMenu()
        {
            return true;
        }

        // Create new fuel order menu button
        public DelegateCommand NewFuelOrderCommand { get; private set; }

        private void OnFuelOrderMenu()
        {
            ShowMenuItem("NewFuelOrder");
        }
        private bool CanFuelOrderMenu()
        {
            return true;
        }


        // Display Employee schedule
        public DelegateCommand EmployeeSchedCommand { get; private set; }

        private void OnEmpSched()
        {
            ShowMenuItem("ShowEmpSched");
        }
        private bool CanEmpSched()
        {
            return true;
        }

        // Display bag claims
        public DelegateCommand BagClaimsCommand { get; private set; }

        private void ShowBagClaim()
        {
            ShowMenuItem("ShowBagClaims");
        }
        private bool CanShowBagClaims()
        {
            return true;
        }

        // Display bag claims
        public DelegateCommand WorkOrdersCommand { get; private set; }

        private void ShowWorkOrders()
        {
            ShowMenuItem("ShowWorkOrder");
        }
        private bool CanShowWorkOrders()
        {
            return true;
        }

        // Display bag claims
        public DelegateCommand FuelOrderCommand { get; private set; }

        private void ShowFuelOrders()
        {
            ShowMenuItem("ShowFuelOrder");
        }
        private bool CanShowFuelOrders()
        {
            return true;
        }

        // Search button in the corner
        public DelegateCommand SearchCommand { get; private set; }

        private void OnSearch()
        {
            ShowMenuItem("Search");
        }
        private bool CanSearch()
        {
            return true;
        }
        #endregion

        #region HelperMethods

        // Show a certain menu when it is clicked
        private void ShowMenuItem(string open)
        {
            switch (open)
            {
                case "NewEmployeeShift":
                    BagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Visible;
                    WorkOrderVisible = Visibility.Collapsed;
                    FuelOrderVisible = Visibility.Collapsed;
                    break;
                case "NewBaggageClaim":
                    BagClaimVisible = Visibility.Visible;
                    EmpShiftVisible = Visibility.Collapsed;
                    WorkOrderVisible = Visibility.Collapsed;
                    FuelOrderVisible = Visibility.Collapsed;
                    break;
                case "NewWorkOrder":
                    BagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    WorkOrderVisible = Visibility.Visible;
                    FuelOrderVisible = Visibility.Collapsed;
                    break;
                case "NewFuelOrder":
                    BagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    WorkOrderVisible = Visibility.Collapsed;
                    FuelOrderVisible = Visibility.Visible;
                    break;
                case "EmployeeSched":
                    BagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Visible;
                    WorkOrderVisible = Visibility.Collapsed;
                    FuelOrderVisible = Visibility.Collapsed;
                    break;
                case "BagClaims":
                    BagClaimVisible = Visibility.Visible;
                    EmpShiftVisible = Visibility.Collapsed;
                    WorkOrderVisible = Visibility.Collapsed;
                    FuelOrderVisible = Visibility.Collapsed;
                    break;
                case "WorkOrders":
                    BagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    WorkOrderVisible = Visibility.Visible;
                    FuelOrderVisible = Visibility.Collapsed;
                    break;
                case "FuelOrders":
                    BagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    WorkOrderVisible = Visibility.Collapsed;
                    FuelOrderVisible = Visibility.Visible;
                    break;
                case "Search":
                    BagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    WorkOrderVisible = Visibility.Collapsed;
                    FuelOrderVisible = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
