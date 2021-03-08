using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    float t = 0;
    [SerializeField] private TextMeshProUGUI LenText;
    private LineRenderer lr;
    public PointController first;
    public PointController second;
    public bool visible = false;

    private Color _color = new Color(130f / 255f, 101f / 255f, 62f / 255f);

    private void Awake()
    {

        lr = GetComponent<LineRenderer>();    
    }

    public void AddPoint(PointController f, PointController s, WayData w)
    {
        first = f;
        second = s;
        LenText.text = w.length.ToString();
        this.name = LenText.text;
        Instantiate(LenText, Vector3.zero, Quaternion.identity, this.transform);
    }
    public void SetColor(float tau, bool _bool)
    {
        if (_bool)
            t = tau;
        else if (!_bool)
            t = 0.5f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(_color, 0.0f), new GradientColorKey(_color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(tau, 0.0f), new GradientAlphaKey(tau, 1.0f) }
        );
        
        if(GetComponentInChildren<TextMeshProUGUI>() != null)
            GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(_bool);
        lr.colorGradient = gradient;

    }
    private void LateUpdate()
    {
        lr.SetPosition(0, first.transform.position);
        lr.SetPosition(1, second.transform.position);
    }

}
