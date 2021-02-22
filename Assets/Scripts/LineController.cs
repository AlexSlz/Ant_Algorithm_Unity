using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private List<PointController> points;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;

        points = new List<PointController>();
    }
    public void AddPoint(PointController p)
    {
        lr.positionCount++;
        points.Add(p);
    }
    private void LateUpdate()
    {
        if(points.Count >= 2)
        {
            for (int i = 0; i < points.Count; i++)
            {
                lr.SetPosition(i, points[i].transform.position);
            }
        }
    }

}
