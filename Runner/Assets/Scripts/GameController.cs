using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public bool started;
    public int obstacleCount;

    public int obstacleMovingDistance;
    public int obstacleMovingSpeed;
    public bool isOnOfObstaclesMoving;


    [SerializeField] private GameObject canvas;

    private void Start()
    {
        started = false;
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 obstaclePos = new Vector3(Random.Range(-3.5f, 3.5f), 3f, Random.Range(-1f, 70f));

            GameObject obstacle = Instantiate(Resources.LoadAll<GameObject>("")[Random.Range(0, 5)], GameObject.Find("Obstacles").transform, false);
            obstacle.transform.rotation = new Quaternion(Random.Range(0, 361), Random.Range(0, 361), Random.Range(0, 361), Random.Range(0, 361));
            obstacle.transform.position = obstaclePos;
        }
    }


    public void OnRestartClicked()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OpenCanvas()
    {
        canvas.SetActive(true);
    }
}
