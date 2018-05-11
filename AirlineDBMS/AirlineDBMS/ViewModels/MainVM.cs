using AirlineDBMS.BackEnd;
using AirlineDBMS.Models;
using AirlineDBMS.Views;
using MySql.Data.MySqlClient;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace AirlineDBMS.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        private static object lockObj = new object();
        private static object statusObj = new object();
        private static volatile MainVM instance;
        QueryDisplayVM qvm = QueryDisplayVM.Instance;

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
            SwitchUserGroup = new DelegateCommand(ExecuteSwitchUserGroup, CanSwitchUserGroup);
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
        public DelegateCommand SwitchUserGroup { get; private set; }

        #region TopButtons
        // Employee Shift menu button select
        public DelegateCommand AddEmpShiftCommand { get; private set; }

        private void OnEmpShiftMenu()
        {
            ShowView(MenuItem.new_emp_shift);
        }
        private bool CanEmpShiftMenu()
        {
            return true;
        }

        // At Bag Claim menu button select
        public DelegateCommand NewBagClaimCommand { get; private set; }

        private void OnBagClaimMenu()
        {
            ShowView(MenuItem.new_bag_claim);
        }
        private bool CanBagClaimMenu()
        {
            return true;
        }

        // Create new work order menu button
        public DelegateCommand NewWorkOrderCommand { get; private set; }

        private void OnWorkOrderMenu()
        {
            ShowView(MenuItem.new_work_order);
        }
        private bool CanWorkOrderMenu()
        {
            return true;
        }

        // Create new fuel order menu button
        public DelegateCommand NewFuelOrderCommand { get; private set; }

        private void OnFuelOrderMenu()
        {
            ShowView(MenuItem.new_fuel_order);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `employee` ORDER BY `employment_start` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                // Set table name for refresh button in querydisplaypanel.xaml
                result.Table.TableName = "Employees";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `employee_shift` ORDER BY `date` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if(result != null && result.Count > 0)
            {
                result.Table.TableName = "EmployeeSched";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `bag` ORDER BY `id` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                result.Table.TableName = "Bags";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `baggage_claim` ORDER BY `request_date` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                result.Table.TableName = "BagClaims";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `equipment` ORDER BY `id` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                result.Table.TableName = "Equipment";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `work_order` ORDER BY `request_date` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                result.Table.TableName = "WorkOrders";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `flight` ORDER BY `scheduled_departure` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                result.Table.TableName = "Flights";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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
            DataView result = DBManager.GetTableData($"SELECT * FROM `fuel_order` ORDER BY `id` DESC LIMIT {qvm.LIMIT}");

            // If we got something toss it in the DataGrid
            if (result != null && result.Count > 0)
            {
                result.Table.TableName = "FuelOrders";
                QueryDisplayVM.Instance.QueryDisplayItemsSource = null;
                QueryDisplayVM.Instance.QueryDisplayItemsSource = result;
                ShowView(MenuItem.query_display);
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

        Boolean CanSwitchUserGroup()
        {
            return true;
        }

        void ExecuteSwitchUserGroup()
        {
            switch(User.instance.GetUserGroup())
            {
                case User.Group.Auditor:
                    User.LoadInstance("employee", "test");
                    AddMessage("Switched user-group to Employee");
                    break;
                case User.Group.Employee:
                    User.LoadInstance("manager", "test");
                    AddMessage("Switched user-group to Manager");
                    break;
                case User.Group.Manager:
                    User.LoadInstance("auditor", "test");
                    AddMessage("Switched user-group to Auditor");
                    break;
            }
            MainWindow.instance.UpdateInterfaceForUserGroup();
            ShowView(MenuItem.none);
        }

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
            System.Threading.Thread.Sleep(8000);
            if (currentStatus == StatusId && !ManuallyOpened)
            {
                MainVM.Instance.OpenFlyout = false;
            }
        }

        public enum MenuItem {new_bag_claim, new_emp_shift, new_work_order, new_fuel_order, query_display, none}

        private void SetMenuItem(MenuItem item, Boolean visible)
        {
            Console.WriteLine($"ShowMenuItem({item}, {visible})");
            Boolean isException = IsException(item);
            Visibility off = isException ? Visibility.Hidden : Visibility.Collapsed;
            Visibility on = isException ? Visibility.Hidden : Visibility.Visible;
            switch (item)
            {
                case MenuItem.new_bag_claim:
                    NewBagClaimVisible = visible ? on : off;
                    break;
                case MenuItem.new_emp_shift:
                    EmpShiftVisible = visible ? on : off;
                    break;
                case MenuItem.new_fuel_order:
                    NewFuelOrderVisible = visible ? on : off;
                    break;
                case MenuItem.new_work_order:
                    NewWorkOrderVisible = visible ? on : off;
                    break;
                case MenuItem.query_display:
                    QueryDisplayVM.Instance.QueryDisplayVisible = visible ? on : off;
                    break;
            }
        }

        public void ShowView(MenuItem item)
        {
            SetMenuItem(item, true);
            foreach (MenuItem topMenuItem in Enum.GetValues(typeof(MenuItem)))
            {
                if(topMenuItem != item)
                {
                    SetMenuItem(topMenuItem, false);
                }
            }
        }
        
        private Boolean IsException(MenuItem view)
        {
            User.Group group = User.instance.GetUserGroup();
            if(group == User.Group.Auditor && view != MenuItem.query_display)
            {
                return true;
            }

            if(group == User.Group.Employee && view == MenuItem.new_emp_shift)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
