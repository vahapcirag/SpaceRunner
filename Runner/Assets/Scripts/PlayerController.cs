using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalForce;
    public float fixedVerticalRunSpeed;
    public float verticalRunSpeed;
    public float verticalForce;

    public bool shielded;
    [SerializeField] float time;

    public GameObject[] particles;

    private Rigidbody rb;
    private Rigidbody cameraRb;

    public Animator anim;
    private GameController gameController;
    private Vector3 fixedDistanceWCameraPlayer;

    [SerializeField] private Material shieldMat;
    [SerializeField] private Material normalMat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraRb = Camera.main.gameObject.GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        fixedDistanceWCameraPlayer = cameraRb.position - rb.position;
        shielded = false;

        verticalRunSpeed = fixedVerticalRunSpeed;
    }


    void FixedUpdate()
    {
        time += Time.deltaTime;
        // Vector3 horizontal;

        if (!gameController.started)
            return;

        if (GameObject.Find("TimeStopper").transform.position.z <= transform.position.z)
            Time.timeScale = 0;


        if (Input./*GetButton("Vertical")*/touchCount != 0)
        {

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                if (Mathf.Abs(touch.deltaPosition.x) > 1f)
                    StartCoroutine(LeftRight(touch.deltaPosition.x, transform.position.x));
                // rb.AddForce(Mathf.Abs(touch.deltaPosition.x) / touch.deltaPosition.x * Vector3.right * horizontalForce, ForceMode.Acceleration);
            }
            else if (touch.deltaPosition.x < 1f && time >0.3f)
            {
                if (time < 1f)
                {
                    rb.AddForce(Vector3.up * verticalForce, ForceMode.Acceleration);
                    anim.SetBool("floating", true);

                    particles[0].SetActive(true);
                    particles[1].SetActive(true);
                }

            }
        }
        else
        {
            time = 0;
        }
        Vector3 direction = transform.forward * verticalRunSpeed * Time.deltaTime;
        transform.position += direction;


    }


    private void LateUpdate()
    {
        Vector3 distanceWCameraPlayer = cameraRb.position - rb.position;
        bool distanceMangitude = (distanceWCameraPlayer.magnitude != fixedDistanceWCameraPlayer.magnitude);

        if (distanceMangitude)
        {
            Vector3 lastCamPos = cameraRb.position + fixedDistanceWCameraPlayer - distanceWCameraPlayer;
            Vector3 pos = Vector3.Lerp(cameraRb.position, lastCamPos, .1f);
            cameraRb.position = pos;
        }
    }


    private void Update()
    {


        Debug.Log(" Ignoring: " + Physics.GetIgnoreLayerCollision(8, 9));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.name == "Obstacles")
        {
            if (!Physics.GetIgnoreLayerCollision(8, 9) && !shielded)
            {
                string hitBool = "hitted" + Random.Range(1, 3);
                anim.SetBool(hitBool, true);
                Physics.IgnoreLayerCollision(8, 9);
                verticalRunSpeed = 0;
                StartCoroutine(FlashRenderer());
            }
            else if (shielded)
            {
                shielded = false;
                transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = normalMat;
            }
        }

        else if (other.transform.parent != null && other.transform.parent.name == "PlanetSurface")
        {

            anim.SetBool("floating", false);
            particles[0].SetActive(false);
            particles[1].SetActive(false);
        }

        else if (other.tag == "Shield")
        {
            shielded = true;
            transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = shieldMat;
            Destroy(other.gameObject);
        }


    }

    IEnumerator FlashRenderer()
    {
        int i = 0;
        while (i < 100)
        {
            transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().enabled = (!transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().enabled);
            if (i != 3)
                yield return new WaitForSecondsRealtime(.03f);
            i++;
        }

        //Physics.IgnoreLayerCollision(8, 9, false);
        Debug.Log("Fix" + " Ignoring: " + Physics.GetIgnoreLayerCollision(8, 9));

        yield return null;
    }


    IEnumerator LeftRight(float deltaPos, float startX)
    {

        while (true)
        {
            if (Mathf.Abs((transform.position + Vector3.right * 0.1f * Mathf.Sign(deltaPos)).x) < 2.70f)
                transform.position += Vector3.right * 0.02f * Mathf.Sign(deltaPos);
            yield return new WaitForSecondsRealtime(0.01f);
            if (Mathf.Abs(transform.position.x - startX) > 0.15f)
                break;
        }

        yield return null;
    }
}
