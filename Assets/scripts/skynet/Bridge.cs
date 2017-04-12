using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Bridge
{
    Figure destanations, velocities, angles;
    Point target;
    Point current;
    float max_sensor_length, max_velocity = 150, width_Of_Car = 800;
    //куда смотритъ
    Point direction;
    //направление на цель
    Point target_direction;
    int flag = 0;
    private int findIndex(List<int> arr, int val)
    {
        for (int i = 0; i < arr.Count; i++)
            if (arr[i] == val)
                return i;
        return -1;
    }
    private int CalcSide()
    {
        Point a = new Point(0, 0);
        double d = (target_direction.x - a.x) * (direction.y - a.y)
        - (target_direction.y - a.y) * (direction.x - a.x);
        if (d < 0)
            return -1;
        if (d > 0)
            return 1;
        return 0;
    }
    private double calcVelocity(double dist)
    {
        List<int> indexes = destanations.getTrapezes(dist);
        List<double> precisions = new List<double>();
        for (int i = 0; i < indexes.Count; i++)
        {
            precisions.Add(destanations.trapezes[indexes[i]].getOrdinate(dist));
        }

        List<List<int>> cuttedTrapezes = new List<List<int>>(2);
        cuttedTrapezes.Add(new List<int>());
        cuttedTrapezes.Add(new List<int>());
        for (int i = 0; i < precisions.Count; ++i)
        {
            // блок, описывающий правила (пишутся на бумаге ручками)
            // switch - case ???
            // 0 - трапеция "очень маленького" растояния
            // 0 - трапеция "очень маленькой" скорости
            // правило: Если Расстояние "оч мал", то Скорость "оч мал"
            // и так далее
            if (indexes[i] == 0)
            {
                cuttedTrapezes[i].Add(0);
                cuttedTrapezes[i].Add(1);
            }
            if (indexes[i] == 1)
            {
                cuttedTrapezes[i].Add(1);
                cuttedTrapezes[i].Add(2);
                cuttedTrapezes[i].Add(3);
            }
            if (indexes[i] == 2)
            {
                cuttedTrapezes[i].Add(4);
            }
            //if (indexes[i] == 3)
            //{
            //    cuttedTrapezes[i].Add(3);
            //    cuttedTrapezes[i].Add(4);
            //}
            //if (indexes[i] == 4) cuttedTrapezes[i].Add(4);
        }

        if (precisions.Count == 1)
            return new WeightCenter().calc(velocities.getPointFigure(cuttedTrapezes[0], precisions[0])).x;
        else
            return new WeightCenter().calc(velocities.getPointFigure(
                cuttedTrapezes[0], precisions[0],
                cuttedTrapezes[1], precisions[1]
                )).x;
    }
    public double getAngle(Point p1, Point p2)
    {
        double numerator = p1.x * p2.x + p1.y * p2.y;
        double denumerator = Math.Sqrt(p1.x * p1.x + p1.y * p1.y) * Math.Sqrt(p2.x * p2.x + p2.y * p2.y);
        double answer =Math.Acos(numerator / denumerator) * 180 / Math.PI;
        return answer * CalcSide();// == 90 ? 85 : answer;
    }
    public Bridge(double x, double y, float max_sensor_length)
    {        
        Dictionary<string, Point> angle_graph = new Dictionary<string, Point>();
        angle_graph.Add("1", new Point(-15, -30));
        angle_graph.Add("2", new Point(-5, -10));
        angle_graph.Add("3", new Point(0, -3));
        angle_graph.Add("4", new Point(0, 3));
        angle_graph.Add("5", new Point(5,10));
        angle_graph.Add("6", new Point(15, 30));
        angles = new Figure(angle_graph, 30);
        target = new Point(x, y);
        changeMaximumSensorLength(max_sensor_length);
        changeMaxVelocity(max_velocity);
    }

    public void changeMaximumSensorLength(float new_maximum)
    {
        double xx = (int)(width_Of_Car / Math.Sqrt(2)) / 100;
        double yy = (int)(2 * width_Of_Car / Math.Sqrt(2)) / 100;
        double zz = (int)(3 * width_Of_Car / Math.Sqrt(2)) / 100;
        double ww = (yy + (zz - yy) / zz) / 100;
        xx = 0.1;
        yy = 0.25;
        ww = 0.5;
        zz = 0.75;
        max_sensor_length = new_maximum;
        // точка задает отрезок полной уверенности что параметр именно такой
        Dictionary<string, Point> dist_graph = new Dictionary<string, Point>();
        dist_graph.Add("1", new Point(0, xx * max_sensor_length));
        dist_graph.Add("2", new Point(yy * max_sensor_length, ww * max_sensor_length));
        dist_graph.Add("3", new Point(zz * max_sensor_length, max_sensor_length));
        // dist_graph.Add("4", new Point(0.6 * max_sensor_length, 0.7 * max_sensor_length));
        //dist_graph.Add("5", new Point(0.8 * max_sensor_length, max_sensor_length));
        destanations = new Figure(dist_graph, max_sensor_length);
    }

    public void changeMaxVelocity(float new_velocity)
    {
        max_velocity = new_velocity;
        Dictionary<string, Point> velocity_graph = new Dictionary<string, Point>();
        velocity_graph.Add("1", new Point(0, 0.1 * max_velocity));
        velocity_graph.Add("2", new Point(0.2 * max_velocity, 0.3 * max_velocity));
        velocity_graph.Add("3", new Point(0.4 * max_velocity, 0.6 * max_velocity));
        velocity_graph.Add("4", new Point(0.7 * max_velocity, 0.85 * max_velocity));
        velocity_graph.Add("5", new Point(0.9 * max_velocity, max_velocity));
        velocities = new Figure(velocity_graph, max_velocity);
    }

    public double getVelocity(double dist_sens_left, double dist_sens_front, double dist_sens_right)
    {
        if (current.isNear(target, 10))
            return 0;
        if (dist_sens_left != -1 && dist_sens_front == -1 && dist_sens_right == -1)
            return 0.75 * max_velocity;
        double dist = dist_sens_front == -1 ? max_sensor_length : dist_sens_front;
        double dist_l = dist_sens_left == -1 ? max_sensor_length : dist_sens_left;
        double dist_r = dist_sens_right == -1 ? max_sensor_length : dist_sens_right;
        return Math.Min(Math.Min(calcVelocity(dist), calcVelocity(dist_l)), calcVelocity(dist_r));
    }

    public double getAngleRight(double dist_sens_left, double dist_sens_front, double dist_sens_right)
    {
        if (dist_sens_left == -1 && dist_sens_front == -1 && dist_sens_right == -1)
        {
            flag = 0;
            //Debug.Log("С какой стороны цель: " + CalcSide());
            if (CalcSide() == 1)
                return getAngle(target_direction, direction) * 0.3;
            else return 2;
        }

        // если правый средний, остальные -1, то чуть вправо
        // если правый малый, остальные -1, то чуть влево
        if (dist_sens_left == -1 && dist_sens_front == -1 && dist_sens_right != -1)
        {
            List<int> ind = destanations.getTrapezes(dist_sens_right);
            List<double> prec = new List<double>();
            for (int i = 0; i < ind.Count; i++)
            {
                prec.Add(destanations.trapezes[ind[i]].getOrdinate(dist_sens_right));
            }
            List<List<int>> cutted = new List<List<int>>(2);
            cutted.Add(new List<int>());
            cutted.Add(new List<int>());
            for (int i = 0; i < prec.Count; ++i)
            {
                if (ind[i] == 1)
                {
                    //Debug.Log("Зеркальное Правило старого угла номер немного влево");
                    cutted[i].Add(3);
                }
                if (ind[i] == 0)
                {
                    //Debug.Log("Зеркальное Правило старого угла номер немного вправо");
                    cutted[i].Add(2);
                }
                if (ind[i] == 2)
                {
                    //Debug.Log("Зеркальное Правило старого угла номер немного вправо");
                    cutted[i].Add(4);
                }
                if (prec.Count == 0)
                    return 0;
                if (prec.Count == 1)
                    return new WeightCenter().calc(angles.getPointFigure(cutted[0], prec[0])).x;
                else
                    return new WeightCenter().calc(angles.getPointFigure(
                        cutted[0], prec[0],
                        cutted[1], prec[1]
                        )).x;
            }
        }
        // конец

        if (dist_sens_left != -1 && dist_sens_right != -1)
        {
            // маленький ли правый
            List<int> ind = destanations.getTrapezes(dist_sens_right);
            List<double> prec = new List<double>();
            for (int i = 0; i < ind.Count; i++)
            {
                prec.Add(destanations.trapezes[ind[i]].getOrdinate(dist_sens_right));
            }
            List<List<int>> cutted = new List<List<int>>(2);
            cutted.Add(new List<int>());
            cutted.Add(new List<int>());
            double p = -1;
            if (ind.Contains(0))
            {
                cutted[0].Add(2);
                p = prec[findIndex(ind, 0)];
            }
            // большой ли левый
            List<int> ind1 = destanations.getTrapezes(dist_sens_left);
            List<double> prec1 = new List<double>();
            for (int i = 0; i < ind1.Count; i++)
            {
                prec1.Add(destanations.trapezes[ind1[i]].getOrdinate(dist_sens_left));
            }
            List<List<int>> cutted1 = new List<List<int>>(2);
            cutted1.Add(new List<int>());
            cutted1.Add(new List<int>());
            if (p != -1 && ind1.Contains(2))
                return new WeightCenter().calc(angles.getPointFigure(cutted[0], p)).x;
        }

        dist_sens_front = dist_sens_front == -1 ? max_sensor_length : dist_sens_front;
        dist_sens_left = dist_sens_left == -1 ? max_sensor_length : dist_sens_left;
        dist_sens_right = dist_sens_right == -1 ? max_sensor_length : dist_sens_right;
        double dist = Math.Min(Math.Min(dist_sens_left, dist_sens_right), dist_sens_front);
        if (Math.Abs(dist_sens_front - 1) == Math.Abs(dist_sens_left - 1) && Math.Abs(dist_sens_left - 1) == Math.Abs(dist_sens_right - 1))
            dist = dist_sens_right;
        // получение трапеций расстояния для сенсора, который видит препятствие ближнее
        List<int> indexes = destanations.getTrapezes(dist);
        List<double> precisions = new List<double>();
        for (int i = 0; i < indexes.Count; i++)
        {
            precisions.Add(destanations.trapezes[indexes[i]].getOrdinate(dist));
        }

        List<List<int>> cuttedTrapezes = new List<List<int>>(2);
        cuttedTrapezes.Add(new List<int>());
        cuttedTrapezes.Add(new List<int>());
        for (int i = 0; i < precisions.Count; ++i)
        {
            if (indexes[i] == 0)
            {
                //Debug.Log("Правило старого угла номер " + 1);
                cuttedTrapezes[i].Add(0);
            }
            if (indexes[i] == 1)
            {
                //Debug.Log("Правило старого угла номер " + 2);
                cuttedTrapezes[i].Add(1);
                //cuttedTrapezes[i].Add(2);
            }
            if (indexes[i] == 2)
            {
                //Debug.Log("Правило старого угла номер " + 3);
                cuttedTrapezes[i].Add(1);
            }
        }

        if (precisions.Count == 0)
            return 0;
        if (precisions.Count == 1)
            return new WeightCenter().calc(angles.getPointFigure(cuttedTrapezes[0], precisions[0])).x;
        else
            return new WeightCenter().calc(angles.getPointFigure(
                cuttedTrapezes[0], precisions[0],
                cuttedTrapezes[1], precisions[1]
                )).x;
    }

    public double getAngleLeft(double dist_sens_left, double dist_sens_front, double dist_sens_right)
    {
        if (dist_sens_left == -1 && dist_sens_front == -1 && dist_sens_right == -1)
        {
            flag = 0;
            //Debug.Log("С какой стороны цель: " + CalcSide());
            if (CalcSide() == -1)
                return getAngle(target_direction, direction) * 0.3;
            else return -2;
        }
        
        // если левый средний, остальные -1, то чуть влево
        // если левый малый, остальные -1, то чуть вправо
        if (dist_sens_left != -1 && dist_sens_front == -1 && dist_sens_right == -1)
        {
            List<int> ind = destanations.getTrapezes(dist_sens_left);
            List<double> prec = new List<double>();
            for (int i = 0; i < ind.Count; i++)
            {
                prec.Add(destanations.trapezes[ind[i]].getOrdinate(dist_sens_left));
            }
            List<List<int>> cutted = new List<List<int>>(2);
            cutted.Add(new List<int>());
            cutted.Add(new List<int>());
            for (int i = 0; i < prec.Count; ++i)
            {
                if (ind[i] == 1)
                {
                    //Debug.Log("Правило старого угла номер немного влево");
                    cutted[i].Add(2);
                }
                if (ind[i] == 0)
                {
                    //Debug.Log("Правило старого угла номер немного вправо");
                    cutted[i].Add(3);
                }
                if (ind[i] == 2)
                {
                    //Debug.Log("Правило старого угла номер sredne вправо");
                    cutted[i].Add(1);
                }
                if (prec.Count == 0)
                    return 0;
                if (prec.Count == 1)
                    return new WeightCenter().calc(angles.getPointFigure(cutted[0], prec[0])).x;
                else
                    return new WeightCenter().calc(angles.getPointFigure(
                        cutted[0], prec[0],
                        cutted[1], prec[1]
                        )).x;
            }
        }
        // конец

        if (dist_sens_left != -1 && dist_sens_right != -1)
        {
            // маленький ли левый
            List<int> ind = destanations.getTrapezes(dist_sens_left);
            List<double> prec = new List<double>();
            for (int i = 0; i < ind.Count; i++)
            {
                prec.Add(destanations.trapezes[ind[i]].getOrdinate(dist_sens_left));
            }
            List<List<int>> cutted = new List<List<int>>(2);
            cutted.Add(new List<int>());
            cutted.Add(new List<int>());
            double p =-1;
            if (ind.Contains(0))
            {
                cutted[0].Add(3);
                p = prec[findIndex(ind, 0)];
            }
            // большой ли правый
            List<int> ind1 = destanations.getTrapezes(dist_sens_right);
            List<double> prec1 = new List<double>();
            for (int i = 0; i < ind1.Count; i++)
            {
                prec1.Add(destanations.trapezes[ind1[i]].getOrdinate(dist_sens_right));
            }
            List<List<int>> cutted1 = new List<List<int>>(2);
            cutted1.Add(new List<int>());
            cutted1.Add(new List<int>());
            if (p != -1 && ind1.Contains(2))
                return new WeightCenter().calc(angles.getPointFigure(cutted[0], p)).x;
        }
        
        dist_sens_front = dist_sens_front == -1 ? max_sensor_length : dist_sens_front;
        dist_sens_left = dist_sens_left == -1 ? max_sensor_length : dist_sens_left;
        dist_sens_right = dist_sens_right == -1 ? max_sensor_length : dist_sens_right;
        double dist = Math.Min(Math.Min(dist_sens_left, dist_sens_right), dist_sens_front);
        if (Math.Abs(dist_sens_front - 1) == Math.Abs(dist_sens_left - 1) && Math.Abs(dist_sens_left-1) == Math.Abs(dist_sens_right - 1))
            dist = dist_sens_right;
        // получение трапеций расстояния для сенсора, который видит препятствие ближнее
        List<int> indexes = destanations.getTrapezes(dist);
        List<double> precisions = new List<double>();
        for (int i = 0; i < indexes.Count; i++)
        {
            precisions.Add(destanations.trapezes[indexes[i]].getOrdinate(dist));
        }
            
        List<List<int>> cuttedTrapezes = new List<List<int>>(2);
        cuttedTrapezes.Add(new List<int>());
        cuttedTrapezes.Add(new List<int>());
        for (int i = 0; i < precisions.Count; ++i)
        {
            if (indexes[i] == 0)
            {
                //Debug.Log("Правило старого угла номер " + 1);
                cuttedTrapezes[i].Add(5);
            }
            if (indexes[i] == 1)
            {
                //Debug.Log("Правило старого угла номер " + 2);
                cuttedTrapezes[i].Add(4);
                //cuttedTrapezes[i].Add(2);
            }
            if (indexes[i] == 2)
            {
                ////Debug.Log("Правило старого угла номер " + 3);
                cuttedTrapezes[i].Add(5);
            }
        }

        if (precisions.Count == 0)
            return 0;
        if (precisions.Count == 1)
            return new WeightCenter().calc(angles.getPointFigure(cuttedTrapezes[0], precisions[0])).x;
        else
            return new WeightCenter().calc(angles.getPointFigure(
                cuttedTrapezes[0], precisions[0],
                cuttedTrapezes[1], precisions[1]
                )).x;
    }

    public double getAngle(double dist_sens_left, double dist_sens_front, double dist_sens_right)
    {
        if (flag == 1)
        {
            return getAngleLeft(dist_sens_left, dist_sens_front, dist_sens_right);
        }
        if (flag == -1)
        {
            return getAngleRight(dist_sens_left, dist_sens_front, dist_sens_right);
        }
        if (dist_sens_left != -1 && dist_sens_right != -1)
        {
            return 0;
        }
        //if(dist_sens_left == -1 && dist_sens_front != -1 && dist_sens_right == -1)
        //{
        //    return 0;
        //}
        if (dist_sens_left == -1)
        {
            flag = -1;
            return getAngleRight(dist_sens_left, dist_sens_front, dist_sens_right);
        }
        if (dist_sens_right == -1)
        {
            flag = 1;
            return getAngleLeft(dist_sens_left, dist_sens_front, dist_sens_right);
        }
        if (dist_sens_left <= dist_sens_right)
        {
            flag = 1;
            return getAngleLeft(dist_sens_left, dist_sens_front, dist_sens_right);
        }
        if (dist_sens_right <= dist_sens_left)
        {
            flag = -1;
            return getAngleRight(dist_sens_left, dist_sens_front, dist_sens_right);
        }
        return 0;
    }

    public double getAngle2(double dist_sens_left, double dist_sens_front, double dist_sens_right)
    {
        if (dist_sens_left == -1 && dist_sens_front == -1 && dist_sens_right == -1)
            return getAngle(target_direction, direction);
        //dist_sens_front = dist_sens_front == -1 ? max_sensor_length : dist_sens_front;
        //dist_sens_left = dist_sens_left == -1 ? max_sensor_length : dist_sens_left;
        //dist_sens_right = dist_sens_right == -1 ? max_sensor_length : dist_sens_right;
            
        // получение трапеций расстояния для левого сенсора
        List<int> indexes_left = destanations.getTrapezes(dist_sens_left);
        List<double> precisions_left = new List<double>();
        for (int i = 0; i < indexes_left.Count; i++)
        {
            precisions_left.Add(destanations.trapezes[indexes_left[i]].getOrdinate(dist_sens_left));
        }
        // получение трапеций расстояния для центрального сенсора
        List<int> indexes_front = destanations.getTrapezes(dist_sens_front);
        List<double> precisions_front = new List<double>();
        for (int i = 0; i < indexes_front.Count; i++)
        {
            precisions_front.Add(destanations.trapezes[indexes_front[i]].getOrdinate(dist_sens_front));
        }
        // получение трапеций расстояния для правого сенсора
        List<int> indexes_right = destanations.getTrapezes(dist_sens_right);
        List<double> precisions_right = new List<double>();
        for (int i = 0; i < indexes_right.Count; i++)
        {
            precisions_right.Add(destanations.trapezes[indexes_right[i]].getOrdinate(dist_sens_right));
        }

        // сбор трапеций
        List<List<int>> cuttedTrapezes = new List<List<int>>(2);
        cuttedTrapezes.Add(new List<int>());
        cuttedTrapezes.Add(new List<int>());
        List<double> precision = new List<double>();
        // в каждам if надо добавить в cuttedTrapezes трапецию и соответствующую ей вероятность в precisions
        // наример: если добавлять маленький угол влево, то надо добавить в precisions precisions_left[indexes_front.find(0)]
        // т.е. вероятность при которой расстояние справа маленькое

        // если indexes.Count == 0, то значит трапеция не добавилась, значит она вышла за границы, значит -1

        // L=Большое, M=-1, R=-1 или L=Среднее, M=-1, R=-1
        if (indexes_left.Contains(2) && indexes_front.Count == 0 && indexes_right.Count == 0
            || indexes_left.Contains(1) && indexes_front.Count == 0 && indexes_right.Count == 0)
        {
            if (CalcSide() == 1)
                return getAngle(target_direction, direction);
            if (CalcSide() == -1)
                return 0;
        }
        // L=-1, M=-1, R=Среднее или L=-1, M=-1, R=Большое
        else if (indexes_left.Count == 0 && indexes_front.Count == 0 && indexes_right.Contains(2)
            || indexes_left.Count == 0 && indexes_front.Count == 0 && indexes_right.Contains(1))
        {
            if (CalcSide() == 1)
                return 0;
            if (CalcSide() == -1)
                return getAngle(target_direction, direction);;
        }
        // L=-1, M=-1, R=Маленькое
        // Поворочаваем на
        else if (indexes_left.Count == 0 && indexes_front.Count == 0 && indexes_right.Contains(0))
        {
            cuttedTrapezes[0].Add(3);
            cuttedTrapezes[0].Add(2);
            // ищем индекс по котором трапеция маленькая и по этому индексу вытаскиваем вероятность, с которой считается что она маленькая
            double prec = precisions_right[findIndex(indexes_right, 0)];
                
        }
        //L=-1, M=Большая, R=-1
        else if (indexes_left.Count == 0 && indexes_front.Contains(2) && indexes_right.Count == 0) {
            return 0;
        }
        //L=-1, M=Средняя, R=-1
        else if (indexes_left.Count == 0 && indexes_front.Contains(1) && indexes_right.Count == 0)
        {
            // можем налево и направо повернуться с одинаковой вероятностью
            if (CalcSide() == 1)
            {
                // cutted[0] - empty, cutted[1] содержит трапецию на поворот направо
                precision.Add(-1);
                cuttedTrapezes[1].Add(4);
                precision.Add(precisions_front[findIndex(indexes_front, 1)]);
            }
                // наоборот
            else if (CalcSide() == -1)
            {
                cuttedTrapezes[0].Add(1);
                precision.Add(precisions_front[findIndex(indexes_front, 1)]);
                precision.Add(-1);
            }
        }
        // L=-1, M=маленькая, R=-1
        else if (indexes_left.Count == 0 && indexes_front.Contains(0) && indexes_right.Count == 0)
        {
            // можем налево и направо повернуться с одинаковой вероятностью
            if (CalcSide() == 1)
            {
                // cutted[0] - empty, cutted[1] содержит трапецию на поворот направо
                precision.Add(-1);
                cuttedTrapezes[1].Add(5);
                precision.Add(precisions_front[findIndex(indexes_front, 0)]);
            }
            // наоборот
            else if (CalcSide() == -1)
            {
                cuttedTrapezes[0].Add(0);
                precision.Add(precisions_front[findIndex(indexes_front, 0)]);
                precision.Add(-1);
            }
        }
        // L=Большая, M=Большая, R=-1
        else if (indexes_left.Contains(2) && indexes_front.Contains(2) && indexes_right.Count == 0)
        {
            // немного направо с максимум вероятностью что слева и по центру большое расстояние до препятствия
            cuttedTrapezes[1].Add(3);
            // максимум вероятностей что по центру и слева расстояние большое
            precision.Add(-1);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 2)]
                , precisions_front[findIndex(indexes_front, 2)]);
            precision.Add(prec);
        }
        // L=-1, M=Большая, R=Большая;
        else if (indexes_left.Count == 0 && indexes_front.Contains(2) && indexes_right.Contains(2))
        {
            // немного налево с максимум вероятностью что справа и по центру большое расстояние до препятствия
            cuttedTrapezes[0].Add(5);
            double prec = Math.Max(precisions_front[findIndex(indexes_front, 2)]
                , precisions_right[findIndex(indexes_right, 2)]);
            precision.Add(prec);
        }
        // L=Средняя, M=Средняя, R=-1; L=Средняя, M=Маленькая, R=-1;
        // L=Большое, M=Средняя, R=-1; L=Большое, M=Маленькая, R=-1; L=Маленькая, M=Маленькая, R=-1;
        else if (indexes_left.Contains(1) && indexes_front.Contains(1) && indexes_right.Count == 0)
        {
            cuttedTrapezes[0].Add(5);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 1)]
                , precisions_front[findIndex(indexes_front, 1)]);
            precision.Add(prec);
        }
        else if(indexes_left.Contains(1) && indexes_front.Contains(0) && indexes_right.Count == 0)
        {
            cuttedTrapezes[0].Add(5);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 1)]
                , precisions_front[findIndex(indexes_front, 0)]);
            precision.Add(prec);
        }
        else if(indexes_left.Contains(2) && indexes_front.Contains(1) && indexes_right.Count == 0)
        {
            cuttedTrapezes[0].Add(5);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 2)]
                , precisions_front[findIndex(indexes_front, 1)]);
            precision.Add(prec);
        }
        else if(indexes_left.Contains(2) && indexes_front.Contains(0) && indexes_right.Count == 0)
        {
            cuttedTrapezes[0].Add(5);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 2)]
                , precisions_front[findIndex(indexes_front, 0)]);
            precision.Add(prec);
        }
        else if(indexes_left.Contains(0) && indexes_front.Contains(0) && indexes_right.Count == 0)
        {
            cuttedTrapezes[0].Add(5);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 0)]
                , precisions_front[findIndex(indexes_front, 0)]);
            precision.Add(prec);
        }
        // L=Среднее, M=Большая, R=-1
        else if (indexes_left.Contains(1) && indexes_front.Contains(2) && indexes_right.Count == 0
            || indexes_left.Contains(2) && indexes_front.Count == 0 && indexes_right.Contains(2)
            || indexes_left.Contains(2) && indexes_front.Count == 0 && indexes_right.Contains(1)
            || indexes_left.Contains(1) && indexes_front.Count == 0 && indexes_right.Contains(2)
            || indexes_left.Contains(1) && indexes_front.Count == 0 && indexes_right.Contains(1)
            || indexes_left.Contains(0) && indexes_front.Count == 0 && indexes_right.Contains(0)
            || indexes_left.Contains(0) && indexes_front.Contains(2) && indexes_right.Contains(0)
            || indexes_left.Contains(0) && indexes_front.Contains(1) && indexes_right.Contains(0)
            || indexes_left.Contains(1) && indexes_front.Contains(2) && indexes_right.Contains(1)
            || indexes_left.Contains(2) && indexes_front.Contains(2) && indexes_right.Contains(2))
        {
            return 0;
        }
        // L=Маленькая, M=Большая, R=-1
        else if (indexes_left.Contains(0) && indexes_front.Contains(2) && indexes_right.Count == 0)
        {
            cuttedTrapezes[1].Add(3);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 0)]
                , precisions_front[findIndex(indexes_front, 2)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        // L=Маленькая, M=Средняя, R=-1
        else if (indexes_left.Contains(0) && indexes_front.Contains(1) && indexes_right.Count == 0)
        {
            cuttedTrapezes[1].Add(4);
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 0)]
                , precisions_front[findIndex(indexes_front, 1)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        //
        else if (indexes_left.Count == 0 && indexes_front.Contains(2) && indexes_right.Contains(2) ||
            indexes_left.Count == 0 && indexes_front.Contains(2) && indexes_right.Contains(1)
            || indexes_left.Count == 0 && indexes_front.Contains(1) && indexes_right.Contains(1) ||
            indexes_left.Count == 0 && indexes_front.Contains(2) && indexes_right.Contains(0) ||
            indexes_left.Count == 0 && indexes_front.Contains(1) && indexes_right.Contains(0))
        {
            // 
            if (CalcSide() == -1)
                return getAngle(target, direction);
            return 0;
        }
        else if (indexes_left.Count == 0 && indexes_front.Contains(1) && indexes_right.Contains(2))
        {
            if (CalcSide() == -1)
                return getAngle(target, direction);
            // 
            cuttedTrapezes[1].Add(4);
            // 
            double prec = Math.Max(precisions_front[findIndex(indexes_front, 1)]
                , precisions_right[findIndex(indexes_right, 2)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        //
        else if (indexes_left.Count == 0 && indexes_front.Contains(0) && indexes_right.Contains(2))
        { 
            cuttedTrapezes[1].Add(5);
            // 
            double prec = Math.Max(precisions_front[findIndex(indexes_front, 0)]
                , precisions_right[findIndex(indexes_right, 2)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        //
        else if (indexes_left.Count == 0 && indexes_front.Contains(0) && indexes_right.Contains(1)) {
            cuttedTrapezes[1].Add(5);
            // 
            double prec = Math.Max(precisions_front[findIndex(indexes_front, 0)]
                , precisions_right[findIndex(indexes_right, 1)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        else if (indexes_left.Count == 0 && indexes_front.Contains(0) && indexes_right.Contains(0))
        {
            cuttedTrapezes[1].Add(5);
            // 
            double prec = Math.Max(precisions_front[findIndex(indexes_front, 0)]
                , precisions_right[findIndex(indexes_right, 0)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        else if (indexes_left.Contains(2) && indexes_front.Count == 0 && indexes_right.Contains(0))
        {
            cuttedTrapezes[0].Add(2);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 2)]
                , precisions_right[findIndex(indexes_right, 0)]);
                
            precision.Add(prec);
        }
        else if(indexes_left.Contains(1) && indexes_front.Count == 0 && indexes_right.Contains(0))
        {
            cuttedTrapezes[0].Add(2);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 1)]
                , precisions_right[findIndex(indexes_right, 0)]);
                
            precision.Add(prec);
        }
        else if (indexes_left.Contains(0) && indexes_front.Count == 0 && indexes_right.Contains(2))
            {
            cuttedTrapezes[1].Add(3);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 0)]
                , precisions_right[findIndex(indexes_right, 2)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        else if(indexes_left.Contains(1) && indexes_front.Count == 0 && indexes_right.Contains(1))
        {
            cuttedTrapezes[1].Add(3);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 1)]
                , precisions_right[findIndex(indexes_right, 1)]);
            precision.Add(-1);
            precision.Add(prec);
        }
        else if (indexes_left.Contains(1) && indexes_front.Contains(0) && indexes_right.Contains(1))
        {
            cuttedTrapezes[1].Add(5);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 1)]
                , precisions_front[findIndex(indexes_front, 0)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 1)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
        else if (indexes_left.Contains(2) && indexes_front.Contains(1) && indexes_right.Contains(2))
        {
            cuttedTrapezes[1].Add(4);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 2)]
                , precisions_front[findIndex(indexes_front, 1)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 2)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
        else if (indexes_left.Contains(2) && indexes_front.Contains(0) && indexes_right.Contains(2))
        {
            cuttedTrapezes[1].Add(5);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 2)]
                , precisions_front[findIndex(indexes_front, 0)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 2)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
        else if (indexes_left.Contains(0) && indexes_front.Contains(0) && indexes_right.Contains(0))
        {
            cuttedTrapezes[1].Add(5);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 0)]
                , precisions_front[findIndex(indexes_front, 0)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 0)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
        else if (indexes_left.Contains(1) && indexes_front.Contains(1) && indexes_right.Contains(1))
        {
            cuttedTrapezes[1].Add(4);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 1)]
                , precisions_front[findIndex(indexes_front, 1)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 1)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
        else if (indexes_left.Contains(0) && indexes_front.Contains(1) && indexes_right.Contains(2))
        {
            cuttedTrapezes[1].Add(3);
            // 
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 0)]
                , precisions_front[findIndex(indexes_front, 1)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 2)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
        else if (indexes_left.Contains(2) && indexes_front.Contains(1) && indexes_right.Contains(0))
        {
            cuttedTrapezes[1].Add(5);
            //  
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 2)]
                , precisions_front[findIndex(indexes_front, 1)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 0)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
            //
        else if (indexes_left.Contains(0) && indexes_front.Contains(1) && indexes_right.Contains(1))
        {
            cuttedTrapezes[1].Add(4);
            //  
            double prec = Math.Max(precisions_left[findIndex(indexes_left, 0)]
                , precisions_front[findIndex(indexes_front, 1)]);

            double prec2 = Math.Max(prec, precisions_right[findIndex(indexes_right, 1)]);
            precision.Add(-1);
            precision.Add(prec2);
        }
        else
        {
            cuttedTrapezes[1].Add(5);
            precision.Add(-1);
            precision.Add(0.8);
        }
        ///
        return new WeightCenter().calc(angles.getPointFigure(
                cuttedTrapezes, precision
                )).x;
    }
    public void setCoordinate(double x, double y)
    {
        current = new Point(x, y);
    }
    public Point get_target()
    {
        return target;
    }
    public void set_direction(Point new_direction)
    {
        direction = new_direction;
    }
    public void set_target_direction(Point new_target_direction)
    {
        target_direction = new_target_direction;
    }

    public void set_velocity(float new_velocity)
    {
        max_velocity = new_velocity;
    }

    public float get_velocity()
    {
        return max_velocity;
    }

    public void set_target(Point t)
    {
        target = t;
    }
}

