using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

    public float moveSpeed = 5;
    public float jumpForce = 10;
    CharacterController controller;

    [SerializeField]
    private float _gravityScale;

    private Vector3 moveVelocity;

	void Start () {
        //controller = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
	}
	

	void Update () {

        //Get Player input from Horizontal/Vertical axes
        //moveVelocity = new Vector3( Input.GetAxisRaw("Horizontal") * moveSpeed, moveVelocity.y, Input.GetAxisRaw("Vertical") * moveSpeed);
        float yStore = moveVelocity.y;
        moveVelocity = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveVelocity = moveVelocity.normalized * moveSpeed;
        moveVelocity.y = yStore;


        //Check if player is grounded
        if (controller.isGrounded)
        {
            moveVelocity.y = 0;
            //If "Jump" is pressed, jump
            if (Input.GetButtonDown("Jump"))
            {
                moveVelocity.y = jumpForce;
            }
        }
        //Deduct gravity from upwards momentum every frame
        moveVelocity.y += Physics.gravity.y * _gravityScale * Time.deltaTime; //TODO: Calculate gravity off the wanted jumptime

        //Move the player
        controller.Move(moveVelocity*Time.deltaTime);
    }
}
