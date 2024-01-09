using System.Globalization;

namespace Senshost_APP.Converters
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

                if(parameter?.ToString() == "true")
                    return localDateTime.ToString("dd-MMM-yyyy h:mm tt");

                if (localDateTime.Date == DateTime.Now.Date)
                    return localDateTime.ToString("h:mm tt");
                else if (localDateTime.Date == DateTime.Now.AddDays(-1).Date)
                    return "Yesterday";
                else
                    return localDateTime.ToString("dd-MMM-yyyy");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}

