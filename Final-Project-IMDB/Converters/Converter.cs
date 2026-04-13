using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Final_Project_IMDB.Converters
{
    public class RoleFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";

            var str = value.ToString();
            if (string.IsNullOrWhiteSpace(str)) return "";

            return string.Join(", ",
                str.Split(',')
                   .Select(x => x.Trim())
                   .Where(x => x.Length > 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}