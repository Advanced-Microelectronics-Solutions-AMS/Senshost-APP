using System;
using System.Globalization;
using Senshost.Models.Account;

namespace Senshost.Converters
{	
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime dateTime)
            {

                //DateTime startTimeFormate = x.Startdate; // This  is utc date time
                TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, systemTimeZone);

                return localDateTime.ToString("dd-MMM-yyyy h:mm tt");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}

