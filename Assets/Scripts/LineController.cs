using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineController : MonoBehaviour
{

    public float t = 0;
    [SerializeField] private TextMeshProUGUI LenText;
    private LineRenderer lr;
    public PointController first;
    public PointController second;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();    
    }

    public void AddPoint(PointController f, PointController s, WayData w)
    {
        first = f;
        second = s;
        LenText.text = w.length.ToString();
        Instantiate(LenText, Vector3.zero, Quaternion.identity, this.transform);
    }
    public void SetColor(float tau)
    {
        t = tau;
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
