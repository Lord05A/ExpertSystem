using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Figure
{
    List<KeyValuePair<string, Point>> range;
    public List<Trapeze> trapezes =
        new List<Trapeze>();

    public Figure(Dictionary<string, Point> range, double max_sens_length)
    {
        KeyValuePair<string, Point> f = range.First<KeyValuePair<string, Point>>();
        this.range = new List<KeyValuePair<string,Point>>();
        this.range.Add(new KeyValuePair<string,Point>("tempZero",new Point(f.Value.x, f.Value.x)));
        foreach(var k in range)
        {
            this.range.Add(new KeyValuePair<string,Point>(k.Key,k.Value));
        }
        int ind = range.Count;
        this.range.Add(new KeyValuePair<string, Point>("tempOne", new Point(max_sens_length, max_sens_length)));
        for(int i = 1; i <= ind; ++i)
        {
            Point p1 = new Point(this.range[i - 1].Value.y, 0);
            Point p2 = new Point(this.range[i].Value.x, 1);
            Point p3 = new Point(this.range[i].Value.y, 1);
            Point p4 = new Point(this.range[i + 1].Value.x, 0);
            Trapeze trap = new Trapeze(p1, p2, p3, p4);
            trapezes.Add(trap);
        }
        this.range.Clear();
        foreach (var k in range)
        {
            this.range.Add(new KeyValuePair<string, Point>(k.Key, k.Value));
        }
    }

    public List<int> getTrapezes(double p)
    {
        List<int> res = new List<int>();
        for (int i = 0; i < trapezes.Count; ++i)
        {
            if (p >= trapezes[i].adj_boudary_left &&
            p <= trapezes[i].adj_boudary_right)
                res.Add(i);
        }
        return res;
    }


    public List<Point> getPointFigure(List<int> traps1, double y1,
        List<int> traps2, double y2)
    {
        List<Point> res = new List<Point>();
        for (int i = 0; i < traps1.Count; ++i)
        {
            List<Point> temp = getPartOfFigure(y1, traps1[i]);
            for(int j =0; j < temp.Count; ++j)
            {
                res.Add(temp[j]);
            }
        }
        for (int i = 0; i < traps2.Count; ++i)
        {
            List<Point> temp = getPartOfFigure(y2, traps2[i]);
            for (int j = 0; j < temp.Count; ++j)
            {
                res.Add(temp[j]);
            }
        }
        return res;
    }

    public List<Point> getPointFigure(List<List<int>> traps, List<double> y)
    {
        List<Point> res = new List<Point>();
        for(int i = 0; i < traps.Count; ++i)
        {
            for(int j = 0; j < traps[i].Count; ++j)
            {
                List<Point> temp = getPartOfFigure(y[i], traps[i][j]);
                for(int k = 0; k < temp.Count; ++k)
                {
                    res.Add(temp[k]);
                }
            }
        }

        /*
        List<Point> res = new List<Point>();
        for (int i = 0; i < traps1.Count; ++i)
        {
            List<Point> temp = getPartOfFigure(y1, traps1[i]);
            for(int j =0; j < temp.Count; ++j)
            {
                res.Add(temp[j]);
            }
        }
        for (int i = 0; i < traps2.Count; ++i)
        {
            List<Point> temp = getPartOfFigure(y2, traps2[i]);
            for (int j = 0; j < temp.Count; ++j)
            {
                res.Add(temp[j]);
            }
        }*/
        return res;
    }

    public List<Point> getPointFigure(List<int> traps1, double y1)
    {
        List<Point> res = new List<Point>();
        for (int i = 0; i < traps1.Count; ++i)
        {
            List<Point> temp = getPartOfFigure(y1, traps1[i]);
            for (int j = 0; j < temp.Count; ++j)
            {
                res.Add(temp[j]);
            }
        }
        return res;
    }
    private List<Point> getPartOfFigure(double y, int n)
    {
        Trapeze tr = trapezes[n];
        Line line = new Line(new Point(0, y), new Point(100, y));
        List<Point> res = tr.getIntersection(line);
        res.Add(new Point(tr.adj_boudary_left, 0));
        res.Add(new Point(tr.adj_boudary_right, 0));
        return res;
    }

}

