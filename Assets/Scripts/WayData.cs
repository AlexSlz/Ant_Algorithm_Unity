using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayData
{
    public int first;
    public int second;
    public double length;
    public double tau;
    public WayData(int _first, int _second, double _length)
    {
        first = _first;
        second = _second;
        length = _length;
        tau = 0.01;
    }
}
