using AirlineDBMS.BackEnd;
using MySql.Data.MySqlClient;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AirlineDBMS.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        private static object lockObj = new object();
        private static object statusObj = new object();
        private static volatile MainVM instance;
        private MySqlDataAdapter mySqlDataAdapter;

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
        }

        private void RegisterCommands()
        {
            AddEmpShiftCommand = new DelegateCommand(OnEmpShiftMenu, CanEmpShiftMenu);
            NewBagClaimCommand = new DelegateCommand(OnBagClaimMenu, CanBagClaimMenu);
            NewWorkOrderCommand = new DelegateCommand(OnWorkOrderMenu, CanWorkOrderMenu);
            NewFuelOrderCommand = new DelegateCommand(OnFuelOrderMenu, CanFuelOrderMenu);

            EmployeesCommand = new DelegateCommand(ShowEmployees, CanShowEmployees);
            EmployeeSchedCommand = new DelegateCommand(OnEmpSched, CanEmpSched);
            BagsCommand = new DelegateCommand(ShowBags, CanShowBags);
            BagClaimsCommand = new DelegateCommand(ShowBagClaim, CanShowBagClaims);
            EquipmentCommand = new DelegateCommand(ShowEquipment, CanShowEquipment);
            WorkOrdersCommand = new DelegateCommand(ShowWorkOrders, CanShowWorkOrders);
            FlightsCommand = new DelegateCommand(ShowFlights, CanShowFlights);
            FuelOrdersCommand = new DelegateCommand(ShowFuelOrders, CanShowFuelOrders);

            StatusCommand = new DelegateCommand(OnStatus, CanStatus);
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

        #region TopButtons

        private Visibility newBagClaimVisible = Visibility.Collapsed;
        public Visibility NewBagClaimVisible
        {
            get
            {
                return newBagClaimVisible;
            }
            set
            {
                if (newBagClaimVisible != value)
                {
                    newBagClaimVisible = value;
                    NotifyPropertyChanged("NewBagClaimVisible");
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
        private Visibility newWorkOrderVisible = Visibility.Collapsed;
        public Visibility NewWorkOrderVisible
        {
            get
            {
                return newWorkOrderVisible;
            }
            set
            {
                if (newWorkOrderVisible != value)
                {
                    newWorkOrderVisible = value;
                    NotifyPropertyChanged("NewWorkOrderVisible");
                }
            }
        }
        private Visibility newFuelOrderVisible = Visibility.Collapsed;
        public Visibility NewFuelOrderVisible
        {
            get
            {
                return newFuelOrderVisible;
            }
            set
            {
                if (newFuelOrderVisible != value)
                {
                    // toggle highlight of menu item based on value
                    newFuelOrderVisible = value;
                    NotifyPropertyChanged("NewFuelOrderVisible");
                }
            }
        }

        #endregion

        #endregion

        #region Commands

        #region TopButtons
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


        // Status button in the corner
        public DelegateCommand StatusCommand { get; private set; }

        private void OnStatus()
        {
            Console.WriteLine("MainVM.cs#OnStatus()");
            // Update flag to block timers from closing
            // If flyout is closed set flag
            if (!OpenFlyout)
            {
                ManuallyOpened = true;
            }
            else
            {
                ManuallyOpened = false;
            }
            // Open flyout
            OpenFlyout = !OpenFlyout;
            // Set button color back to normal
            NewMsg = false;
        }

        private bool CanStatus()
        {
            return true;
        }

        private bool openFlyout = false;
        public bool OpenFlyout
        {
            get { return openFlyout; }
            set
            {
                lock (statusObj)
                {
                    if (openFlyout != value)
                    {
                        openFlyout = value;
                        NotifyPropertyChanged("OpenFlyout");
                    }
                }
            }
        }

        private ObservableCollection<string> statusItems;
        public ObservableCollection<string> StatusItems
        {
            get
            {
                if (statusItems == null)
                {
                    statusItems = new ObservableCollection<string>();
                }
                return statusItems;
            }
            set
            {
                if (statusItems != value)
                {
                    statusItems = value;
                    NotifyPropertyChanged("StatusItems");
                }
            }
        }

        private bool newMsg = false;
        public bool NewMsg
        {
            get { return newMsg; }
            set
            {
                if (newMsg != value)
                {
                    newMsg = value;
                    NotifyPropertyChanged("NewMsg");
                }
            }
        }

        #endregion

        #region Side buttons
        // Display employee table
        public DelegateCommand EmployeesCommand { get; private set; }

        private void ShowEmployees()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `employee` LIMIT 100");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanShowEmployees()
        {
            return true;
        }

        // Display Employee schedule
        public DelegateCommand EmployeeSchedCommand { get; private set; }

        private void OnEmpSched()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `equipment` LIMIT 100");

            // If we got something toss it in the DataGrid
            if(result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanEmpSched()
        {
            return true;
        }

        // Display bag table
        public DelegateCommand BagsCommand { get; private set; }

        private void ShowBags()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `bag` LIMIT 100");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanShowBags()
        {
            return true;
        }

        // Display bag claims
        public DelegateCommand BagClaimsCommand { get; private set; }

        private void ShowBagClaim()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `baggage_claim` LIMIT 100");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanShowBagClaims()
        {
            return true;
        }

        // Display Equipment table
        public DelegateCommand EquipmentCommand { get; private set; }

        private void ShowEquipment()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `equipment` LIMIT 100");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanShowEquipment()
        {
            return true;
        }

        // Display Work Order table
        public DelegateCommand WorkOrdersCommand { get; private set; }

        private void ShowWorkOrders()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `work_order` LIMIT 100");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanShowWorkOrders()
        {
            return true;
        }

        // Display Flight table
        public DelegateCommand FlightsCommand { get; private set; }

        private void ShowFlights()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `flight` LIMIT 100");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanShowFlights()
        {
            return true;
        }

        // Display Fuel Orders table
        public DelegateCommand FuelOrdersCommand { get; private set; }

        private void ShowFuelOrders()
        {
            // Get all the table data as a dataview
            DataView result = DBManager.GetTableData("SELECT * FROM `fuel_order` LIMIT 100");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowMenuItem("ShowQueryDisplay");
            }
            else
            {
                AddMessage("Query results are empty.");
            }
        }
        private bool CanShowFuelOrders()
        {
            return true;
        }

        #endregion

        #endregion

        #region HelperMethods

        // Add to the status box
        public void AddMessage(string msg)
        {
            // Add item to status box
            statusItems.Insert(0, msg);
            // Notify user by changing button color
            NewMsg = true;
            StatusId++;
            Task.Run(() =>
               FlyoutOpenCloseTimer()
            );
        }
        // Flag to reset a timer for the flyout
        private int statusId = 0;
        private int StatusId
        {
            get
            {
                lock (statusObj)
                {
                    return statusId;
                }
            }
            set
            {
                lock (statusObj)
                {
                    if (statusId != value)
                    {
                        statusId = value;
                    }
                }
            }
        }
        // Flag to keep flyout open if was opened manually
        private bool manuallyOpened = false;
        private bool ManuallyOpened
        {
            get
            {
                lock (statusObj)
                {
                    return manuallyOpened;
                }
            }
            set
            {
                lock (statusObj)
                {
                    if (manuallyOpened != value)
                    {
                        manuallyOpened = value;
                    }
                }
            }
        }
        // Background thread that opens the status message flyout for a few seconds
        private void FlyoutOpenCloseTimer()
        {
            Console.WriteLine("FlyoutOpenCloseTimer Thread Started!");
            MainVM.Instance.OpenFlyout = true;

            int currentStatus = StatusId;
            System.Threading.Thread.Sleep(3000);
            if (currentStatus == StatusId && !ManuallyOpened)
            {
                MainVM.Instance.OpenFlyout = false;
            }
        }

        // Show a certain menu when it is clicked
        private void ShowMenuItem(string open)
        {
            switch (open)
            {
                case "NewEmployeeShift":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Visible;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    QueryDisplayVM.Instance.QueryDisplayVisible = Visibility.Collapsed;
                    break;
                case "NewBaggageClaim":
                    NewBagClaimVisible = Visibility.Visible;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    QueryDisplayVM.Instance.QueryDisplayVisible = Visibility.Collapsed;
                    break;
                case "NewWorkOrder":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Visible;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    QueryDisplayVM.Instance.QueryDisplayVisible = Visibility.Collapsed;
                    break;
                case "NewFuelOrder":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Visible;
                    QueryDisplayVM.Instance.QueryDisplayVisible = Visibility.Collapsed;
                    break;
                case "ShowQueryDisplay":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    QueryDisplayVM.Instance.QueryDisplayVisible = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
