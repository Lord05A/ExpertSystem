using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class WeightCenter
    {
        public Point calc(List<Point> points)
        {
            double answer_x = 0.0, answer_y = 0.0;
            for (int i = 0; i < points.Count; i++)
            {
                answer_x += points[i].x;
                answer_y += points[i].y;
            }
            return new Point(answer_x / points.Count, answer_y / points.Count);
        }
    }
