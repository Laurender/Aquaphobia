using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

    //MovementSpeed
    public float moveSpeed = 5;

    //Jumping stats
    [SerializeField]
    [Tooltip("Height of jump")]
    private float jumpHeight = 2f;
    [SerializeField]
    [Tooltip("How long it takes to reach jump height")]
    private float timeToApex = 1f;

    private float jumpTimer = 0.0f;
    Vector3 gravity;
    [SerializeField]
    private bool isJumping = false;

    float storeY;
    private float _slideMultiplier = 1;

    Vector3 moveVelocity;

    private bool onSlope; // is on a slope or not
    private bool slidingSlope; // is on a slope thats too steep
    public float slideSpeed = 0.3f; // slope slide speed
    public float slopeLimit = 50; //Slope limit
    [SerializeField]
    private float _slopeAngle;
    private Vector3 hitNormal; //orientation of the slope.


    CharacterController controller;

    [SerializeField]
    private float _gravityScale;

    void Awake()
    {
        gravity.y = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
    }

    void Start () {
        controller = GetComponent<CharacterController>();
	}

 /*   Vector3 Gravity
    {

        get
        {

            Ray ray = new Ray();
            ray.origin = this.transform.position;
            ray.direction = Vector3.down;

            //On floor
            if (Physics.Raycast(ray, this.controller.height * 0.55f))
            {
                isJumping = false;
                //Jump
                if (Input.GetKey(KeyCode.Space))
                {
                    jumpTimer = timeToApex;
                    isJumping = true;
                } 
                
            }

            //Jump time
            if (jumpTimer > 0f)
            {
                isJumping = true;
                float height = jumpCurve.Evaluate((timeToApex - jumpTimer) / timeToApex); //Percentage of elapsed jump
                this.jumpTimer -= Time.deltaTime;
                var _gravity = Vector3.up * (jumpCurve.Evaluate((timeToApex - jumpTimer) / timeToApex) - height) * jumpHeight;
                return (_gravity);
            }
            else
            {
                isJumping = false;
            }
            return gravity * Time.deltaTime;

        }

    }
    */

/*	void Update () {
        storeY = transform.position.y;
        //Get Player input from Horizontal/Vertical axes
        Vector3 moveInput = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveInput = moveInput.normalized * moveSpeed;

        var moveVelocity = (Gravity + moveInput * Time.deltaTime);
        Debug.Log(moveVelocity.y);
        controller.Move(moveVelocity);
    }
*/

    void Update()
    {
        Vector3 input = Vector3.zero;
        input = MoveInput();
        
        if (hitNormal != Vector3.zero) _slopeAngle = Vector3.Angle(Vector3.up, hitNormal);
        if (!controller.isGrounded) hitNormal = Vector3.zero;
        input = HandleSlopes(input, _slopeAngle); //get movement input from method and send it to handleslopes method. man fuck this code

        storeY = transform.position.y;
        moveVelocity += gravity * Time.deltaTime;// * _gravityScale;
        controller.Move((input + moveVelocity) * Time.deltaTime);
    }

    public Vector3 MoveInput()
    {
        
        Vector3 moveInput = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveInput = moveInput.normalized * moveSpeed;
        Vector3 _moveVelocity = new Vector3(0,moveInput.y,0);

        if (controller.isGrounded || controller.collisionFlags == CollisionFlags.CollidedBelow)
            {
            moveVelocity.y = 0;
            if (Input.GetKeyDown(KeyCode.Space))
                {
                moveVelocity.y = jumpHeight;
                hitNormal = Vector3.zero;
                Debug.Log("jump");
            }
        }
        if (!slidingSlope) _moveVelocity = moveInput;
        return _moveVelocity;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private Vector3 HandleSlopes(Vector3 movement, float slopeAngle)
    {
        onSlope = (Mathf.Abs(slopeAngle)>5f&&controller.isGrounded);
        Debug.Log(onSlope);
        slidingSlope = (slopeAngle >= slopeLimit);

        //Character sliding of surfaces
        if (onSlope) {
            
            bool ascendSlope, descendSlope;
            if (storeY < transform.position.y)
            {
                descendSlope = false; ascendSlope = true;
            }
            if(storeY > transform.position.y)
            {
                descendSlope = true; ascendSlope = true;
            }
            else
            {
                descendSlope = false; ascendSlope = false;
            }
            if (slidingSlope)
            {
                movement.x += (1f - hitNormal.y) * hitNormal.x * slideSpeed * _slideMultiplier;
                movement.z += (1f - hitNormal.y) * hitNormal.z * slideSpeed * _slideMultiplier;
                _slideMultiplier *= 1.1f;
            }           
            if (descendSlope)
            {
                float descend_moveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Vector3.Distance(transform.position, movement);
                movement = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * new Vector3(movement.x, 0, movement.z);
                movement.y -= descend_moveAmountY;
            }
            if (ascendSlope)
            {
                float moveDistance = Mathf.Abs(Vector3.Distance(transform.position, movement));
                float climb_moveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                //movement.y = climb_moveAmountY;
                //_moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance;
            }
        }
        else
        {
            _slideMultiplier = 1;
        }
        
        return movement;
    }
    private void OnDrawGizmos()
    {
        
    }
}
