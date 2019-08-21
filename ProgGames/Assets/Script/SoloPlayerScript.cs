using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloPlayerScript : MonoBehaviour
{
    CharacterController PlayerController;
    Animator PlayerAnimator;
    public Animator runAnimation;
    public Animator idleAnimation;
    float movementSpeed = 100.0f;
    Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
            if (PlayerController.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * movementSpeed;

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = 100.0f;
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
                moveDirection.y -= 100.0f * Time.deltaTime;
            }


            PlayerController.Move(moveDirection * Time.deltaTime);

        }

}
