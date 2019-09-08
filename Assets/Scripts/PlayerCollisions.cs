using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCollisions : MonoBehaviour
{
    private const float SkinWidth = .015f;

    private BoxCollider2D thisCollider;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    private RaycastOrigins raycastOrigins;

    [Header("Raycast Properties")] [SerializeField]
    private int HorizontalRayCount = 4;

    [SerializeField] private int VerticalRayCount = 4;
    [SerializeField] private LayerMask CollisionLayerMask;

    private void Start()
    {
        thisCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void GetHorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionLayerMask);

            if (hit)
            {
                velocity.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;
            }

            Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.red);
        }
    }
    
    public void GetVerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionLayerMask);

            if (hit)
            {
                velocity.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;
            }

            Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
        }
    }

    private struct RaycastOrigins
    {
        public Vector2 BottomLeft;
        public Vector2 BottomRight;
        public Vector2 TopLeft;
        public Vector2 TopRight;
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = thisCollider.bounds;
        bounds.Expand(SkinWidth * -2f);

        raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = thisCollider.bounds;
        bounds.Expand(SkinWidth * -2f);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, Int32.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, Int32.MaxValue);

        horizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }
}