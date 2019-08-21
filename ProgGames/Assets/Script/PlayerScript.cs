using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerScript : NetworkBehaviour
{
    CharacterController PlayerController;
    Animator PlayerAnimator;
    public Animator runAnimation;
    public Animator idleAnimation;
    float movementSpeed = 2.0f;
    Vector3 moveDirection;
    [SyncVar] public int treasureCount = 6;
    private bool check = false;
    

    // Start is called before the first frame update
    void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        PlayerAnimator = GetComponent<Animator>();
        treasureCount = 6;
    }
/*    https://forum.unity.com/threads/command-object-references.334755/ */

    [Command]
    public void CmdDestroyObject(NetworkInstanceId g)
    {
        GameObject gObject;
        if (isClient)
        {
            gObject = ClientScene.FindLocalObject(g);
        }
        else
        {
            gObject = NetworkServer.FindLocalObject(g);
        }
        Destroy(gObject);
     
        //  gObject.SetActive(false);
        //  NetworkServer.UnSpawn(gObject);    
       if (treasureCount <= 0)
        {
          GameObject.Find("LobbyManager").transform.GetComponent<NetworkLobbyManager>().ServerChangeScene("Results");

        }
    }

    [Command]
    void CmdChangeTreasure(int treas)
    {
        treasureCount = treas;
       // Debug.LogError(treasureCount);
        RpcchangeTreasure(treas);
    }

    [ClientRpc]
    void RpcchangeTreasure(int treasureC)
    {
        treasureCount = treasureC;
        Debug.LogError(treasureCount);
    }

    public void decreaseTreasureCount()
    {
        this.check = true;
        CmdChangeTreasure((treasureCount - 1));
        this.check = false;
    }

    private void FixedUpdate(){}

    // Update is called once per frame
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
