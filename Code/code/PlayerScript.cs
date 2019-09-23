using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerScript : NetworkBehaviour
{
    CharacterController PlayerController;
    float movementSpeed = 2.0f;
    Vector3 moveDirection;
    [SyncVar] public int treasureCount = 8;
    private bool check = false;
    
    //sets player controller and initial treasure count
    void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        treasureCount = 8;
    }

    /*
    * Client objects send information to Server object
    * Updating treasure count for all player objects
    * Destroys in game object that has the Network Instance ID as a variable passed through the command
    * Checks if scene change is necessary. This happens when there are no more treasures left in the game and goes to the results scene.
    */
    [Command]
    public void CmdDestroyObject(NetworkInstanceId g, NetworkInstanceId treasureGameObject)
    {
        GameObject gObject;
        if (isClient)
        {
            gObject = ClientScene.FindLocalObject(g);
            GameObject treasureCountGameObject = ClientScene.FindLocalObject(treasureGameObject);
            treasureCountGameObject.transform.GetComponent<StillPlayingCheck>().setTreasuresLeft((treasureCountGameObject.transform.GetComponent<StillPlayingCheck>().getTreasuresLeft() - 1));
            CmdChangeTreasure(treasureCountGameObject.transform.GetComponent<StillPlayingCheck>().getTreasuresLeft());
        }
        else
        {
            gObject = NetworkServer.FindLocalObject(g);
            GameObject treasureCountGameObject = NetworkServer.FindLocalObject(treasureGameObject);
            treasureCountGameObject.transform.GetComponent<StillPlayingCheck>().setTreasuresLeft((treasureCountGameObject.transform.GetComponent<StillPlayingCheck>().getTreasuresLeft() - 1));
            CmdChangeTreasure(treasureCountGameObject.transform.GetComponent<StillPlayingCheck>().getTreasuresLeft());
        }
        Destroy(gObject);  
     if (treasureCount <= 0)
        {
          GameObject.Find("LobbyManager").transform.GetComponent<NetworkLobbyManager>().ServerChangeScene("Results");

        }
    }

    /*
     * Client objects send information to Server object
     * Updating treasure count for all player objects
     */
    [Command]
    void CmdChangeTreasure(int treas)
    {
        treasureCount = treas;
        RpcchangeTreasure(treas);
    }

    /*
     * Server objects send information to Client objects
     * Updating treasure count for all player objects
     */
    [ClientRpc]
    void RpcchangeTreasure(int treasureC)
    {
        treasureCount = treasureC;
      //  Debug.LogError(treasureCount);
    }

    //set treasure count - 1
    public void decreaseTreasureCount()
    {
        this.check = true;
        CmdChangeTreasure((treasureCount - 1));
        this.check = false;
    }

    /* 
     * Move player based on key inputs
     * 'up', 'w', or 'space' allows character to jump
     * 'up', 'w' allows character to go up ladder
     * 'down', 's' allows character to go down ladder
     * 'left' directional key or 'a' allows character to move left
     * 'right' directional key or 'd' allows the character to move right
     * 
     * Unity Documentation was utilized and modified for this project : https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
     */
    void Update()
    {
        if (this.isLocalPlayer)
        {
            if (PlayerController.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * movementSpeed;

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = 8.0f;
                }
            }
            if (GameManager.instance.isLadder == true)
            {
                if (Input.GetKey("up") || Input.GetKey("w"))
                {
                    moveDirection.y += 0.05f;
                }
                else if (Input.GetKey("down") || Input.GetKey("s"))
                {
                    moveDirection.y -= 0.05f;
                }
                if (Input.GetKey("left") || Input.GetKey("a"))
                {
                    moveDirection.x -= 0.05f;
                }
                else if (Input.GetKey("right") || Input.GetKey("d"))
                {
                    moveDirection.x += 0.05f;
                }
            }
            if (GameManager.instance.isLadder == true)
            {
                moveDirection.y -= 0.0f * Time.deltaTime;
            }
            else
            {
                moveDirection.y -= 20.0f * Time.deltaTime;
            }


            PlayerController.Move(moveDirection * Time.deltaTime);

        }
    }

}
