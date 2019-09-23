using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject spawnPoint;

    /*
     * When player collides with this boundary (falls off platforms) respawn them at spawn point location
     */
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.transform.GetComponent<PlayerScript>().isLocalPlayer)
        {
            player.gameObject.transform.position = spawnPoint.transform.position;
        }
    }
}
