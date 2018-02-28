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
        //Debug.DrawLine(raycastHit.point, raycastHit.point + raycastHit.normal * 2f, Color.green);
        return raycastHit;
    }
    public bool RaycastGrid()
    {
        UpdateRaycastOrigins();
        bool hitBool = false;
        Vector3 velocity = player.Controller.velocity * Time.deltaTime;
        Vector3 direction = new Vector3(velocity.x, 0, velocity.z);
        float rayLength = Mathf.Abs(velocity.y) + 
            1 + skinWidth;


        if (Mathf.Abs(player.Controller.velocity.y) < 0.5f)
        {
            rayLength = 1 + skinWidth;
        }

        for (int k = 0; k < verticalRayCount; k++)
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                float directionY = Mathf.Sign(player.moveVelocity.y);
                Vector3 rayOrigin = raycastOrigin + Vector3.forward * k * verticalRaySpacing + direction;
                rayOrigin += Vector3.right * (verticalRaySpacing * i);
                RaycastHit hit;
                hitBool = Physics.Raycast(rayOrigin, player.transform.up * directionY, out hit, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.black);

                    
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
        bounds.Expand(skinWidth * -0.1f);
        float x, y, z;
        x = player.transform.position.x - bounds.extents.x;
        y = player.transform.position.y;
        z = player.transform.position.z - bounds.extents.z;

        //raycastOrigins.xyz = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        raycastOrigin = new Vector3(x,y,z);
        //raycastOrigins.xyZ = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);

        //raycastOrigins.Xyz = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        //raycastOrigins.XyZ = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);

    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = player.Controller.bounds;
        bounds.Expand(skinWidth * -0.1f);

        float boundsWidth = bounds.size.x;
        //float boundsLength = bounds.size.z;
        //float boundsHeigth = bounds.size.y;

        //horizontalRayCount = Mathf.RoundToInt(boundsHeigth / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);        

        //horizontalRaySpacing = boundsHeigth / (horizontalRayCount - 1);
        verticalRaySpacing = boundsWidth / (verticalRayCount - 1);
    }
}
