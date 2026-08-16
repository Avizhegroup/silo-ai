using System.Globalization;

public static class DateTimeTools
{
    public static string ToNormalPersianDate(this string unixDate)
    => unixDate.HasValue()
        ? $"{unixDate.Substring(0,4)}/{unixDate.Substring(4, 2)}/{unixDate.Substring(6, 2)}"
        : "";

    public static string ToNormalPersianDateTime(this string unixDate)
    {
        try
        {
            return unixDate.HasValue()
           ? $"{unixDate.Substring(0, 4)}/{unixDate.Substring(4, 2)}/{unixDate.Substring(6, 2)} {unixDate.Substring(8, 2)}:{unixDate.Substring(10, 2)}"
           : "";
        }
        catch (Exception)
        {
            return "";
        }
    }

    public static string GetUnixPersianTime(DateTime? dateTime = null)
    {
        if (dateTime is null)
            dateTime = DateTime.Now;

        var pc = new PersianCalendar();

        return pc.GetYear(dateTime.Value) +
               pc.GetMonth(dateTime.Value).ToString().PadLeft(2, '0') +
               pc.GetDayOfMonth(dateTime.Value).ToString().PadLeft(2, '0') +
               dateTime.Value.Hour.ToString().PadLeft(2, '0') +
               dateTime.Value.Minute.ToString().PadLeft(2, '0') +
               dateTime.Value.Second.ToString().PadLeft(2, '0'); //+
                                                               //                   DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
    }

    public static string ConvertMiladi2ShamsiDate(DateTime date)
    {
        try
        {
            if (date.ToString(CultureInfo.InvariantCulture) != "01/01/0001 12:00:00 AM" &&
                date.ToString(CultureInfo.InvariantCulture) != "01/01/0001 12:00:00 ق.ظ" && date.ToString() != "01/01/0001 00:00:00")
            {
                var shamsi = new PersianCalendar();
                var ysh = shamsi.GetYear(date);
                var msh = shamsi.GetMonth(date);
                var dsh = shamsi.GetDayOfMonth(date);
                return $"{ysh}/{msh.ToString().PadLeft(2, '0')}/{dsh.ToString().PadLeft(2, '0')}";
            }
            else
            {
                var shamsi = new PersianCalendar();
                var ysh = shamsi.GetYear(DateTime.Now);
                var msh = shamsi.GetMonth(DateTime.Now);
                var dsh = shamsi.GetDayOfMonth(DateTime.Now);
                //return $"{ysh}/{msh.ToString().PadLeft(2, '0')}/{dsh.ToString().PadLeft(2, '0')}";

                return "";
            }


        }
        catch
        {
            return "";
        }
    }

    public static string GetTime()
       => DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0');

    public static string GetPersianDate()
    {
        var pc = new PersianCalendar();
        return pc.GetYear(DateTime.Now) + "/" +
               pc.GetMonth(DateTime.Now).ToString().PadLeft(2, '0') + "/" +
               pc.GetDayOfMonth(DateTime.Now).ToString().PadLeft(2, '0');
    }

    public static string ConvertFormatTimeTo24H(string Time)
    {

        if (Time.Contains("AM") || Time.Contains("PM") || Time.Contains("ق") || Time.Contains("ب"))
        {
            string[] Temp = Time.Split(' ');
            if (Temp[1].Contains("P") || Temp[1].Contains("ب"))
            {
                string[] HourMinTemp = Temp[0].Split(':');
                return ((HourMinTemp[0] != "12") ? Convert.ToInt32(HourMinTemp[0]) + 12 : Convert.ToInt32(HourMinTemp[0])) + ":" + HourMinTemp[1].PadLeft(2, '0');
            }
            else
            {
                string[] HourMinTemp = Temp[0].Split(':');
                return ((HourMinTemp[0] != "12") ? HourMinTemp[0].PadLeft(2, '0') : "00") + ":" + HourMinTemp[1].PadLeft(2, '0');

            }
        }
        else
        {
            if (Time.Length >= 5)
            {
                return Time.Substring(0, 5);
            }
        }

        return "00:00";
    }

    /// <summary>
    /// Get Unix DateTime in string format for gregorain date
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static string GetUnixDateTime(DateTime datetime)
    {
        long unixTimestamp = ((DateTimeOffset)datetime).ToUnixTimeSeconds();

        return unixTimestamp.ToString();
    }
}
