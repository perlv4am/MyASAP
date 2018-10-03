using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyASAP.Utils;

namespace MyASAP.Position
{
    public class SkyPosition
    {
        private double m_raHours;
        private double m_decDeg;

        public SkyPosition()
        {
            m_raHours = 0;
            m_decDeg = 0;
        }

        public SkyPosition(SkyPosition otherSkyPosition)
        {
            m_raHours = otherSkyPosition.m_raHours;
            m_decDeg = otherSkyPosition.m_decDeg;
        }

        public double RaHours
        {
            get { return m_raHours; }
            set { m_raHours = value; }
        }

        public double DecDeg
        {
            get { return m_decDeg; }
            set { m_decDeg = value; }
        }

        public double Ra
        {
            get { return (m_raHours * 15.0 * DegreeUtils.DToR); }
            set { m_raHours = (value * DegreeUtils.RToD / 15.0); }
        }

        public double Dec
        {
            get { return (m_decDeg * DegreeUtils.DToR); }
            set { m_decDeg = (value * DegreeUtils.RToD); }
        }

        public double RaDeg
        {
            get { return (m_raHours * 15.0); }
            set { m_raHours = (value / 15.0); }
        }

        public double DistanceInDegrees(SkyPosition skyPosition)
        {
            double distance = Distance(Ra, Dec, skyPosition.Ra, skyPosition.Dec);
            return distance * DegreeUtils.RToD;
        }

        private static double Distance(double ra1, double dec1, double ra2, double dec2)
        {
            double cosRa1 = Math.Cos(ra1);
            double cosRa2 = Math.Cos(ra2);
            double sinRa1 = Math.Sin(ra1);
            double sinRa2 = Math.Sin(ra2);

            double cosDec1 = Math.Cos(dec1);
            double cosDec2 = Math.Cos(dec2);
            double sinDec1 = Math.Sin(dec1);
            double sinDec2 = Math.Sin(dec2);

            double x1 = cosDec1 * cosRa1;
            double x2 = cosDec2 * cosRa2;
            double y1 = cosDec1 * sinRa1;
            double y2 = cosDec2 * sinRa2;
            double z1 = sinDec1;
            double z2 = sinDec2;

            double R = x1 * x2 + y1 * y2 + z1 * z2;

            if (R > 1.0) R = 1.0;
            if (R < -1.0) R = -1.0;

            double distance = Math.Acos(R);
            return distance;
        }

        public double GetPositiveDifferenceInRaHours(SkyPosition skyPosition)
        {
            return DegreeUtils.GetPositiveDifferenceInRaHours(m_raHours, skyPosition.m_raHours);
        }

        public double GetDifferenceInRaHours(SkyPosition skyPosition)
        {
            return DegreeUtils.GetDifferenceInRaHours(m_raHours, skyPosition.m_raHours);
        }

        public void CorrectCoordinates()
        {
            DegreeUtils.NormalizeRaHours(ref m_raHours);
            DegreeUtils.NormalizeDecDegrees(ref m_decDeg);
        }

        public SkyPosition GetTargetPosition(PositionAngle positionAngle, double distanceInDegrees)
        {
            PositionExtrapolator positionExtrapolator = new PositionExtrapolator(this);
            return positionExtrapolator.GetTargetPosition(positionAngle, distanceInDegrees);
        }
    }
}