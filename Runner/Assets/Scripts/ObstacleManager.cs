using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    byte random;
    [SerializeField] private GameObject player;
    void Start()
    {
        random = (byte)Random.Range(0, 20);
        if (random == 1)
            InvokeRepeating("Func", 0, Random.Range(3, 5));
    }


    void Func()
    {
        if ((transform.position - player.transform.position).magnitude < 4 && (transform.position.z > player.transform.position.z))
            GetComponent<Rigidbody>().AddForce(-(transform.position  - player.transform.position) * 300 ,ForceMode.Acceleration);
    }

    
}
