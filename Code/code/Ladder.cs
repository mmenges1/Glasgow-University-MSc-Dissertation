using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    Collider ladderCollider;
    //Set ladder component to game object this script is attached to
    void Start()
    {
        ladderCollider = gameObject.GetComponent<Collider>();
    }
    /*
     * On player enter ladder collider toggle GameManager's isLadder boolean
     */
    private void OnTriggerEnter(Collider player)
    {
        if (player)
        {
            GameManager.instance.isLadder = true;
        }
        else
        {
            GameManager.instance.isLadder = false;
        }
    }
    /*
     * On player enter ladder collider toggle GameManager's isLadder boolean
     */
    private void OnTriggerExit(Collider player)
    {
        if (player)
        {
            GameManager.instance.isLadder = false;
        }
    }
}
