using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirlineDBMS.ViewModels
{
    class QueryDisplayVM : INotifyPropertyChanged
    {
        private static object lockObj = new object();
        private static volatile QueryDisplayVM instance;

        #region Constructor/Instance
        public static QueryDisplayVM Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new QueryDisplayVM();
                    }
                }
                return instance;
            }
        }

        public QueryDisplayVM()
        {
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

        private DataView queryDisplayItemsSource;

        public DataView QueryDisplayItemsSource
        {
            get
            {
                return queryDisplayItemsSource;
            }
            set
            {
                if (queryDisplayItemsSource != value)
                {
                    queryDisplayItemsSource = value;
                    NotifyPropertyChanged("QueryDisplayItemsSource");
                }
            }
        }

        private Visibility queryDisplayVisible = Visibility.Collapsed;
        public Visibility QueryDisplayVisible
        {
            get
            {
                return queryDisplayVisible;
            }
            set
            {
                if (queryDisplayVisible != value)
                {
                    // toggle highlight of menu item based on value
                    queryDisplayVisible = value;
                    NotifyPropertyChanged("QueryDisplayVisible");
                }
            }
        }

        #endregion

    }
}
