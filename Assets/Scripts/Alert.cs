using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Alert : MonoBehaviour
{
    public void SetAlert(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
        GetComponent<Animator>().SetTrigger("ok");
    }
}
