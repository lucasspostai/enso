using Framework.LevelDesignEvents;
using UnityEngine;

namespace Framework
{
    public class CharacterCollisions : RaycastController
    {
        public CollisionInfo Info;

        public void GetHorizontalCollisions(ref Vector2 velocity)
        {
            float directionX = Mathf.Sign(velocity.x);
            float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

            for (int i = 0; i < HorizontalRayCount; i++)
            {
                Vector2 rayOrigin = directionX == -1 ? Origins.BottomLeft : Origins.BottomRight;
                rayOrigin += Vector2.up * (HorizontalRaySpacing * i);

                //Obstacle Collision
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionLayerMask);

                if (hit && hit.transform != transform)
                {
                    velocity.x = (hit.distance - SkinWidth) * directionX;
                    rayLength = hit.distance;

                    Info.Left = directionX == -1;
                    Info.Right = directionX == 1;
                }

                if (ShowDebug)
                    Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
            }
        }

        public void GetVerticalCollisions(ref Vector2 velocity)
        {
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

            for (int i = 0; i < VerticalRayCount; i++)
            {
                Vector2 rayOrigin = directionY == -1 ? Origins.BottomLeft : Origins.TopLeft;
                rayOrigin += Vector2.right * (VerticalRaySpacing * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionLayerMask);

                if (hit && hit.transform != transform)
                {
                    velocity.y = (hit.distance - SkinWidth) * directionY;
                    rayLength = hit.distance;

                    Info.Below = directionY == -1;
                    Info.Above = directionY == 1;
                }

                if (ShowDebug)
                    Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
            }
        }

        public struct CollisionInfo
        {
            public bool Above, Below;
            public bool Left, Right;

            public void Reset()
            {
                Above = Below = false;
                Left = Right = false;
            }
        }
    }
}