using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayData
{
    public int first;
    public int second;
    public double length;
    public double tau;
    public LineController lineC;
    public WayData(int _first, int _second, double _length, LineController ln)
    {
        first = _first;
        second = _second;
        length = _length;
        lineC = ln;
        tau = Algorithm.rho;
    }
}
