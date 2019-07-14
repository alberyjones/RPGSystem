using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static RPGSystem.Utils;

namespace RPGApp.Converter
{
    public class HeightDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intVal)
            {
                DivideWithRemainder(intVal, 12, out var feet, out var inches);
                if (feet == 0)
                {
                    return $"{inches}\"";
                }
                else if (inches == 0)
                {
                    return $"{feet}\'";
                }
                return $"{feet}\' {inches}\"";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
