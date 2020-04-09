using UnityEngine;

namespace Framework
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastController : MonoBehaviour
    {
        protected const float SkinWidth = .015f;
        protected const float DistanceBetweenRays = .25f;

        [HideInInspector] public float HorizontalRaySpacing;
        [HideInInspector] public float VerticalRaySpacing;
        [HideInInspector] public int HorizontalRayCount;
        [HideInInspector] public int VerticalRayCount;
        [HideInInspector] public RaycastOrigins Origins;

        [SerializeField] protected BoxCollider2D Collider;
        [SerializeField] protected bool ShowDebug;

        [Header("Raycast Properties")]
        [SerializeField] protected LayerMask CollisionLayerMask;

        public virtual void Start()
        {
            CalculateRaySpacing();
        }
    
        public void UpdateRaycastOrigins()
        {
            Bounds bounds = Collider.bounds;
            bounds.Expand(SkinWidth * -2f);

            Origins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            Origins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            Origins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            Origins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        private void CalculateRaySpacing()
        {
            Bounds bounds = Collider.bounds;
            bounds.Expand(SkinWidth * -2f);

            float boundsWidth = bounds.size.x;
            float boundsHeight = bounds.size.y;

            HorizontalRayCount = Mathf.RoundToInt(boundsHeight / DistanceBetweenRays);
            VerticalRayCount = Mathf.RoundToInt(boundsWidth / DistanceBetweenRays);

            HorizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
            VerticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
        }

        public struct RaycastOrigins
        {
            public Vector2 BottomLeft;
            public Vector2 BottomRight;
            public Vector2 TopLeft;
            public Vector2 TopRight;
        }
    }
}
