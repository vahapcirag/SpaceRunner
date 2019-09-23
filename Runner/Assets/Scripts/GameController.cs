using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool started;
    public int obstacleCount;

    private void Start()
    {
        started = false;

        //for (int i = 0; i < obstacleCount; i++)
        //{
        //    Vector3 obstaclePos = new Vector3(Random.Range(-10f, 10f), 2f, Random.Range(-1f, 70f));

        //    GameObject obstacle = Instantiate(Resources.LoadAll<GameObject>("")[Random.Range(0, 5)], GameObject.Find("Obstacles").transform, false);
        //    obstacle.transform.position = obstaclePos;
        //}

        
    }
}
