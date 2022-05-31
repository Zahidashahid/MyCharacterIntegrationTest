using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointFollwer : MonoBehaviour
{
    public GameObject[] wayPoints;
    int CurrentWayPointIndex = 0;
    public float speed = 1.5f;
    void Update()
    {
        if(Vector2.Distance(wayPoints[CurrentWayPointIndex].transform.position, transform.position) < .1f)
        {
            CurrentWayPointIndex++;
            if (CurrentWayPointIndex >= wayPoints.Length)
                CurrentWayPointIndex = 0;
                
        }
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[CurrentWayPointIndex].transform.position, Time.deltaTime * speed);
    }
}
