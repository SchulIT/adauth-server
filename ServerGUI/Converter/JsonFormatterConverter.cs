using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ServerGUI.Converter
{
    public class JsonFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(value.ToString());
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
