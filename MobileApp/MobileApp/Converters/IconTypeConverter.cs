using Mobile.Core.Models.Core;
using MobileApp.Controls;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileApp.Converters
{
    public class IconTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IconType iconType)
            {
                return IconFont.GetSource(iconType, Color.Black);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
