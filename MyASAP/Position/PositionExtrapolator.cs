using MyASAP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyASAP.Position
{
    class PositionExtrapolator
    {
        private SkyPosition m_initialPosition;
        private PositionAngle m_positionAngle;
        private double m_distanceInDegrees;
        private double m_cosDec;

        public PositionExtrapolator(SkyPosition initialPosition)
        {
            m_initialPosition = initialPosition;
            m_cosDec = Math.Cos(initialPosition.Dec);
        }

        public SkyPosition GetTargetPosition(PositionAngle positionAngle, double distanceInDegrees)
        {
            m_positionAngle = positionAngle;
            m_distanceInDegrees = distanceInDegrees;
            if ((distanceInDegrees < 0.1) && (m_cosDec > 0.1))
            {
                // for small distances and declination below 84, use the simple method
                // usable for most of the asteroids
                return _GetSimpleTargetPosition();
            }
            else
            {
                return _GetPreciseTargetPosition();
            }
        }

        private SkyPosition _GetSimpleTargetPosition()
        {
            SkyPosition targetPoint = new SkyPosition(m_initialPosition);
            targetPoint.RaDeg += m_distanceInDegrees * m_positionAngle.SinPa / m_cosDec;
            targetPoint.DecDeg += m_distanceInDegrees * m_positionAngle.CosPa;
            targetPoint.CorrectCoordinates();
            return targetPoint;
        }

        private SkyPosition _GetPreciseTargetPosition()
        {
            SkyPosition targetPoint = new SkyPosition(m_initialPosition);
            double sinDec = Math.Sin(m_initialPosition.Dec);
            double distanceInRad = m_distanceInDegrees * DegreeUtils.DToR;
            double sinDistance = Math.Sin(distanceInRad);
            double cosDistance = Math.Cos(distanceInRad);
            double sinDistanceCosPa = sinDistance * m_positionAngle.CosPa;

            double x = m_cosDec * cosDistance - sinDec * sinDistanceCosPa;
            double y = sinDistance * m_positionAngle.SinPa;
            double z = sinDec * cosDistance + m_cosDec * sinDistanceCosPa;

            if (x != 0 || y != 0)
            {
                targetPoint.Ra += Math.Atan2(y, x);
            }
            if (Math.Abs(z) < 0.7)
            {
                targetPoint.Dec = Math.Asin(z);
            }
            else
            {
                targetPoint.Dec = Math.Acos(Math.Sqrt(x * x + y * y));
                if (z < 0)
                {
                    targetPoint.Dec = -targetPoint.Dec;
                }
            }
            targetPoint.CorrectCoordinates();
            return targetPoint;
        }
    }
}
