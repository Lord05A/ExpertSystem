using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class Trapeze
    {
        private Line line1, line2;
        private double true_boundary_left, true_boundary_right;
        public double adj_boudary_left, adj_boudary_right;
        // точки трапеции задаются слева - направо
        public Trapeze(Point point1, Point point2, Point point3, Point point4)
        {
            line1 = new Line(point1, point2);
            line2 = new Line(point3, point4);
            true_boundary_left = point2.x;
            true_boundary_right = point3.x;
            adj_boudary_left = point1.x;
            adj_boudary_right = point4.x;
        }
        // возвращает значение принадлежности
        public double getOrdinate(double x)
        {
            if (x >= true_boundary_left && x <= true_boundary_right)
                return 1.0;
            return line2.getOrdinate(x) <= 1 ? line2.getOrdinate(x) : line1.getOrdinate(x);
        }

        ///////////////////
        public List<Point> getIntersection(Line line)
        {
            List<Point> res = new List<Point>();
            Point cross1 = Line.getCross(line, line1), cross2 = Line.getCross(line, line2);
            if (!cross1.isNan())
                res.Add(cross1);
            if (!cross2.isNan())
                res.Add(cross2);
            return res;
        }

    }
