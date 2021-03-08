using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSet : MonoBehaviour
{


    public static int PointCount;
    [SerializeField] private TextMeshProUGUI StartPointText;
    [SerializeField] private GameObject AddZone;
    [SerializeField] private GameObject Prefab;

    [SerializeField] private Alert _alert;

    [SerializeField] private TMP_InputField _Alpha;
    [SerializeField] private TMP_InputField _Beta;
    [SerializeField] private TMP_InputField _Q;
    [SerializeField] private TMP_InputField _RHO;
    [SerializeField] public Toggle Toggle_Del;

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
            _alert.SetAlert("Нужно добавить больше 2 точек.");
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
        if (_Alpha.text == "" || _Alpha.text == "0")
            _Alpha.text = "1";
        Algorithm.alpha = Convert.ToDouble(_Alpha.text);

        if (_Beta.text == "" || _Beta.text == "0")
            _Beta.text = "1";
        Algorithm.beta = Convert.ToDouble(_Beta.text);
        if (_Q.text == "" || _Q.text == "0")
            _Q.text = "0,2";
        Algorithm.Q = Convert.ToDouble(_Q.text);
        if (_RHO.text == "" || _RHO.text == "0")
            _RHO.text = "0,01";
        Algorithm.rho = Convert.ToDouble(_RHO.text);
    }

}
