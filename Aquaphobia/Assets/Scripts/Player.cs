using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour{

    [SerializeField]
    public LayerMask collisionMask;

    enum MovementType { Basic, Hangable}
    [SerializeField]
    private MovementType currentType;

    private float typeChangeTimer = 0f;


    //MovementSpeed
    [SerializeField]
    protected float moveSpeed = 5;

    //Jumping stats
    [SerializeField]
    [Tooltip("Height of jump")]
    protected float jumpHeight = 2f;
    [SerializeField]
    [Tooltip("How long it takes to reach jump height")]
    protected float timeToApex = 1f;
    protected Vector3 gravity;


    [SerializeField][Range(1, 2f)][Tooltip("How much gravity increases per midair frame")]
    protected float _gravityMultiplier = 1.02f;

    [SerializeField]
    protected float slideSpeed = 0.3f; // slope slide speed
    [SerializeField]
    protected float slopeLimit = 50; //Slope limit

    protected RaycastHit _hit;

    public CharacterController Controller { get; set; }
    protected RaycastController rayController;

    [SerializeField]
    protected float _gravityScale;

    void Awake()
    {
        gravity.y = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        Debug.Log("Gravity: " + gravity.y);
        Controller = GetComponent<CharacterController>();
        rayController = GetComponent<RaycastController>();       
    }

    void Start ()
    {
        currentType = MovementType.Basic;
    }

    public bool IsGrounded()
    { 
        return rayController.IsGrounded;
    }
    
    void Update()
    {
        if (typeChangeTimer > 0)
        {
            Debug.Log(typeChangeTimer);
            typeChangeTimer += -Time.deltaTime;            
        }
        else
        {
            typeChangeTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentType == MovementType.Basic && typeChangeTimer <= 0)
            {
                typeChangeTimer = 0.5f;
                currentType = MovementType.Hangable;
            }
            else if (currentType == MovementType.Hangable && typeChangeTimer <= 0)
            {
                typeChangeTimer = 0.5f;
                currentType = MovementType.Basic;
            }
            Debug.Log("MovementType Changed. New type: "+currentType);
        }

        if(currentType == MovementType.Basic)
        {
            Basic_Move();
        }
        if(currentType == MovementType.Hangable)
        {
            Hanging_Move();
        }
    }
    #region BasicMovement

    private bool isJumping;
    private bool onSlope; // is on a slope or not
    private bool slidingSlope; // is on a slope thats too steep
    private float _slopeAngle;
    private Vector3 hitNormal; //orientation of the slope.

    float storeY;
    private float _slideMultiplier = 1;

    public Vector3 moveVelocity;

    void Basic_Move()
    {
        Vector3 input = Vector3.zero;
        input = Basic_MoveInput();
        _hit = rayController.RaycastGridHit();
        hitNormal = rayController.RaycastGridHit().normal;
        if (hitNormal != Vector3.zero) _slopeAngle = Vector3.Angle(Vector3.up, hitNormal);

        input = Basic_HandleSlopes(input, _slopeAngle);
        

        moveVelocity += gravity * Time.deltaTime * _gravityScale;
        Controller.Move((input + moveVelocity) * Time.deltaTime);
    }

    public Vector3 Basic_MoveInput()
    {
        Vector3 moveInput = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveInput = moveInput.normalized * moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) moveInput *= 0.5f;
        Vector3 _moveVelocity = new Vector3(0, 0, 0);
        Basic_Jump();

        if (!slidingSlope) _moveVelocity = moveInput;
        return _moveVelocity;
    }

    public void Basic_Jump()
    {
        if (IsGrounded())
        {
            moveVelocity.y = 0;
            _gravityScale = 1;
            isJumping = false;
            if (Input.GetButtonDown("Jump"))
            {
                if (!slidingSlope)
                {
                    isJumping = true;
                    moveVelocity.y = jumpHeight;
                }
            }
        }
        else
        {
            if (Mathf.Sign(moveVelocity.y) == -1) _gravityScale *= _gravityMultiplier;
            hitNormal = Vector3.zero;
        }
        if (rayController.CurrentState == RaycastController.State.Hanging)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Changed to hanging");
                typeChangeTimer = 0.5f;
                currentType = MovementType.Hangable;
            }
        }
    }

    private Vector3 Basic_HandleSlopes(Vector3 movement, float slopeAngle)
    {
        Vector2 isMoving = new Vector2(movement.x, movement.z);
        onSlope = (Mathf.Abs(slopeAngle) > 5f && slopeAngle < 89);
        slidingSlope = (slopeAngle >= slopeLimit && Mathf.Sign(hitNormal.y) != -1);

        Vector3 rayOrigin = transform.position + transform.forward * 0.55f;

        //Character on slopes
        if (onSlope && !isJumping && isMoving != Vector2.zero)
        {
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
        else if (slidingSlope)
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

    #endregion

    #region HangingRoof
    void Hanging_Move()
    {
        moveVelocity.y = 0;
        Vector3 input = Vector3.zero;
        input = Hanging_MoveInput();
        _hit = rayController.RaycastGridHit();
        hitNormal = rayController.RaycastGridHit().normal;

        if (rayController.CurrentState == RaycastController.State.Moving || (Input.GetButtonDown("Jump") && typeChangeTimer <= 0))
        {
            Debug.Log("Changed to Babsic");
            typeChangeTimer = 0.5f;
            currentType = MovementType.Basic;
            moveVelocity.y = -0.8f;
        }
        
        Controller.Move((input + moveVelocity) * Time.deltaTime);
    }

    public Vector3 Hanging_MoveInput()
    {
        Vector3 moveInput = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveInput = moveInput.normalized * moveSpeed * 0.75f;
        Vector3 _moveVelocity = new Vector3(0, 0, 0);

        _moveVelocity = moveInput;
        return _moveVelocity;
    }
    #endregion

    private void OnDrawGizmos()
    {

        gravity.y = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        var curPos = transform.position + -Vector3.up + transform.forward/2;
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
            if (Physics.Raycast(curPos, nextPos - curPos, out hit, Vector3.Distance(curPos, nextPos), collisionMask, QueryTriggerInteraction.Ignore))
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
}