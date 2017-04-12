using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class Point
    {
        public double x, y;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public bool isNan()
        {
            bool f = Double.NaN.CompareTo(x) == 0;
            bool f2 = Double.NaN.CompareTo(y) == 0;
            return f || f2;
        }
        public bool isNear(Point dist, double eps)
        {
            return Math.Abs(dist.x - x) < eps && Math.Abs(dist.y - y) < eps;
        }
    }
