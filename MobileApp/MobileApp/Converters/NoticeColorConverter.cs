using CoreEngine.Model.DBModel;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileApp.Converters
{
    public class NoticeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PostType postType)
            {
                switch (postType)
                {
                    case PostType.ClassCancel:
                        return Color.DarkCyan;
                    case PostType.Examination:
                        return Color.Maroon;
                    case PostType.Notice:
                        return Color.Magenta;
                    default:
                        return Color.DarkOrange;
                }
            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
