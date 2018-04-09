using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            BaggageClaimCommand = new DelegateCommand(OnBaggageClaim, CanBaggageClaim);
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

        #region Commands
        public DelegateCommand BaggageClaimCommand { get; private set; }

        private void OnBaggageClaim()
        {
            throw new NotImplementedException();
        }
        private bool CanBaggageClaim()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
