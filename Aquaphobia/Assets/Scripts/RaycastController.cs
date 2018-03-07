using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class RaycastController : MonoBehaviour{

    private LayerMask collisionMask;

    private float skinWidth = .015f;
    private const float dstBetweenRays = .17f; //How far the raycasts are from each other
    //private int horizontalRayCount = 4;
    private int verticalRayCount = 4;

    //private float horizontalRaySpacing;
    private float verticalRaySpacing;

    [SerializeField]
    private Player player;

    private RaycastHit raycastHit;

    private Vector3 raycastOrigin;
        
    public enum State { Moving, Hanging }
    public State CurrentState { get; private set; }

    private Hashtable Layers;

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }

        private set
        {
            isGrounded = value;
        }
    }

    private bool isGrounded = false;

    void Start () {
        if (player == null) player = GetComponent<Player>();
        skinWidth = player.Controller.skinWidth*1.2f;
        collisionMask = player.collisionMask;
        CalculateRaySpacing();

        //Init Layers
        Layers = new Hashtable
        {
            { "Environment", LayerMask.NameToLayer("Environment") },
            { "Interactable", LayerMask.NameToLayer("Interactable") }
        };
    }

    private void Update()
    {
        RaycastGrid();
    }

    private void SetRaycastHit(RaycastHit hit)
    {
        raycastHit = hit;
    }

    public RaycastHit RaycastGridHit()
    {
        //Debug.DrawLine(raycastHit.point, raycastHit.point + raycastHit.normal * 2f, Color.green);
        return raycastHit;
    }



    public void RaycastGrid()
    {
        UpdateRaycastOrigins();
        bool downHitBool = false;
        bool upHitBool = false;
        Vector3 velocity = player.Controller.velocity * Time.deltaTime;
        Vector3 direction = new Vector3(velocity.x, velocity.y, velocity.z);
        float rayLength = 1 + skinWidth;

        Debug.Log(CurrentState);


        if (Mathf.Abs(player.Controller.velocity.y) < 0.5f)
        {
            rayLength = 1 + skinWidth;
        }

        for (int k = 0; k < verticalRayCount; k++)
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                //float directionY = (player.Controller.velocity.y>0)? 1 : -1;
                
                Vector3 rayOrigin = raycastOrigin + player.transform.forward * k * verticalRaySpacing + direction;
                rayOrigin += player.transform.right * (verticalRaySpacing * i);
                RaycastHit downHit;
                downHitBool = Physics.Raycast(rayOrigin, -player.transform.up, out downHit, rayLength, collisionMask, QueryTriggerInteraction.Ignore);
                RaycastHit upHit;
                upHitBool = Physics.Raycast(rayOrigin, player.transform.up, out upHit, rayLength, collisionMask, QueryTriggerInteraction.Ignore);

                Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.black);
                Debug.DrawRay(rayOrigin, -Vector2.up * rayLength, Color.grey);


                if (downHitBool)    // Raycasts downward
                {                   
                    rayLength = downHit.distance;
                    SetRaycastHit(downHit);
                    Debug.DrawLine(downHit.point - Vector3.up * 0.15f, downHit.point + Vector3.up * 0.15f);
                    Debug.DrawLine(downHit.point - Vector3.right * 0.15f, downHit.point + Vector3.right * 0.15f);
                    
                    CurrentState = State.Moving;
                    IsGrounded = downHitBool;
                    return;
                }
                else if (upHitBool) //Raycasts upward
                {
                    rayLength = upHit.distance;
                    SetRaycastHit(upHit);
                    Debug.DrawLine(upHit.point - Vector3.up * 0.15f, upHit.point + Vector3.up * 0.15f);
                    Debug.DrawLine(upHit.point - Vector3.right * 0.15f, upHit.point + Vector3.right * 0.15f);

                    if (upHit.transform.gameObject.layer == (int)Layers["Interactable"])
                    {
                        if (upHit.transform.gameObject.tag == "Hangable")
                        {
                            CurrentState = State.Hanging;
                            IsGrounded = false;
                            return;
                        }
                        else
                        {
                            CurrentState = State.Moving;
                        }
                    }
                }
                else
                {
                    CurrentState = State.Moving;
                    IsGrounded = false;
                }

            }
        }
    }


    public void UpdateRaycastOrigins()
    {
        Bounds bounds = player.Controller.bounds;        
        bounds.Expand(skinWidth * -0.2f);

        Vector3 BackLeft = player.transform.position; //Calculate the point in the back left corner of player
        BackLeft += -player.transform.right * bounds.extents.x;
        BackLeft += -player.transform.forward * bounds.extents.z;

        raycastOrigin = BackLeft;

    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = player.Controller.bounds;
        bounds.Expand(skinWidth * -0.2f);

        float boundsWidth = bounds.size.x;

        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);        
        verticalRaySpacing = boundsWidth / (verticalRayCount - 1);
    }
}
