using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using static RPGSystem.Utils;

namespace RPGApp.Converter
{
    public class WeightDisplayConverter : IValueConverter
    {
        private static Regex stoneRegex = new Regex("([0-9]+)st");
        private static Regex lbRegex = new Regex("([0-9]+)lb");

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
            int result = 0;
            if (value is string strVal && !String.IsNullOrEmpty(strVal))
            {
                var match = stoneRegex.Match(strVal);
                if (match.Success && Int32.TryParse(match.Groups[1].Value, out var stone))
                {
                    result = stone * 14;
                }
                match = lbRegex.Match(strVal);
                if (match.Success && Int32.TryParse(match.Groups[1].Value, out var lb))
                {
                    result += lb;
                }
            }
            return result;
        }
    }
}
