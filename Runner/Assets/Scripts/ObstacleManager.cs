using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    byte random;
    bool isMoved;
    bool a;
    GameController gameController;

    [SerializeField] private GameObject player;
    void Start()
    {
        random = (byte)Random.Range(0, 2);
        if (random != 1)
            return;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        player = GameObject.Find("Player");
       InvokeRepeating("Func", 0, Random.Range(.1f, .3f));
        StartCoroutine(Fall());
    }

   IEnumerator Fall()
    {
      
        while (true)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
            {
                if (hit.collider != null && (hit.collider.gameObject.layer == 18))
                {
                    GetComponent<Rigidbody>().useGravity = false; Debug.Log("sdaasd");
                    break;
                }
            }
            Debug.Log(34);
            
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return null;
    }

    void Func()
    {
        //Debug.Log("gcs:" +gameController.started);
        if (!isMoved&&!gameController.isOnOfObstaclesMoving&& gameController.started && (transform.position.z - player.transform.position.z)> 0.3f && (transform.position.z - player.transform.position.z) < gameController.obstacleMovingDistance)
        {
            isMoved = true;
            Vector3 path = transform.position - player.transform.position;
            path.x = 0f;
            path.x *= 1.5f;
            GetComponent<Rigidbody>().velocity = -path.normalized * gameController.obstacleMovingSpeed * Time.deltaTime;
           
            gameController.isOnOfObstaclesMoving = true;
        }

        else if((transform.position.z - player.transform.position.z)<0.3f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            gameController.isOnOfObstaclesMoving = false;
        }
    }

    
}
