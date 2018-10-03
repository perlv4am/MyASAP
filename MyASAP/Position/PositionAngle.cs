using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyASAP.PreProcessing;
using MyASAP.Utils;

namespace MyASAP.Position
{
    public class PositionAngle
    {
        private double m_positionAngleDeg;
        private double m_cosPa;
        private double m_sinPa;

        private double deltaRaArcSec;
        private double deltaDecArcSec;
        private double cosDec2;

        private SkyPosition m_position1;

        public PositionAngle(SkyPosition position1, SkyPosition position2)
        {
            _Initialize(position1, position2);
            _ComputePositionAngle();
        }

        public PositionAngle(SkyPosition position, TrackingSpeed trackingSpeed)
        {
            _Initialize(position, trackingSpeed);
            _ComputePositionAngle();
        }

        public PositionAngle(double positionAngleDeg)
        {
            _FillPositionAngleResults(positionAngleDeg * DegreeUtils.DToR);
        }

        private void _Initialize(SkyPosition position1, SkyPosition position2)
        {
            m_position1 = position1;
            deltaRaArcSec = DegreeUtils.GetDifferenceInRaHours(position2.RaHours, position1.RaHours) * 15 * 3600;
            deltaDecArcSec = (position2.DecDeg - position1.DecDeg) * 3600;
            cosDec2 = Math.Cos(position2.Dec);
        }

        private void _Initialize(SkyPosition position, TrackingSpeed trackingSpeed)
        {
            // 1 second sample tracking
            m_position1 = position;
            deltaRaArcSec = trackingSpeed.RaArcsecPerSec;
            deltaDecArcSec = trackingSpeed.DecArcsecPerSec;
            cosDec2 = Math.Cos(position.Dec);
        }

        private void _ComputePositionAngle()
        {
            if (cosDec2 < 0.1)
            {
                // Use the advanced method always around poles (> 84 degrees)
                _ComputeAdvancedPositionAngle();
            }
            else if ((Math.Abs(deltaRaArcSec) / cosDec2) > 1200)
            {
                // Use the advanced method for RA difference more than 20 arcmin (on equator)
                _ComputeAdvancedPositionAngle();
            }
            else
            {
                _ComputeFastPositionAngle();
            }
        }

        private void _ComputeFastPositionAngle()
        {
            double positionAngleRad = DegreeUtils.HALF_PI - Math.Atan2(deltaDecArcSec, deltaRaArcSec * cosDec2);
            _FillPositionAngleResults(positionAngleRad);
        }

        private void _ComputeAdvancedPositionAngle()
        {
            double deltaRaRad = DegreeUtils.DToR * deltaRaArcSec / 3600.0;
            double deltaDecRad = DegreeUtils.DToR * deltaDecArcSec / 3600.0;
            double cosDeltaRa = Math.Cos(deltaRaRad);
            double sinDeltaRa = Math.Sin(deltaRaRad);
            double sinDeltaDec = Math.Sin(deltaDecRad);
            double sinDec1 = Math.Sin(m_position1.Dec);

            double xA = sinDeltaRa * cosDec2;
            double yA = sinDeltaDec + (1 - cosDeltaRa) * sinDec1 * cosDec2;

            double positionAngleRad = 0;

            if ((xA != 0) || (yA != 0))
            {
                positionAngleRad = DegreeUtils.HALF_PI - Math.Atan2(yA, xA);
            }
            _FillPositionAngleResults(positionAngleRad);
        }

        private void _FillPositionAngleResults(double positionAngleRad)
        {
            DegreeUtils.NormalizeAngleFrom0To2Pi(ref positionAngleRad);
            m_positionAngleDeg = DegreeUtils.RToD * positionAngleRad;
            m_sinPa = Math.Sin(positionAngleRad);
            m_cosPa = Math.Cos(positionAngleRad);
        }

        public double PositionAngleDeg
        {
            get { return m_positionAngleDeg; }
        }

        public double SinPa
        {
            get { return m_sinPa; }
        }

        public double CosPa
        {
            get { return m_cosPa; }
        }

        public bool IsPositionAngleInTolerance(PositionAngle otherPositionAngle, double toleranceDeg)
        {
            double positionAngleDifference = AbsoluteDifferenceDeg(otherPositionAngle);
            return (positionAngleDifference < toleranceDeg);
        }

        /*
         * Returns the absolute difference of PA in interval from 0 to 180 degrees
         */
        public double AbsoluteDifferenceDeg(PositionAngle otherPositionAngle)
        {
            double differenceDeg = Math.Abs(m_positionAngleDeg - otherPositionAngle.m_positionAngleDeg);
            while (differenceDeg > 180)
            {
                differenceDeg = Math.Abs(differenceDeg - 360);
            }
            return differenceDeg;
        }

        public override string ToString()
        {
            return m_positionAngleDeg.ToString(CultureInfo.InvariantCulture);
        }
    }
}