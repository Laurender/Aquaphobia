using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour{

    [SerializeField]
    public LayerMask collisionMask;

    //MovementSpeed
    [SerializeField]
    private float moveSpeed = 5;

    //Jumping stats
    [SerializeField]
    [Tooltip("Height of jump")]
    private float jumpHeight = 2f;
    [SerializeField]
    [Tooltip("How long it takes to reach jump height")]
    private float timeToApex = 1f;

    private Vector3 gravity;

    float storeY;
    private float _slideMultiplier = 1;
    [SerializeField][Range(1, 2f)][Tooltip("How much gravity increases per midair frame")]
    private float _gravityMultiplier = 1.02f;

    private bool isJumping;
    private bool onSlope; // is on a slope or not
    private bool slidingSlope; // is on a slope thats too steep
    public float slideSpeed = 0.3f; // slope slide speed
    public float slopeLimit = 50; //Slope limit
    private float _slopeAngle;
    private Vector3 hitNormal; //orientation of the slope.

    private RaycastHit _hit;

    public Vector3 moveVelocity;

    public CharacterController Controller { get; set; }
    private RaycastController rayController;

    [SerializeField]
    private float _gravityScale;

    void Awake()
    {
        gravity.y = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        Debug.Log("Gravity: " + gravity.y);
        Controller = GetComponent<CharacterController>();
        rayController = GetComponent<RaycastController>();
    }

    void Start () {
	}

    public bool IsGrounded()
    { 
        return rayController.RaycastGrid();
    }
    
    void Update()
    {
        Vector3 input = Vector3.zero;
        input = MoveInput();       
        _hit = rayController.RaycastGridHit();
        hitNormal = rayController.RaycastGridHit().normal;
        if (hitNormal != Vector3.zero) _slopeAngle = Vector3.Angle(Vector3.up, hitNormal);

        input = HandleSlopes(input, _slopeAngle);
        Jump();

        moveVelocity += gravity * Time.deltaTime * _gravityScale;
        Controller.Move((input+moveVelocity) * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {

        gravity.y = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        var curPos = transform.position + -Vector3.up;
        var jump = jumpHeight;
        var steps = 60*timeToApex;
        var gravityScale = 1f;
        RaycastHit hit;
        bool b_hit = false;
        for (int i = 0; i < steps; i++)
        {
            var nextPos = curPos + transform.forward * moveSpeed / steps; //Forward movement
            
            jump  += (gravity.y ) / steps * gravityScale;
            if(Mathf.Sign(jump)==-1) gravityScale *= _gravityMultiplier;
            nextPos += new Vector3(0, jump/steps , 0);
            Gizmos.color = (!b_hit) ? Color.blue: Color.gray;            
            Gizmos.DrawLine(curPos, nextPos);            
            if (Physics.Raycast(curPos, nextPos - curPos, out hit, Vector3.Distance(curPos, nextPos), collisionMask))
            {
                Gizmos.color = (!b_hit) ? Color.magenta : Color.gray;
                Gizmos.DrawWireSphere(hit.point, 0.2f);
                b_hit = true;
            }
            else
            {
                if (i >= steps - 1 && steps<120*timeToApex)
                {
                    steps++;
                }
            }
            curPos = nextPos;
        }
    }
    public Vector3 MoveInput()
    {
        
        Vector3 moveInput = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveInput = moveInput.normalized * moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) moveInput *= 0.5f;
        Vector3 _moveVelocity = new Vector3(0,moveInput.y,0);

        if (!slidingSlope) _moveVelocity = moveInput;
        return _moveVelocity;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            moveVelocity.y = 0;
            _gravityScale = 1;
            isJumping = false;
            if (Input.GetButtonDown("Jump") && !slidingSlope)
            {
                isJumping = true;               
                moveVelocity.y = jumpHeight;
                //Debug.Log("jump");
                
            }
        }
        else
        {
            if(Mathf.Sign(moveVelocity.y)==-1) _gravityScale *= _gravityMultiplier;
            hitNormal = Vector3.zero;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
    }

    private Vector3 HandleSlopes(Vector3 movement, float slopeAngle)
    {
        Vector2 isMoving = new Vector2(movement.x, movement.z);
        onSlope = (Mathf.Abs(slopeAngle)>5f && slopeAngle<89);
        slidingSlope = (slopeAngle >= slopeLimit);

        Vector3 rayOrigin = transform.position + transform.forward * 0.55f;
  
        //Character on slopes
        if (onSlope && !isJumping && isMoving != Vector2.zero) {
            bool ascendSlope, descendSlope; //Determine if character is going up or down a slope.
            if (Physics.Raycast(rayOrigin, Vector3.down, 1f, collisionMask))
            {
                descendSlope = false;
                ascendSlope = true;
                //Debug.Log("ascend");
            }
            else
            {
                descendSlope = true;
                ascendSlope = false;
                //Debug.Log("descend");
            }           

            if (descendSlope && !slidingSlope)
            {                
                float descend_moveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Vector3.Distance(transform.position, movement);
                movement = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * new Vector3(movement.x, 0, movement.z);
                movement.y -= descend_moveAmountY;
            }

            /* //paska ei toimi          
            if (ascendSlope && !slidingSlope) 
            {
                float moveDistance = Mathf.Abs(Vector3.Distance(transform.position, movement));
                float climb_moveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                movement.y = climb_moveAmountY;
            }
            */
        }
        //Lose control on slopes too steep
        else if(slidingSlope&&Mathf.Sign(hitNormal.y)!=-1)
        {
            Debug.Log("Sliding");
            movement.x += (1f - hitNormal.y) * hitNormal.x * slideSpeed * _slideMultiplier;
            movement.z += (1f - hitNormal.y) * hitNormal.z * slideSpeed * _slideMultiplier;

            float descend_moveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * Vector3.Distance(transform.position, movement);
            movement.y -= descend_moveAmountY;
            _slideMultiplier *= 1.1f;
        }
        else
        {
            //Reset slidespeed multiplier
            _slideMultiplier = 1;
        }
        //Debug.DrawLine(transform.position, transform.position + movement, Color.cyan);
        return movement;
    }
    
}