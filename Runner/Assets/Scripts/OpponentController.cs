using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentController : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject closestObstacle;

    [SerializeField] private List<GameObject> otherObstacles = new List<GameObject>();

    private Rigidbody rb;

    private GameController gameController;

    private Vector3 path;
    [SerializeField] private Vector3 beforeJump = Vector3.zero;

    private Animator anim;

    public float horizontalRunSpeed;
    public float verticalRunSpeed;
    public float force;

    [SerializeField] private bool changingPosition = false;
    [SerializeField] private bool changingPositionStarted = false;

    private bool finded = false;

    [SerializeField] float refreshRate;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        InvokeRepeating("FindTheClosestObstacle", 0, refreshRate);
    }

    private void FixedUpdate()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        if (!gameController.started)
            return;

        Vector3 direction = transform.forward * verticalRunSpeed * Time.deltaTime;
        transform.position += direction;

        if (!finded)
            return;

        if (transform.position.z > closestObstacle.transform.position.z)
            changingPosition = false;

        else if ((!changingPosition || Mathf.Abs(closestObstacle.transform.position.z - transform.position.z) > 3 || Mathf.Abs(closestObstacle.transform.position.x - transform.position.x) > .35f))
            return;

        path = Vector3.zero;
        switch (otherObstacles.Count)
        {
            case 0:
                path.x = Mathf.Sign(transform.position.x - closestObstacle.transform.position.x) * 0.01f;
                break;

            case 1:
                path.x = Mathf.Sign(transform.position.x - ((closestObstacle.transform.position.x + otherObstacles[0].transform.position.x) / 2)) * 0.01f;
                break;

            default:

                if (transform.position.y - closestObstacle.transform.position.y < .3f)
                    rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
                break;
        }

        if (!changingPositionStarted)
            StartCoroutine(ChancingPositionX(path));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.name == "Obstacles")
        {
            string hitBool = "hitted" + Random.Range(0, 2).ToString();
            anim.SetBool(hitBool, true);


            if (!Physics.GetIgnoreLayerCollision(int.Parse(gameObject.name[gameObject.name.Length - 1].ToString()) + 9, 8))
            {
                Physics.IgnoreLayerCollision(int.Parse(gameObject.name[gameObject.name.Length - 1].ToString()) + 9, 8, true);
                verticalRunSpeed /= 2;
                StartCoroutine(FlashRenderer(hitBool));
            }
        }
    }

    IEnumerator FlashRenderer(string hitBool)
    {
        int i = 0;
        while (i < 100)
        {
            transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().enabled = (!transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().enabled);
            if (i != 3)
                yield return new WaitForSecondsRealtime(.03f);
            i++;
        }
        verticalRunSpeed *= 2;

        Physics.IgnoreLayerCollision(int.Parse(gameObject.name[gameObject.name.Length - 1].ToString()) + 9, 8, false);
        anim.SetBool(hitBool, false);
        yield return null;
    }


    IEnumerator ChancingPositionX(Vector3 path)
    {
        changingPositionStarted = true;

        while (true)
        {
            if (Mathf.Abs(transform.position.x - closestObstacle.transform.position.x) <= 0.25f)
                transform.position += path;

            yield return new WaitForSecondsRealtime(0.0001f);

            if (transform.position.z - closestObstacle.transform.position.z > 0.1f)
            {
                if (otherObstacles.Count >= 2)
                {
                    rb.AddForce(Vector3.down * force, ForceMode.Acceleration);

                }


                changingPosition = false;
                break;
            }

        }
        changingPositionStarted = false;
        yield return null;
    }


    void FindTheClosestObstacle()
    {
        finded = true;

        if (changingPosition)
            return;

        otherObstacles = new List<GameObject>();
        for (int i = 0; i < obstacles.Length; i++)
        {

            if (i == 0 && obstacles[0].transform.position.z >= transform.position.z)
            {
                closestObstacle = obstacles[0];
            }
            else if (obstacles[i].transform.position.z >= transform.position.z &&
                (Mathf.Abs((transform.position - closestObstacle.transform.position).magnitude) > Mathf.Abs((transform.position - obstacles[i].transform.position).magnitude)))
            {

                this.closestObstacle = obstacles[i];
            }
        }



        for (int i = 0; i < obstacles.Length; i++)
        {
            if (obstacles[i] != closestObstacle)
            {
                if ((closestObstacle.transform.position - obstacles[i].transform.position).magnitude < 1)
                {
                    otherObstacles.Add(obstacles[i]);
                }
            }
        }

        if (closestObstacle.transform.position.z - transform.position.z < 2 && closestObstacle.transform.position.z - transform.position.z > 0)
        {
            changingPosition = true;
            beforeJump = transform.position;
        }


    }
}




