using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AntAi : MonoBehaviour
{

    int time;

    private int curr = 0;
    private bool back = false;
    int antSpeed = 1;

    private bool DeleteWrongWay = true;


    List<Ant> ants;
    List<WayData> wayd;
    List<PointController> p;

    private TextMeshProUGUI BestText;

    [SerializeField] private Image AntVisual;
    [SerializeField] private SettingsSet _settings;


    int numCities;
    int[] bestTail;
    double bestLength;


    public void SpeedUpdate(int speed)
    {
        antSpeed = speed;
    }

    public void AddAnt(List<Ant> _ants, List<WayData> _wayd, List<PointController> _p, int num, int[] b, TextMeshProUGUI _BestText, SettingsSet set)
    {
        time = _p.Count * 100;
        ants = _ants;
        wayd = _wayd;
        p = _p;
        numCities = num;
        bestTail = b;
        bestLength = Algorithm.Length(bestTail, wayd);
        BestText = _BestText;
        _settings = set;
    }

    private void LateUpdate()
    {
        if (antSpeed < Main.antMaxSpeed)
        {
            if (transform.position != p[bestTail[curr] - 1].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, p[bestTail[curr] - 1].transform.position, antSpeed * Time.deltaTime);
                Vector2 _dir = new Vector2(p[bestTail[curr] - 1].transform.position.x - transform.position.x, p[bestTail[curr] - 1].transform.position.y - transform.position.y);
                transform.up = _dir;
            }
            else
            {
                AntVisual.enabled = true;
                if (!back)
                    curr = (curr + 1) % p.Count;
                else
                    curr = (curr - 1) % p.Count;
                if (bestTail[curr] == bestTail[bestTail.Length - 1])
                    back = true;
                else if (bestTail[curr] == Main.startPoint)
                {
                    back = false;
                    NextIteration();
                }
            }
        }
        else
        {
            NextIteration();
            AntVisual.enabled = false;
        }
    }

    private void NextIteration()
    {
        if (time > 0)
        {
            Algorithm.UpdateAnts(ants, wayd, numCities, Main.startPoint);
            Algorithm.UpdatePheromones(ants, wayd, numCities);
            for (int i = 0; i < wayd.Count / 2; i++)
            {
                wayd[i].lineC.SetColor((float)wayd[i].tau * 2, true);
            }
            int[] currBestTrail = Algorithm.BestTrail(ants, wayd);
            double currBestLength = Algorithm.Length(currBestTrail, wayd);
            if (currBestLength < bestLength || currBestLength == bestLength)
            {
                bestLength = currBestLength;
                bestTail = currBestTrail;
            }
            BestText.text = "Текущий путь: \n" + Algorithm.DisplayTail(currBestTrail) + " | " + Algorithm.Length(currBestTrail, wayd);
            Debug.Log("Best Way: \n" + Algorithm.DisplayTail(currBestTrail) + " | " + Algorithm.Length(currBestTrail, wayd));
            time--;
        }
        else
        {
            BestText.text = "Лучший путь: \n" + Algorithm.DisplayTail(bestTail) + " | " + Algorithm.Length(bestTail, wayd);
            List<int> knowWay = new List<int>();

            for (int i = 0; i < bestTail.Length - 1; i++)
            {
                for (int j = 0; j < wayd.Count - 1; j++)
                {
                    if (bestTail[i] == wayd[j].first && bestTail[i + 1] == wayd[j].second)
                    {
                        wayd[j].lineC.visible = true;
                        break;
                    }else if (bestTail[i] == wayd[j].second && bestTail[i + 1] == wayd[j].first)
                    {
                        wayd.Find(item => item.first == bestTail[i] && item.second == bestTail[i + 1]).lineC.visible = true;
                        break;
                    }
                }
            }

            DeleteWrongWay = _settings.Toggle_Del.isOn;

            for (int i = 0; i < wayd.Count - 1; i++)
            {
                if (!wayd[i].lineC.visible)
                {
                    wayd[i].lineC.SetColor(0, !DeleteWrongWay);
                }
                else
                {
                    wayd[i].lineC.SetColor(100, true);
                }
            }
        }
    }
}
