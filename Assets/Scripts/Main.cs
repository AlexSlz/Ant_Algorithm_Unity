using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main : MonoBehaviour
{
    public int startPoint = 1;
    public int antCount = 1;
    private int antSpeed = 0;
    [SerializeField] private TextMeshProUGUI SpeedText;
    [SerializeField] private TextMeshProUGUI BestText;
    [SerializeField] private ClickScript _ClickZone;
    [SerializeField] private GameObject _SpawnAnt;
    [SerializeField] private GameObject PointPref;
    [SerializeField] private GameObject LinePref;
    [SerializeField] private GameObject AntPref;

    List<Ant> ants = new List<Ant>();
    List<WayData> wayd = new List<WayData>();
    List<PointController> p = new List<PointController>();

    int numCities;
    private int[] bestTail;
    void Start()
    {
        _ClickZone.OnClickEvent += AddPoint;
    }
    public void SpeedUpdate(float value)
    {
        antSpeed = Mathf.RoundToInt(value * 69);
        foreach (var item in _ClickZone.GetComponentsInChildren<AntAi>())
        {
            item.SpeedUpdate(antSpeed);
        }
        SpeedText.text = antSpeed + "";
    }
    private void Update()
    {
    }
    private void AddPoint()
    {
        p.Add(Instantiate(PointPref, GetMousePos(), Quaternion.identity, _ClickZone.transform).GetComponent<PointController>());
        p[p.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = p.Count + "";
    }

    public void StartAlgorithm()
    {
        if (p.Count >= 2)
        {
            CreateWay();

            numCities = Algorithm.GetCount(wayd);

            ants.Clear();
            for (int i = 0; i < antCount; ++i)
            {
                int[] tail = Algorithm.RandomTrail(1, numCities);
                ants.Add(new Ant(tail));
            }

            bestTail = Algorithm.BestTrail(ants, wayd);

            Debug.Log(Algorithm.DisplayTail(bestTail));

            Instantiate(AntPref, p[0].transform.position, Quaternion.identity, _SpawnAnt.transform).GetComponent<AntAi>().AddAnt(ants, wayd, p, numCities, bestTail, startPoint, BestText);
            
        }
        else
        {
            Debug.LogError("Count < 2");
        }
    }
    private void CreateWay()
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
        
        for (int i = 1; i < p.Count + 1; i++)
        {
            for (int j = i + 1; j < p.Count + 1; j++)
            {
                wayd.Add(new WayData(i, j, i + j, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
            }
        }
        /*
        wayd.Add(new WayData(1, 2, 12, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(1, 3, 9, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(1, 4, 11, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(1, 5, 2, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(1, 6, 2, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(2, 3, 13, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(2, 4, 9, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(2, 5, 10, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(2, 6, 7, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(3, 4, 5, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(3, 5, 11, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(3, 6, 6, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(4, 5, 3, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(4, 6, 17, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        wayd.Add(new WayData(5, 6, 13, Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>()));
        */
        for (int i = 0; i < wayd.Count; i++)
        {
            wayd[i].lineC.AddPoint(p[wayd[i].first - 1], p[wayd[i].second - 1]);
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
