using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    public PointController first;
    public PointController second;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();    
    }

    public void AddPoint(PointController f, PointController s)
    {
        first = f;
        second = s;
    }

    private void LateUpdate()
    {
        lr.SetPosition(0, first.transform.position);
        lr.SetPosition(1, second.transform.position);
    }

}
