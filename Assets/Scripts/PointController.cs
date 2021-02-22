using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointController : MonoBehaviour, IDragHandler, IPointerClickHandler
{

    public Action<PointController> OnDragEvent;

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Main.GetMousePos();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //this.GetComponent<Image>().color = Color.green;
    }

}
