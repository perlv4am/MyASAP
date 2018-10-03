using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyASAP.PreProcessing
{
    public class TrackingSpeed
    {
        private static double s_skySpeed = 15.041;        // arcsec per sec

        private double m_raArcsecPerSec;
        private double m_decArcsecPerSec;

        public TrackingSpeed()
        {
            m_raArcsecPerSec = 0;
            m_decArcsecPerSec = 0;
        }

        public void SetSpeedForFixedTracking()
        {
            m_raArcsecPerSec = s_skySpeed;
            m_decArcsecPerSec = 0;
        }

        public double RaArcsecPerSec
        {
            get { return m_raArcsecPerSec; }
            set { m_raArcsecPerSec = value; }
        }

        public double DecArcsecPerSec
        {
            get { return m_decArcsecPerSec; }
            set { m_decArcsecPerSec = value; }
        }
    }
}
