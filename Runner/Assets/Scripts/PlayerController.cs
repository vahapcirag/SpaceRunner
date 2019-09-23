using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalRunSpeed;
    public float verticalRunSpeed;
    public float force;

    private float horizontal;

    public bool shielded;

    private Rigidbody rb;
    private Rigidbody cameraRb;
    private Animator anim;

    private GameController gameController;
    private Vector3 fixedDistanceWCameraPlayer;

    [SerializeField] Material shieldMat;
    [SerializeField] Material normalMat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraRb = Camera.main.gameObject.GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        fixedDistanceWCameraPlayer = cameraRb.position - rb.position;
        shielded = false;
    }


    void FixedUpdate()
    {
        if (!gameController.started)
            return;

        if (Input.GetButton("Vertical"))
        {
            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);

        }

        horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 direction = transform.forward * verticalRunSpeed * Time.deltaTime;
        direction.x = horizontal *Time.deltaTime*horizontalRunSpeed;
        transform.position += direction;
    }

    

    private void LateUpdate()
    {
        Vector3 distanceWCameraPlayer = cameraRb.position - rb.position;
        bool distanceMangitude = (distanceWCameraPlayer.magnitude != fixedDistanceWCameraPlayer.magnitude);

        if (distanceMangitude)
        {
            Vector3 lastCamPos = cameraRb.position + fixedDistanceWCameraPlayer - distanceWCameraPlayer;
            Vector3 pos = Vector3.Lerp(cameraRb.position, lastCamPos, 0.1f);
            cameraRb.position= pos;
        }
    }

   

     
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.name == "Obstacles")
        {


            BoxCollider boxCollider = GetComponent<BoxCollider>();


            if (!boxCollider.isTrigger)
            {

                if (!Physics.GetIgnoreLayerCollision(8, 9) && !shielded)
                {
                    anim.SetBool("floating", true);
                    Physics.IgnoreLayerCollision(8, 9, true);

                    verticalRunSpeed /= 2;
                    StartCoroutine(FlashRenderer());
                }
                else if (shielded)
                {
                    shielded = false;
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = normalMat;
                }
            }
        }

        else if (other.tag == "Shield")
        {
            shielded = true;
            transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = shieldMat;
            Destroy(other.gameObject);
        }
        else if (other.name == "TimeStopper")
            Time.timeScale = 0;
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
        verticalRunSpeed *= 2;
        Debug.Log(Physics.GetIgnoreLayerCollision(8, 9));
        Physics.IgnoreLayerCollision(8, 9, false);
        anim.SetBool("floating", false);
        yield return null;
    }

}
