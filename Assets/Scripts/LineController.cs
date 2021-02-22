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

      
    }
    public void AddPoint(List<PointController> p)
    {
        lr.positionCount++;
        points = p;
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
