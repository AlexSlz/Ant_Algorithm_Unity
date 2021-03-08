using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PointController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool mouseDown;
    public Action<PointController> OnDragEvent;
    public void OnDrag(PointerEventData eventData)
    {
        mouseDown = false;
        this.transform.position = Main.GetMousePos();
    }

    void LateUpdate()
    {
        if (GetComponent<Image>().fillAmount <= 1)
        {
            GetComponent<Image>().fillAmount += 0.003f;
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
