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
    [SerializeField] private ClickScript _ClickZone;
    [SerializeField] private GameObject PointPref;
    [SerializeField] private GameObject LinePref;
    [SerializeField] private GameObject AntPref;
    private LineController currLine;

    List<Ant> ants = new List<Ant>();
    List<WayData> wayd = new List<WayData>();
    List<PointController> p = new List<PointController>();

    int numCities;
    private int[] bestTail;
    void Start()
    {
        _ClickZone.OnClickEvent += AddPoint;



        wayd.Add(new WayData(1, 2, 2));
        wayd.Add(new WayData(1, 3, 1.2));
        wayd.Add(new WayData(1, 4, 3));
        wayd.Add(new WayData(2, 3, 2.2));
        wayd.Add(new WayData(2, 4, 3));
        wayd.Add(new WayData(3, 4, 3));

        Algorithm.AddReverse(wayd);

        numCities = Algorithm.GetCount(wayd);


        for (int i = 0; i < antCount; ++i)
        {
            ants.Add(new Ant(Algorithm.RandomTrail(1, numCities)));
        }

        bestTail = Algorithm.BestTrail(ants, wayd);

        Debug.Log(Algorithm.DisplayTail(bestTail));
    }
    public void SpeedUpdate(float value)
    {
        antSpeed = Mathf.RoundToInt(value * 100); ;
        SpeedText.text = antSpeed + "";
    }
    private int curr = 0;
    private bool back = false;
    private void Update()
    {
        if (p.Count == 4)
        {
            if (AntPref.transform.position != p[bestTail[curr] - 1].transform.position)
            {
                AntPref.transform.position = Vector2.MoveTowards(AntPref.transform.position, p[bestTail[curr] - 1].transform.position, antSpeed * Time.deltaTime);
            }
            else
            {
                if (!back)
                    curr = (curr + 1) % p.Count;
                else
                    curr = (curr - 1) % p.Count;
                if (bestTail[curr] == bestTail[bestTail.Length - 1])
                    back = true;
                else if (bestTail[curr] == startPoint)
                {
                    back = false;
                    Algorithm.UpdateAnts(ants, wayd, numCities, startPoint);
                    Algorithm.UpdatePheromones(ants, wayd, numCities);
                    bestTail = Algorithm.BestTrail(ants, wayd);
                    Debug.Log(Algorithm.DisplayTail(bestTail) + " | " + Algorithm.Length(bestTail,wayd));
                }
            }
        }
    }
    private void AddPoint()
    {
        if (currLine == null)
        {
            currLine = Instantiate(LinePref, Vector3.zero, Quaternion.identity, _ClickZone.transform).GetComponent<LineController>();
        }

        p.Add(Instantiate(PointPref, GetMousePos(), Quaternion.identity, _ClickZone.transform).GetComponent<PointController>());
        p[p.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = p.Count + "";

        currLine.AddPoint(p);

    }

    public static Vector3 GetMousePos()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        return worldPos;
    }

}
