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
    public class HeightDisplayConverter : IValueConverter
    {
        private static Regex feetRegex = new Regex("([0-9]+)\'");
        private static Regex inchesRegex = new Regex("([0-9]+)\"");

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
            int result = 0;
            if (value is string strVal && !String.IsNullOrEmpty(strVal))
            {
                var match = feetRegex.Match(strVal);
                if (match.Success && Int32.TryParse(match.Groups[1].Value, out var feet))
                {
                    result = feet * 12;
                }
                match = inchesRegex.Match(strVal);
                if (match.Success && Int32.TryParse(match.Groups[1].Value, out var inches))
                {
                    result += inches;
                }
            }
            return result;
        }
    }
}
