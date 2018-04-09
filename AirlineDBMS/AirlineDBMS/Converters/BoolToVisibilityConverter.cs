using System;
using System.Windows;
using System.Windows.Data;


namespace AirlineDBMS.Converters
{
   #region BoolToVisibilityConverter
   /// <summary>
   /// Converts True to Visible, False to Collapsed.
   /// </summary>
   [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Convert from bool to visibility value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            else if ((bool)value != false)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        /// <summary>
        /// ConvertBack from visibility to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ( value != null && value is Visibility)
                return (Visibility)value == Visibility.Visible;
            else
              return false;
        }

        #endregion
    }
    #endregion

    #region BoolToVisibilityReverseConverter
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityReverseConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Convert from bool to visibiilty value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			if (value == null)
				return Visibility.Collapsed;
         if ((bool)value != false)
         {
               return Visibility.Collapsed;
         }
         else
               return Visibility.Visible;
        }

        /// <summary>
        /// ConvertBack from visibility to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility vis = (Visibility)value;
            return vis == Visibility.Visible;
        }

        #endregion
    }
    #endregion
    
    #region CountToVisibilityConverter
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class CountToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        /// <summary>
        /// Convert from bool to visibiilty value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (int)value != 0)
            {
                return Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }

        /// <summary>
        /// ConvertBack from visibility to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    #endregion
}


