using System;

namespace MyASAP.Position
{
    public class SimplePoint
    {
        private double m_x;
        private double m_y;

        public SimplePoint(double x, double y)
        {
            m_x = x;
            m_y = y;
        }

        public SimplePoint(SimplePoint otherPoint)
        {
            m_x = otherPoint.X;
            m_y = otherPoint.Y;
        }

        public SimplePoint()
        {
            m_x = 0;
            m_y = 0;
        }

        public double X
        {
            set { m_x = value; }
            get { return m_x; }
        }

        public double Y
        {
            set { m_y = value; }
            get { return m_y; }
        }

        public double Distance(SimplePoint simplePoint)
        {
            return Math.Sqrt(Math.Pow(m_x - simplePoint.m_x, 2) + Math.Pow(m_y - simplePoint.m_y, 2));
        }

        public bool IsTheSame(SimplePoint otherPoint, double deltaPixels)
        {
            return (Distance(otherPoint) < deltaPixels);
        }
    }
}