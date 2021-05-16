using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PointController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool mouseDown;
    public Action<PointController> OnDragEvent;
    [SerializeField] public Image Timer;
    [SerializeField] private Camera cam;
    public void OnDrag(PointerEventData eventData)
    {
        mouseDown = false;
        this.transform.position = Main.GetMousePos();
    }

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 point = cam.WorldToViewportPoint(transform.position);
        if (point.y < 0.15f || point.y > 1f || point.x > 1f || point.x < 0f)
            this.transform.position = Vector3.zero;
        if (Timer.fillAmount <= 1)
        {
            if (Debug.isDebugBuild)
            {
                Timer.fillAmount -= (float)0.007;
            }
            else
            {
                Timer.fillAmount -= (float)0.07;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Main.SelectedPoint = this;
        
        mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
    }
}
