using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMove : MonoBehaviour
{
    private void LateUpdate()
    {
        this.transform.position = (GetComponentInParent<LineController>().first.transform.position + GetComponentInParent<LineController>().second.transform.position) / 2;
    }
}
