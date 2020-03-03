using Jdenticon;
using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace MobileApp.Converters
{
    public class IdenticonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string information)
            {
                var data = information.ToLower().Replace(" ", "");
                var imgFile = Path.Combine(Path.GetTempPath(), data + ".png");
                if (File.Exists(imgFile))
                {
                    return imgFile;
                }
                else
                {
                    Identicon.FromValue(data, size: 160)
                             .SaveAsPng(imgFile);
                    return imgFile;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
