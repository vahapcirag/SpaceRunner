using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{

    PlayerController playerController;

    private void Start()
    {
        playerController= GetComponentInParent<PlayerController>();
    }

    void StartGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().started = true;

        Invoke("FixCollision", 1.5f);

        PlayerController playerController = GetComponentInParent<PlayerController>();

        playerController.verticalRunSpeed = playerController.fixedVerticalRunSpeed;
    }

    void endOfJump()
    {
        playerController.anim.SetBool("hitted1", false);
        playerController.anim.SetBool("hitted2", false);
        
    }


    void FixCollision()
    {
        Physics.IgnoreLayerCollision(8, 9,false);
    }
}