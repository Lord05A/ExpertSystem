using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Line
{
    public double A, B, C;
    public Line(Point fst, Point snd)
    {
        A = snd.y - fst.y;
        B = fst.x - snd.x;
        C = fst.x * (fst.y - snd.y) + (snd.x - fst.x) * fst.y;
    }
    public static Point getCross(Line line1, Line line2)
    {
        Point point = new Point(System.Double.NaN, System.Double.NaN);
        double det = line1.A * line2.B - line1.B * line2.A;
        if (Math.Abs(det) < 1e-9)
            return point;
        point.x = -1 * (line1.C * line2.B - line1.B * line2.C) / det;
        point.y = -1 * (line1.A * line2.C - line1.C * line2.A) / det;
        return point;
    }
    public double getOrdinate(double x)
    {
        return (-1) * (A * x + C) / B;
    }
}

