using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloPlayerScript : MonoBehaviour
{
    CharacterController2D PlayerController;
    Vector3 direction;
    public GameObject floor;
    public bool canMoveLeft = true;
    public bool canMoveRight = true;

    //Set Player game object on start
    void Start()
    {
        PlayerController = gameObject.GetComponent<CharacterController2D>();
    }

    /* 
     * Move player based on key inputs
     * 'up', 'w', or 'space' allows character to jump
     * 'left' directional key or 'a' allows character to move left
     * 'right' directional key or 'd' allows the character to move right
     * 
     * Unity Documentation was utilized and modified for this project : https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
     */
    void Update()
    {
        direction = new Vector3(0.0f, 0.0f, 0.0f);

            if (gameObject.transform.GetComponent<CapsuleCollider2D>().IsTouching(floor.transform.GetComponent<CompositeCollider2D>()))
                {
                if (Input.GetKey("up") || Input.GetKey("w") || Input.GetKey("space"))
                   {
                    direction.y += 50.05f;
                   }
            }
            else
            {
                direction.y -= 230.8f * Time.deltaTime;
            }
                if (Input.GetKey("left") || Input.GetKey("a") && canMoveLeft)
                   {
                      direction.x -= 3.05f;
                   }
                else if (Input.GetKey("right") || Input.GetKey("d") && canMoveRight)
                   {
                      direction.x += 3.05f;
                   }

         PlayerController.Move(direction * Time.deltaTime);

    }

}
