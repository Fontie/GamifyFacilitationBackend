using System;
using System.Collections;

using System.Collections.Generic;

using UnityEngine;



[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour

{

    //THESE ARE THE VALUES THAT MATTER FOR PROGRESSION! DONT TOUCH, LEAVE TO MICHAEL

    public string name = "LeeroyJankins"; //player name TODO: let player decide the name for themselves

    public int score = 0; //Shows how many points you have TODO: put this in some progress bar or something

    public int accessLevel = 0; //What decides which levels you are allowed to play. you can assign the needed level in the trigger boxes.

    //THESE ARE VALUES FOR QUALITY OF LIFE

    public float xx = 0; //player x position

    public float yy = 0; //player y position

    public float zz = 0; //player x position


    //THESE ARE THE VALUES FOR MOVEMENT, DONT TOUCH UNLESS YOU KNOW WHAT YOU ARE DOING
    public Camera playerCamera;

    public float walkSpeed = 3f;

    public float runSpeed = 6f;

    public float jumpPower = 7f;

    public float gravity = 10f;

    public float lookSpeed = 2f;

    public float lookXLimit = 45f;

    public float defaultHeight = 0.5f;

    public float crouchHeight = 1f;

    public float crouchSpeed = 1f;



    private Vector3 moveDirection = Vector3.zero;

    private float rotationX = 0;

    private CharacterController characterController;



    private bool canMove = true;



    void Start()

    {

        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

    }



    void Update()

    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);


        // too lazy to remove run function
        bool isRunning = false;//Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;

        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);



        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {

           // moveDirection.y = jumpPower;

        }
        else
        {

            moveDirection.y = movementDirectionY;

        }



        if (!characterController.isGrounded)
        {

            moveDirection.y -= gravity * Time.deltaTime;

        }



        characterController.Move(moveDirection * Time.deltaTime);

        Vector3 position = transform.position;
        xx = position.x;
        yy = position.y;
        zz = position.z;


    }


}