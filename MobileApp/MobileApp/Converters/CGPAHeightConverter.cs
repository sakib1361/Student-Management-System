using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileApp.Converters
{
    public class CGPAHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal data && data > 2)
            {
                return (data - 2) * 40;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
