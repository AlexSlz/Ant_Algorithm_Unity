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
    public Toggle Toggle_Del;
    [SerializeField] private GridLayoutGroup controlCell;



    int o_old;

    //TMP_InputField
    [SerializeField] private List<GameObject> Inputs;

    void SettAutoSett()
    {
        if(_Alpha.text == "")
            _Alpha.text = Algorithm.alpha + "";
        if(_Beta.text == "")
            _Beta.text = Algorithm.beta + "";
        if (_Q.text == "")
            _Q.text = Algorithm.Q + "";
        if (_RHO.text == "")
            _RHO.text = Algorithm.rho + "";
    }

    public void ShowSettings()
    {
        if (PointCount >= 2)
        {
            //SettAutoSett();
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
            _alert.SetAlert("Потрібно додати більше 2 точок.");
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
    void SettCell()
    {
        double temp = 1500 / PointCount;
        if (temp % 2 != 0)
            temp -= 1;
        if (PointCount == 7 || PointCount == 5)
            temp += 50;
        if (PointCount == 9)
            temp = 190;
        if (PointCount <= 4)
            temp = 500;
        if (temp <= 150)
            temp = 150;
        Vector2 vector2 = new Vector2(Convert.ToSingle(temp), 70);
        controlCell.cellSize = vector2;
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
        SettCell();
    }

    void SetSettings()
    {
        if (_Alpha.text == "" || _Alpha.text == "0")
            _Alpha.text = Algorithm.alpha + "";
        //Algorithm.alpha = Convert.ToSingle(_Alpha.text);

        if (_Beta.text == "" || _Beta.text == "0")
            _Beta.text = Algorithm.beta + "";
        //Algorithm.beta = Convert.ToSingle(_Beta.text);
        if (_Q.text == "" || _Q.text == "0")
            _Q.text = Algorithm.Q + "";
        //Algorithm.Q = Convert.ToSingle(_Q.text);
        if (_RHO.text == "" || _RHO.text == "0")
            _RHO.text = Algorithm.rho + "";
        //Algorithm.rho = Convert.ToSingle(_RHO.text);
    }

}
