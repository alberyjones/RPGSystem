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
    public class WeightDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intVal)
            {
                DivideWithRemainder(intVal, 14, out var stone, out var lb);
                if (stone == 0)
                {
                    return $"{lb}lb";
                }
                else if(lb == 0)
                {
                    return $"{stone}st";
                }
                return $"{stone}st {lb}lb";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
