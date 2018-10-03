using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyASAP.Utils
{
    public class DegreeUtils
    {
        public const double DToR = Math.PI / 180.0;
        public const double RToD = 180.0 / Math.PI;
        public const double SToR = (DToR / 3600.0);
        public const double HALF_PI = Math.PI / 2.0;
        public const double TWOPI = Math.PI * 2.0;

        public static string FormatRA(double ra, bool showSeconds, bool showFraction, bool showMilliseconds)
        {
            double hours, minutes, seconds;
            /*  if (showMilliseconds)
              {  // 12.10.03
                 ra += (0.00005 * 15.0 / 3600.0) * DToR;  // 0.00005 sec
              }
              if (showFraction)
              {  // 14.11.01
                 ra += (0.005 * 15.0 / 3600.0) * DToR;  // 0.005 sec
              }
              else if (showSeconds)
              {
                 ra += (0.5 * 15.0 / 3600.0) * DToR;  // 0.5 sec
              }
              else
              {
                 ra += (0.5 * 15.0 / 60.0) * DToR;    // 0.5 min
              }*/

            hours = ra * RToD / 15.0; // + 1e-10;
            int h = (int)Math.Floor(hours);

            var sb = new StringBuilder();

            if (showSeconds)
            {
                minutes = (hours - h) * 60.0;
                int m = (int)Math.Floor(minutes);

                seconds = (minutes - m) * 60.0;

                // 14.11.01 prevent "24:00:00"
                if (h == 24)
                {
                    h = 0;
                }

                if (showMilliseconds)
                {
                    sb.AppendFormat("{0:00}:{1:00}:{2:00.0000}", h, m, seconds);
                }
                else if (showFraction)
                {
                    sb.AppendFormat("{0:00}:{1:00}:{2:00.00}", h, m, seconds);
                }
                else
                {
                    int s = (int)Math.Floor(seconds);
                    sb.AppendFormat("{0:00}:{1:00}:{2:00}", h, m, s);
                }
            }
            else
            {
                minutes = (hours - h) * 60.0;
                int m = (int)Math.Floor(minutes);

                // 14.11.01 prevent "24:00:00"
                if (h == 24)
                {
                    h = 0;
                }

                sb.AppendFormat("{0:00}:{1:00}", h, m);
            }
            return sb.ToString();
        }

        public static string FormatDec(double dec, bool showSeconds, bool showMinutes, bool showFraction, bool showMas)
        {
            double degrees = Math.Abs(dec * RToD) + 1e-10;

            int g = (int)Math.Floor(degrees);

            var sb = new StringBuilder();

            if (!showMinutes)
            {
                if (dec < 0.0)
                {
                    sb.AppendFormat("-{0:00}°", g);
                }
                else
                {
                    sb.AppendFormat("+{0:00}°", g);
                }
                return sb.ToString();
            }

            double minutes = (degrees - g) * 60.0;
            int m = (int)Math.Floor(minutes);

            if (showSeconds)
            {
                double seconds = (minutes - m) * 60.0;
                int s = (int)Math.Floor(seconds);

                if (dec < 0.0)
                {
                    if (showMas)
                    {
                        sb.AppendFormat("-{0:00}°{1:00}'{2:00.000}\"", g, m, seconds);
                    }
                    else if (showFraction)
                    {
                        sb.AppendFormat("-{0:00}°{1:00}'{2:00.0}\"", g, m, seconds);
                    }
                    else
                    {
                        sb.AppendFormat("-{0:00}°{1:00}'{2:00}\"", g, m, s);
                    }
                }
                else
                {
                    if (showMas)
                    {
                        sb.AppendFormat("+{0:00}°{1:00}'{2:00.000}\"", g, m, seconds);
                    }
                    else if (showFraction)
                    {
                        sb.AppendFormat("+{0:00}°{1:00}'{2:00.0}\"", g, m, seconds);
                    }
                    else
                    {
                        sb.AppendFormat("+{0:00}°{1:00}'{2:00}\"", g, m, s);
                    }
                }
            }
            else
            {
                if (dec < 0.0)
                {
                    sb.AppendFormat("-{0:00}°{1:00}'", g, m);
                }
                else
                {
                    sb.AppendFormat("+{0:00}°{1:00}'", g, m);
                }
            }
            return sb.ToString();
        }

        public static void RaToHms(double ra, out int hours, out int minutes, out double seconds)
        {
            double dHours = ra * RToD / 15.0;
            hours = (int)Math.Floor(dHours);

            double dMinutes = (dHours - hours) * 60.0;
            minutes = (int)Math.Floor(dMinutes);

            seconds = (dMinutes - minutes) * 60.0;

            if (hours == 24) hours = 0;
        }

        public static void DecToDms(double dec, out int degrees, out int minutes, out double seconds)
        {
            dec = Math.Abs(dec);

            double dDegrees = dec * RToD;
            degrees = (int)Math.Floor(dDegrees);

            double dMinutes = (dDegrees - degrees) * 60.0;
            minutes = (int)Math.Floor(dMinutes);

            seconds = (dMinutes - minutes) * 60.0;

            if (degrees == 24) degrees = 0;
        }

        public static void NormalizeAngleFrom0To2Pi(ref double angle)
        {
            while (angle < 0) angle += TWOPI;
            while (angle >= TWOPI) angle -= TWOPI;
        }

        public static void NormalizeAngleFromMinusPiToPi(ref double angle)
        {
            while (angle < -Math.PI) angle += TWOPI;
            while (angle >= Math.PI) angle -= TWOPI;
        }

        public static void NormalizeRaHours(ref double raHours)
        {
            while (raHours < 0)
                raHours += 24;
            while (raHours >= 24)
                raHours -= 24;
        }

        public static void NormalizeDecDegrees(ref double decDeg)
        {
            if (decDeg < -90)
                decDeg = -90;
            if (decDeg > 90)
                decDeg = 90;
        }

        public static double GetDifferenceInRaHours(double raHours1, double raHours2)
        {
            double raDiffHours = raHours1 - raHours2;
            while (raDiffHours < -12)
                raDiffHours += 24;
            while (raDiffHours > 12)
                raDiffHours -= 24;
            return raDiffHours;
        }

        public static double GetPositiveDifferenceInRaHours(double raHours1, double raHours2)
        {
            double raDiffHours = GetDifferenceInRaHours(raHours1, raHours2);
            DegreeUtils.NormalizeRaHours(ref raDiffHours);
            return raDiffHours;
        }
    }
}
