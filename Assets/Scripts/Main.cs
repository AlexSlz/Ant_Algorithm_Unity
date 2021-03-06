﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main : MonoBehaviour
{

    public static int startPoint = 1;
    public int antCount = 1;
    private int antSpeed = 0;
    public static int antMaxSpeed = 41;
    [SerializeField] private TextMeshProUGUI SpeedText;
    [SerializeField] private TextMeshProUGUI BestText;
    [SerializeField] private ClickScript _ClickZone;
    [SerializeField] private GameObject _SpawnAnt;
    [SerializeField] private GameObject PointPref;
    [SerializeField] private GameObject LinePref;
    [SerializeField] private GameObject AntPref;
    [SerializeField] private SettingsSet Settings;

    List<double> InputData = new List<double>();

    List<Ant> ants = new List<Ant>();
    List<WayData> wayd = new List<WayData>();
    public List<PointController> p = new List<PointController>();

    int numCities;
    private int[] bestTail;
    void Start()
    {
        _ClickZone.OnClickEvent += AddPoint;
    }
    public void SpeedUpdate(float value)
    {
        antSpeed = Mathf.RoundToInt(value * antMaxSpeed);
        foreach (var item in _ClickZone.GetComponentsInChildren<AntAi>())
        {
            item.SpeedUpdate(antSpeed);
        }
        if(antSpeed == antMaxSpeed)
            SpeedText.text = "MaxSpeed";
        else
        SpeedText.text = antSpeed + "";
    }
    private void Update()
    {
    }
    private void AddPoint()
    {
        p.Add(Instantiate(PointPref, GetMousePos(), Quaternion.identity, _ClickZone.transform).GetComponent<PointController>());
        p[p.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = p.Count + "";
        SettingsSet.PointCount = p.Count;
    }



    public void ResetAlgorithm()
    {
        foreach (var item in _ClickZone.GetComponentsInChildren<LineController>())
        {
            Destroy(item.gameObject);
        }
        foreach (var item in _ClickZone.GetComponentsInChildren<AntAi>())
        {
            Destroy(item.gameObject);
        }

        wayd.Clear();
        ants.Clear();
    }
    public void StartAlgorithm()
    {
        if (p.Count >= 2)
        {
            GetInputData();
            CreateWay();

            numCities = Algorithm.GetCount(wayd);

            for (int i = 0; i < antCount; ++i)
            {
                int[] tail = Algorithm.RandomTrail(startPoint, numCities);
                ants.Add(new Ant(tail));
            }
            bestTail = Algorithm.BestTrail(ants, wayd);

            Debug.Log(Algorithm.DisplayTail(bestTail));

            Instantiate(AntPref, p[startPoint - 1].transform.position, Quaternion.identity, _SpawnAnt.transform).GetComponent<AntAi>().AddAnt(ants, wayd, p, numCities, bestTail, BestText);
            
        }
        else
        {
            Debug.LogError("Count < 2");
        }
    }

    void GetInputData()
    {
        InputData.Clear();
        List<GameObject> temp = Settings.GetInput();
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].GetComponentInChildren<TMP_InputField>().text != "")
                InputData.Add(Convert.ToDouble(temp[i].GetComponentInChildren<TMP_InputField>().text));
            else
                InputData.Add(UnityEngine.Random.Range(i + 1, (i + 1) * 10));
        }
    }

    private void CreateWay()
    {
        ResetAlgorithm();

        int o = 0;
        for (int i = 1; i < p.Count + 1; i++)
        {
            for (int j = i + 1; j < p.Count + 1; j++)
            {
                wayd.Add(new WayData(i, j, InputData[o], Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
                o++;
            }
        }
        for (int i = 0; i < wayd.Count; i++)
        {
            wayd[i].lineC.AddPoint(p[wayd[i].first - 1], p[wayd[i].second - 1], wayd[i]);
        }
        Algorithm.AddReverse(wayd);
    }

    public static Vector3 GetMousePos()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        return worldPos;
    }

}
