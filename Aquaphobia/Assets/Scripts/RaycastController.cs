using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class RaycastController : MonoBehaviour {

    [SerializeField][Tooltip("Layers raycasts register hits of")]
    private LayerMask collisionMask;

    private float skinWidth = .015f;
    private const float dstBetweenRays = .16f;
    private int horizontalRayCount = 4;
    private int verticalRayCount = 4;
    private int verticalRayCount2 = 4;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    private float verticalRaySpacing2;

    private Player player;
    public RaycastOrigins raycastOrigins;


    public struct RaycastOrigins
    {
        public Vector3 xyz, xyZ;
        public Vector3 Xyz, XyZ;
    }

    public void Awake()
    {
        if(player==null) player = GetComponent<Player>();
        
    }

    void Start () {
        skinWidth = player.Controller.skinWidth;
        CalculateRaySpacing();       
    }

    void Update()
    {
        UpdateRaycastOrigins();

        float directionY = Mathf.Sign(player.moveVelocity.y);
        float rayLength = Mathf.Abs(player.moveVelocity.y) * skinWidth/2 + skinWidth;
        

        if (Mathf.Abs(player.moveVelocity.y) < 0.5f)
        {
            rayLength = 2 * skinWidth;
        }
        /* //All four corners
        Debug.DrawRay(raycastOrigins.xyz, -Vector3.up, Color.red);
        Debug.DrawRay(raycastOrigins.xyZ, -Vector3.up, Color.blue);
        Debug.DrawRay(raycastOrigins.Xyz, -Vector3.up, Color.green);
        Debug.DrawRay(raycastOrigins.XyZ, -Vector3.up, Color.yellow);
        */
        for (int k = 0; k < verticalRayCount2; k++)
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                if (k > 0 && k < verticalRayCount-1 || i > 0 && i < verticalRayCount-1)
                {
                    Vector3 rayOrigin = raycastOrigins.xyz + Vector3.forward * k * verticalRaySpacing2;
                    rayOrigin += Vector3.right * (verticalRaySpacing * i);
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

                    Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength * 3, Color.yellow);
                    Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.black);
                }

            }
        }

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
