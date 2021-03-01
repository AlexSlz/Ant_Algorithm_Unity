using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAi : MonoBehaviour
{
    private int curr = 0;
    private bool back = false;
    int antSpeed;

    List<Ant> ants;
    List<WayData> wayd;
    List<PointController> p;

    int numCities;
    int[] bestTail;
    int startPoint;

    public void SpeedUpdate(int speed)
    {
        antSpeed = speed;
    }

    public void AddAnt(List<Ant> _ants, List<WayData> _wayd, List<PointController> _p, int num, int[] b, int s)
    {
        ants = _ants;
        wayd = _wayd;
        p = _p;
        numCities = num;
        bestTail = b;
        startPoint = s;
    }

    private void LateUpdate()
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
            if (bestTail[curr] == bestTail[bestTail.Length - 1])
                back = true;
            else if (bestTail[curr] == startPoint)
            {
                back = false;
                Algorithm.UpdateAnts(ants, wayd, numCities, startPoint);
                Algorithm.UpdatePheromones(ants, wayd, numCities);
                bestTail = Algorithm.BestTrail(ants, wayd);
                Debug.Log(Algorithm.DisplayTail(bestTail) + " | " + Algorithm.Length(bestTail, wayd));
            }
        }
    }
}
