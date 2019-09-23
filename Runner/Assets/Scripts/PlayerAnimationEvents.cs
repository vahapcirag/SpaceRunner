using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    
    void StartGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().started=true;

        foreach (var item in GameObject.FindGameObjectsWithTag("Opponent"))
        {
            Animator anim = item.transform.GetChild(0).GetComponent<Animator>();
            anim.SetBool("started", true);
        }
    }
}
