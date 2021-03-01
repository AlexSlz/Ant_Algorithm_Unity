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
    public void SetColor(float tau)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(tau, 0.0f), new GradientAlphaKey(tau, 1.0f) }
        );
        lr.colorGradient = gradient;

    }
    private void LateUpdate()
    {
        lr.SetPosition(0, first.transform.position);
        lr.SetPosition(1, second.transform.position);
    }

}
