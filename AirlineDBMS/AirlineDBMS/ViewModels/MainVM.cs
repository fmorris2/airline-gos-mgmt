using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AirlineDBMS.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        public readonly BackgroundWorker loadWorker = new BackgroundWorker();
        private static object lockObj = new object();
        private static volatile MainVM instance;
        private static Thread statusFlyoutThread = null;

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

            EmployeeSchedCommand = new DelegateCommand(OnEmpSched, CanEmpSched);
            BagClaimsCommand = new DelegateCommand(ShowBagClaim, CanShowBagClaims);
            WorkOrdersCommand = new DelegateCommand(ShowWorkOrders, CanShowWorkOrders);
            FuelOrdersCommand = new DelegateCommand(ShowFuelOrders, CanShowFuelOrders);

            StatusCommand = new DelegateCommand(OnStatus, CanStatus);
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

        private Visibility searchVisible = Visibility.Collapsed;
        public Visibility SearchVisible
        {
            get
            {
                return searchVisible;
            }
            set
            {
                if (searchVisible != value)
                {
                    // toggle highlight of menu item based on value
                    searchVisible = value;
                    NotifyPropertyChanged("SearchVisible");
                }
            }
        }

        #endregion

        #region SideButtons

        private Visibility showEmpSchedVisible = Visibility.Collapsed;
        public Visibility ShowEmpSchedVisible
        {
            get
            {
                return showEmpSchedVisible;
            }
            set
            {
                if (showEmpSchedVisible != value)
                {
                    // toggle highlight of menu item based on value
                    showEmpSchedVisible = value;
                    NotifyPropertyChanged("ShowEmpSchedVisible");
                }
            }
        }

        private Visibility showBagClaimsVisible = Visibility.Collapsed;
        public Visibility ShowBagClaimsVisible
        {
            get
            {
                return showBagClaimsVisible;
            }
            set
            {
                if (showBagClaimsVisible != value)
                {
                    // toggle highlight of menu item based on value
                    showBagClaimsVisible = value;
                    NotifyPropertyChanged("ShowBagClaimsVisible");
                }
            }
        }

        private Visibility showWorkOrdersVisible = Visibility.Collapsed;
        public Visibility ShowWorkOrdersVisible
        {
            get
            {
                return showWorkOrdersVisible;
            }
            set
            {
                if (showWorkOrdersVisible != value)
                {
                    // toggle highlight of menu item based on value
                    showWorkOrdersVisible = value;
                    NotifyPropertyChanged("ShowWorkOrdersVisible");
                }
            }
        }

        private Visibility showFuelOrdersVisible = Visibility.Collapsed;
        public Visibility ShowFuelOrdersVisible
        {
            get
            {
                return showFuelOrdersVisible;
            }
            set
            {
                if (showFuelOrdersVisible != value)
                {
                    // toggle highlight of menu item based on value
                    showFuelOrdersVisible = value;
                    NotifyPropertyChanged("ShowFuelOrdersVisible");
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
                if (openFlyout != value)
                {
                    openFlyout = value;
                    NotifyPropertyChanged("OpenFlyout");
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
            ShowMenuItem("ShowWorkOrders");
        }
        private bool CanShowWorkOrders()
        {
            return true;
        }

        // Display bag claims
        public DelegateCommand FuelOrdersCommand { get; private set; }

        private void ShowFuelOrders()
        {
            ShowMenuItem("ShowFuelOrders");
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
            statusItems.Add(msg);
            // Notify user by changing button color
            NewMsg = true;
        }
        
        // Background thread that opens the status message flyout for a few seconds
        private Thread FlyoutOpenCloseTimer = new Thread(() =>
        {
            Console.WriteLine("FlyoutOpenCloseTimer Thread Started!");
            MainVM.Instance.OpenFlyout = true;
            System.Threading.Thread.Sleep(3000);
            MainVM.Instance.OpenFlyout = false;
        });

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
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "NewBaggageClaim":
                    NewBagClaimVisible = Visibility.Visible;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "NewWorkOrder":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Visible;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "NewFuelOrder":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Visible;
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "ShowEmpSched":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    ShowEmpSchedVisible = Visibility.Visible;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "ShowBagClaims":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Visible;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "ShowWorkOrders":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Visible;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "ShowFuelOrders":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Visible;
                    SearchVisible = Visibility.Collapsed;
                    break;
                case "Search":
                    NewBagClaimVisible = Visibility.Collapsed;
                    EmpShiftVisible = Visibility.Collapsed;
                    NewWorkOrderVisible = Visibility.Collapsed;
                    NewFuelOrderVisible = Visibility.Collapsed;
                    ShowEmpSchedVisible = Visibility.Collapsed;
                    ShowBagClaimsVisible = Visibility.Collapsed;
                    ShowWorkOrdersVisible = Visibility.Collapsed;
                    ShowFuelOrdersVisible = Visibility.Collapsed;
                    SearchVisible = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
