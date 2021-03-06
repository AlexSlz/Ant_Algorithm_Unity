using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsSet : MonoBehaviour
{


    public static int PointCount;
    [SerializeField] private TextMeshProUGUI StartPointText;
    [SerializeField] private GameObject AddZone;
    [SerializeField] private GameObject Prefab;


    [SerializeField] private TMP_InputField _Alpha;
    [SerializeField] private TMP_InputField _Beta;
    [SerializeField] private TMP_InputField _Q;
    [SerializeField] private TMP_InputField _RHO;

    int o_old;

    //TMP_InputField
    [SerializeField] private List<GameObject> Inputs;

    public void ShowSettings()
    {
        if (PointCount >= 2)
        {
            if (Inputs.Count != 0)
                RecReatea();
            else
            {
                AddInput();
                o_old = PointCount;
            }
            gameObject.SetActive(!gameObject.activeSelf);
            SetSettings();
        }
        else
        {
            Debug.LogError("Count < 2");
        }
    }

    public List<GameObject> GetInput()
    {
        return Inputs;
    }

    public void SetStartPoss(float value)
    {
        Main.startPoint = Mathf.RoundToInt(value * (PointCount - 1)) + 1;
        StartPointText.text = Main.startPoint + "";
    }
    void RecReatea()
    {
        int o = PointCount;
        if (o_old != o)
        {
            Debug.Log("REcReate");
            foreach (var item in Inputs)
            {
                Destroy(item.gameObject);
            }
            Inputs.Clear();
            AddInput();
            o_old = PointCount;
        }
    }
    void AddInput()
    {
        for (int i = 0; i < PointCount - 1; i++)
        {
            for (int j = i; j < PointCount - 1; j++)
            { 
                Prefab.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1) + " -> " + (j + 2);
                Inputs.Add(Instantiate(Prefab, Vector3.zero, Quaternion.identity, AddZone.transform));
            }
        }
    }

    void SetSettings()
    {
        Algorithm.alpha = Convert.ToDouble(_Alpha.text);
        Algorithm.beta = Convert.ToDouble(_Beta.text);
        Algorithm.Q = Convert.ToDouble(_Q.text);
        Algorithm.rho = Convert.ToDouble(_RHO.text);
    }

}
