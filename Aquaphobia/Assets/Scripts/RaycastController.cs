using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class RaycastController : MonoBehaviour{

    private LayerMask collisionMask;

    private float skinWidth = .015f;
    private const float dstBetweenRays = .15f; //How far the raycasts are from each other
    private int horizontalRayCount = 4;
    private int verticalRayCount = 4;
    private int verticalRayCount2 = 4;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    private float verticalRaySpacing2;

    private float outerRingOffset = 0.15f; //How much higher is the outer ring compared to center
    public Player player;
    public RaycastOrigins raycastOrigins;

    private RaycastHit raycastHit;


    public struct RaycastOrigins
    {
        public Vector3 xyz, xyZ;
        public Vector3 Xyz, XyZ;
    }

    public void Awake()
    {
                   
    }

    void Start () {
        if (player == null) player = GetComponent<Player>();
        skinWidth = player.Controller.skinWidth*1.2f;
        collisionMask = player.collisionMask;
        CalculateRaySpacing();       
    }
    private void SetRaycastHit(RaycastHit hit)
    {
        raycastHit = hit;
    }

    public RaycastHit RaycastGridHit()
    {
        Debug.DrawLine(raycastHit.point, raycastHit.point + raycastHit.normal * 2f, Color.green);
        return raycastHit;
    }
    public bool RaycastGridBool()
    {
        UpdateRaycastOrigins();
        bool hitBool = false;
        Vector3 velocity = player.Controller.velocity * Time.deltaTime;
        Vector3 direction = new Vector3(velocity.x, 0, velocity.z);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth*2;


        if (Mathf.Abs(player.Controller.velocity.y) < 0.5f)
        {
            rayLength = 2 * skinWidth;
        }

        for (int k = 0; k < verticalRayCount2; k++)
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector3 offSetRing = new Vector3(0, 0, 0); 
                if((i != (verticalRayCount - 1) / 2 || k != (verticalRayCount - 1) / 2))
                {
                    offSetRing.y = outerRingOffset;
                }
                    float directionY = Mathf.Sign(player.moveVelocity.y);
                    Vector3 rayOrigin = raycastOrigins.xyz + offSetRing + Vector3.forward * k * verticalRaySpacing + direction;
                    rayOrigin += Vector3.right * (verticalRaySpacing * i);
                    RaycastHit hit;
                    hitBool = Physics.Raycast(rayOrigin, player.transform.up * directionY, out hit, rayLength, collisionMask);

                    Debug.DrawRay(rayOrigin, player.transform.up * directionY * 1f, Color.yellow);
                    Debug.DrawRay(rayOrigin, -Vector2.up * directionY * rayLength, Color.black);

                    
                    if (hitBool)
                    {
                    rayLength = hit.distance;                   
                    SetRaycastHit(hit);
                    Debug.DrawLine(hit.point - Vector3.up * 0.15f, hit.point + Vector3.up * 0.15f);
                    Debug.DrawLine(hit.point - Vector3.right * 0.15f, hit.point + Vector3.right * 0.15f);
                    return hitBool;
                    }

            }
        }
        return hitBool;
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = player.Controller.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.xyz = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigins.xyZ = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);

        raycastOrigins.Xyz = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        raycastOrigins.XyZ = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);

    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = player.Controller.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsLength = bounds.size.z;
        float boundsHeigth = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeigth / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);
        verticalRayCount2 = Mathf.RoundToInt(boundsLength / dstBetweenRays);

        horizontalRaySpacing = boundsHeigth / (horizontalRayCount - 1);
        verticalRaySpacing = boundsWidth / (verticalRayCount - 1);
        verticalRaySpacing2 = boundsLength / (verticalRayCount2 - 1);
    }
}
