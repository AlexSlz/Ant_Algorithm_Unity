using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickScript : MonoBehaviour, IPointerClickHandler
{
    public Action OnClickEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerId == -1 && eventData.pointerPress == this.gameObject)
        {
            OnClickEvent?.Invoke();
            
        }
        else if(eventData.pointerId == -1)
        {
            Main.SelectedPoint = null;
        }
    }
}
