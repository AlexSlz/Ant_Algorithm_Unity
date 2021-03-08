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
    public void OnDrag(PointerEventData eventData)
    {
        mouseDown = false;
        this.transform.position = Main.GetMousePos();
    }

    void LateUpdate()
    {
        if(this.transform.position.y >= Screen.height / 100 || this.transform.position.y <= -(Screen.height / 125))
            this.transform.position = Vector3.zero;
        else if(this.transform.position.x >= Screen.width / 101 || this.transform.position.x <= -(Screen.width / 101))
            this.transform.position = Vector3.zero;
        if (Timer.fillAmount <= 1)
        {
            Timer.fillAmount -= 0.007f;
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
