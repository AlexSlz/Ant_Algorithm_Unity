using System.Collections.Generic;
using System;
using UnityEngine;

public class Algorithm
{

    static System.Random random = new System.Random();

    public static double alpha = 0;
    public static double beta = 0;

    public static double rho = 0;
    public static double Q = 0;

    public static void UpdatePheromones(List<Ant> ants, List<WayData> wayd, int numCities)
    {

        //alpha = 1;//numCities;
        //beta = 1;//(numCities / 2);
        //Q = (float)numCities / 10;
        //rho = (float)numCities / 100;
        //Debug.Log("a " + alpha + " | b " + beta + " | Q " + Q + " | r " + rho);

        foreach (var item in wayd)
        {
            bool flag = false;
            for (int i = 0; i < ants.Count / 2; i++)
            {
                for (int j = 0; j < ants[i].tail.Length - 1; j++)
                {
                    if (item.first == ants[i].tail[j] && item.second == ants[i].tail[j + 1] || (item.first == ants[i].tail[j + 1] && item.second == ants[i].tail[j]))
                    {
                        double length = Length(ants[i].tail, wayd);
                        item.tau = (1 - rho) * item.tau + (Q / length);
                        flag = true;
                        break;
                    }
                }
            }
            if (flag == false)
            {
                item.tau = (1 - rho) * item.tau;
            }
        }
    }
    public static void UpdateAnts(List<Ant> ants, List<WayData> wayd, int numCities, int startPoint)
    {
        for (int k = 0; k < ants.Count; k++)
        {
            ants[k].tail = BuildTrail(wayd, numCities, startPoint);
        }
    }
    static int[] BuildTrail(List<WayData> wayd, int numCities, int startPoint)
    {
        int[] trail = new int[numCities];
        List<double> knowWay = new List<double>();
        trail[0] = startPoint;
        if (startPoint == numCities)
            knowWay.Add(0);
        else
            knowWay.Add(startPoint);
        for (int i = 1; i < trail.Length; i++)
        {
            int next = NextPoint(wayd, numCities, knowWay);
            if (next == 0)
                trail[i] = numCities;
            else
            trail[i] = next;
            knowWay.Add(next);
        }
        //Console.WriteLine("tail end");
        return trail;
    }
    static int NextPoint(List<WayData> wayd, int numCities, List<double> knowWay)
    {
        double[] probs = MoveProbs(wayd, numCities, knowWay);
        //for (int i = 0; i < probs.Length; i++)
        //    Console.WriteLine(probs[i]);
        int ok = 0;
        double rand = random.NextDouble();
        for (double i = 0; ; i += probs[ok], ok++)
        {
            if (i < rand && rand < (i + probs[ok]))
                break;
        }
        //Console.WriteLine("Winner: " + ok + " p: " + probs[ok]);
        return ok;
    }
    static double[] MoveProbs(List<WayData> wayd, int numCities, List<double> knowWay)
    {
        double[] taueta = new double[numCities];
        double sum = 0.0;
        for (int i = 0; i < taueta.Length; i++)
        {
            bool flag = true;
            foreach (var item in knowWay)
            {
                if (item == i)
                    flag = false;
            }
            if (flag == true)
            {
                taueta[i] = Math.Pow(wayd[i].tau, alpha) * Math.Pow((1.0 / wayd[i].length), beta);
                if (taueta[i] < 0.000001)
                    taueta[i] = 0.000001;
                else if (taueta[i] > (double.MaxValue / (numCities * 100)))
                    taueta[i] = double.MaxValue / (numCities * 100);
            }
            sum += taueta[i];
        }


        double[] probs = new double[numCities];
        for (int i = 0; i < probs.Length; ++i)
            probs[i] = taueta[i] / sum;
        return probs;
    }




    public static string DisplayAnt(List<Ant> ants, List<WayData> wayd, bool len)
    {
        string result = "";
        for (int j = 0; j < ants.Count; j++)
        {
            for (int q = 0; q < GetCount(wayd); q++)
            {
                result += ants[j].tail[q] + " -> ";
            }
            if (len)
                result += "| " + Length(ants[j].tail, wayd) + " | ";
        }
        return result;
    }
    public static string DisplayTail(int[] tail)
    {
        string result = "";
        for (int i = 0; i < tail.Length; i++)
        {
            if (tail.Length - 1 == i)
            {
                result += tail[i] + "";
            }
            else
            {
                result += tail[i] + " -> ";
            }
        }
        return result;
    }
    public static int[] BestTrail(List<Ant> ants, List<WayData> wayd)
    {
        double second, best = 0;
        int k = 0;
        for (int i = 0; i < ants.Count; i++)
        {
            if (best == 0)
            {
                best = Length(ants[0].tail, wayd);
            }
            second = Length(ants[i].tail, wayd);
            if (best > second)
            {
                best = second;
                k = i;
            }
        }
        int[] bestTrail = new int[GetCount(wayd)];
        ants[k].tail.CopyTo(bestTrail, 0);
        return bestTrail;
    }
    public static int[] RandomTrail(int start, int numCities)
    {
        int[] trail = new int[numCities];
        var knownNumbers = new HashSet<int>();
        trail[0] = start;
        knownNumbers.Add(start);
        for (int i = 1; i < trail.Length; i++)
        {
            trail[i] = (random.Next(numCities) + 1);
            while (!knownNumbers.Add(trail[i]))
            {
                trail[i] = (random.Next(numCities) + 1);
            }
        }
        return trail;
    }
    public static double Length(int[] ants, List<WayData> wayd)
    {
        double result = 0;
        for (int i = 0; i < ants.Length - 1; i++)
        {
            result += wayd.Find(item => (item.first == ants[i] && item.second == ants[i + 1]) || (item.second == ants[i] && item.first == ants[i + 1])).length;
            //Debug.Log(ants[i] + " " + ants[i + 1] + " | " + wayd.Find(item => item.first == ants[i] && item.second == ants[i + 1]).length + " ");
        }
        return result;
    }
    public static int GetCount(List<WayData> wayd)
    {
        List<WayData> wayp = new List<WayData>();
        int count = 0;
        foreach (var item in wayd)
        {
            if (count < item.second)
                count = item.second;
        }
        return count;

    }
    public static void AddReverse(List<WayData> wayd)
    {
        List<WayData> wayp = new List<WayData>();
        foreach (var item in wayd)
        {
            wayp.Add(new WayData(item.second, item.first, item.length, item.lineC));
        }
        foreach (var item in wayp)
        {
            wayd.Add(item);
        }
    }
}
