using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAnimationEvents : MonoBehaviour
{

    OpponentController opponentController;

    private void Start()
    {
        opponentController= GetComponentInParent<OpponentController>();
    }

   
    void endOfJump()
    {
        opponentController.anim.SetBool("hitted1", false);
        opponentController.anim.SetBool("hitted2", false);
        
    }

    void StartGame()
    {
        opponentController.verticalRunSpeed = opponentController.fixedVerticalRunSpeed;
        Invoke("FixCollision", 1.5f);
    }

    void FixCollision()
    {
        Physics.IgnoreLayerCollision(8, transform.parent.gameObject.layer,false);
    }
}