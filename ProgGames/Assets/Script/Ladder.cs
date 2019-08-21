using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    Collider ladderCollider;
    // Start is called before the first frame update
    void Start()
    {
        ladderCollider = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player)
        {
            Debug.Log("Enter Ladder");
            GameManager.instance.isLadder = true;
        }
        else
        {
            GameManager.instance.isLadder = false;
        }
    }
    private void OnTriggerExit(Collider player)
    {
        if (player)
        {
            Debug.Log("Exit");
            GameManager.instance.isLadder = false;
        }
    }
}
