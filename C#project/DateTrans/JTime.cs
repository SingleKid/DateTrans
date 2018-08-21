using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTrans
{
    public class JTime
    {
       public static bool isBigMonth(int month)
        {
            int[] bigMonth = { 1, 3, 5, 7, 8, 10, 12 };
            for (int i = 0; i < 7; i++)
            {
                if (bigMonth[i] == month) return true;
            }
            return false;
        }
        public static int date_amount_of_month(int year, int month)
        {
            if (month != 2)
            {
                return isBigMonth(month) ? 31 : 30;
            }
            else
            {
                if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
                {
                    return 29;
                }
                else return 28;
            }
        }
    }
    class utc
    {
        public int year;
        public int month;
        public int date;
        public int hour;
        public int minute;
        public double sec;
        public utc(int y, int m, int d, int h, int min, double s)
        {
            year = y;
            month = m;
            date = d;
            hour = h;
            minute = min;
            sec = s;
        }
        public void change_to_doy(int doy)
        {
            int temp = doy;
            for (int i = 1; i <= 12; i++)
            {
                temp = JTime.date_amount_of_month(year, i);
                if (doy <= temp)
                {
                    month = i;
                    date = doy;
                    return;
                }
                else
                {
                    doy -= temp;
                }
            }
            throw new Exception();
        }
        public utc offset_hour(int h)
        {
            utc total = new utc(year, month, date, hour, minute, sec);
            if(h > 0)
            {
                if (hour + h < 24) total.hour += h;
                else if (date < JTime.date_amount_of_month(total.year, total.month))
                {
                    total.date += 1;
                    total.hour = total.hour - 24 + h;
                }
                else if (month <= 11)
                {
                    total.month += 1;
                    total.date = 1;
                    total.hour = total.hour - 24 + h;
                }
                else
                {
                    total.year += 1;
                    total.month = 1;
                    total.date = 1;
                    total.hour = total.hour - 24 + h;
                }
                return total;
            }
            else if (h < 0)
            {
                h = -h;
                if (hour >= h) total.hour -= h;
                else if (date > 1)
                {
                    total.date -= 1;
                    total.hour = total.hour + 24 - h;
                }
                else if (month > 1)
                {
                    total.month -= 1;
                    total.date = total.date - 1 + JTime.date_amount_of_month(total.year, total.month);
                    total.hour = total.hour + 24 - h;
                }
                else
                {
                    total.year -= 1;
                    total.month += 11;
                    total.date = total.date - 1 + JTime.date_amount_of_month(total.year, total.month);
                    total.hour = total.hour + 24 - h;
                }
                return total;
            }
            return total;
        }
        public utc(MJDTime time)
        {
            double rest = time.frac_day * 24;
            hour = (int)rest;
            rest = (rest - hour) * 60;
            minute = (int)rest;
            rest = (rest - minute) * 60;
            sec = Math.Round(rest);

            year = (int)((time.days - 15078.2) / 365.25);
            month = (int)((time.days - 14956.1 - (int)(year * 365.25)) / 30.6001);
            date = time.days - 14956 - (int)(year * 365.25) - (int)(month * 30.6001);
            if (year > 100) year -= 100;
            month = month - 1;
        }
        bool larger_than(utc u1)
        {
            if (year == u1.year)
                if (month == u1.month)
                    if (date == u1.date)
                        if (hour == u1.hour)
                            if (minute == u1.minute)
                                if (sec == u1.sec)
                                    return false;
                                else if (sec > u1.sec) return true;
                                else return false;
                            else if (minute > u1.minute) return true;
                            else return false;
                        else if (hour > u1.hour) return true;
                        else return false;
                    else if (date > u1.date) return true;
                    else return false;
                else if (month > u1.month) return true;
                else return false;
            else if (year > u1.year) return true;
            else return false;
        }
        
    };
    class MJDTime
    {
        public int days;
        public double frac_day;
        public MJDTime(int d, double f)
        {
            days = d;
            frac_day = f;
        }
        public MJDTime(utc time)
        {
            int y, m, temp;
            y = time.year + (time.year < 80 ? 2000 : 1900);
            m = time.month;
            if (m <= 2)
            {
                y--;
                m += 12;
            }
            temp = (int)(365.25 * y);
            temp += (int)(30.6001 * (m + 1));
            temp += time.date;
            temp -= 679019;

            days = temp;
            frac_day = time.hour + time.minute / 60.0 + time.sec / 3600.0;
            frac_day /= 24.0;
        }
        public MJDTime(GPSTime gps)
        {
            double sum = (gps.sec / 86400.0) + 44244 + 7 * gps.week;
            days = (int)sum;
            frac_day = sum - days;
        }
    };
    class GPSTime
    {
        public int week;
        public int sec;
        public GPSTime(int w, double s)
        {
            week = w;
            sec = (int)s;
        }
        public GPSTime(MJDTime time)
        {
            week = (int)((time.days - 44244) / 7);
            int remain = time.days - week * 7 - 44244;
            sec = (int)((remain + time.frac_day) * 86400.0);
        }
        public GPSTime(utc time)
        {
            MJDTime m_time = new MJDTime(time);
            week = (int)((m_time.days - 44244) / 7.0);
            int remain = m_time.days - week * 7 - 44244;
            sec = (int)((remain + m_time.frac_day) * 86400.0);
        }
        GPSTime()
        {

        }
    };
}
