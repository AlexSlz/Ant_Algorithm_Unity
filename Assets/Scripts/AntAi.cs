﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class AntAi : MonoBehaviour
{

    int time;

    private int curr = 0;
    private bool back = false;
    int antSpeed;

    List<Ant> ants;
    List<WayData> wayd;
    List<PointController> p;

    private TextMeshProUGUI BestText;

    int numCities;
    int[] bestTail;
    double bestLength;
    int startPoint;

    public void SpeedUpdate(int speed)
    {
        antSpeed = speed;
    }

    public void AddAnt(List<Ant> _ants, List<WayData> _wayd, List<PointController> _p, int num, int[] b, int s, TextMeshProUGUI _BestText)
    {
        time = 500;
        ants = _ants;
        wayd = _wayd;
        p = _p;
        numCities = num;
        bestTail = b;
        startPoint = s;
        bestLength = Algorithm.Length(bestTail, wayd);
        BestText = _BestText;
    }

    private void LateUpdate()
    {
        if (antSpeed < 69)
        {
            if (transform.position != p[bestTail[curr] - 1].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, p[bestTail[curr] - 1].transform.position, antSpeed * Time.deltaTime);
            }
            else
            {
                if (!back)
                    curr = (curr + 1) % p.Count;
                else
                    curr = (curr - 1) % p.Count;
                  //wayd[wayd.IndexOf(wayd.Find(item => item.first == bestTail[curr] && item.second == bestTail[curr + 1]))].lineC.SetColor();
                if (bestTail[curr] == bestTail[bestTail.Length - 1])
                    back = true;
                else if (bestTail[curr] == startPoint)
                {
                    back = false;
                    NextIteration();
                }
            }
        }
        else
        {
            NextIteration();
        }
    }

    private void NextIteration()
    {
        if (time > 0)
        {
            Algorithm.UpdateAnts(ants, wayd, numCities, startPoint);
            Algorithm.UpdatePheromones(ants, wayd, numCities);
            for (int i = 0; i < wayd.Count / 2; i++)
            {
                wayd[i].lineC.SetColor((float)wayd[i].tau);
            }
            int[] currBestTrail = Algorithm.BestTrail(ants, wayd);
            double currBestLength = Algorithm.Length(currBestTrail, wayd);
            if (currBestLength < bestLength || currBestLength == bestLength)
            {
                bestLength = currBestLength;
                bestTail = currBestTrail;
            }
            BestText.text = "Best Way: \n" + Algorithm.DisplayTail(bestTail) + " | " + Algorithm.Length(bestTail, wayd);
            Debug.Log("Best Way: \n" + Algorithm.DisplayTail(currBestTrail) + " | " + Algorithm.Length(currBestTrail, wayd));
            time--;
        }
        else
        {
            for (int i = 0; i < wayd.Count - 1; i++)
            {
                for (int j = 0; j < bestTail.Length - 1; j++)
                {
                    if (wayd[i].first == bestTail[j] && wayd[i].second == bestTail[j + 1])
                    {
                        if (wayd[i].lineC == null)
                            wayd[wayd.IndexOf(wayd.Find(item => item.second == bestTail[j] && item.first == bestTail[j + 1]))].lineC.SetColor(100);
                        else
                            wayd[i].lineC.SetColor(100);
                        break;
                    }
                    else
                    {
                        if (wayd[i].lineC != null)
                            wayd[i].lineC.SetColor(0);
                    }
                }
            }
        }
    }
}