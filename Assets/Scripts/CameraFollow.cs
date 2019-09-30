using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool lookAheadXStopped;
    private bool lookAheadYStopped;
    private float smoothVelocity;
    private FocusArea focusArea;
    private Vector2 currentLookAhead;
    private Vector2 targetLookAhead;
    private Vector2 lookAheadDirection;
    private Vector2 smoothLookAheadVelocity;
    private Vector2 focusPosition;
    private Vector3 offset;

    [SerializeField] private PlayerCollisions Target;
    [SerializeField] private CameraProperties Properties;

    private void Start()
    {
        focusArea = new FocusArea(Target.Collider.bounds, Properties.FocusAreaSize);
        offset = transform.position;
    }

    private void LateUpdate()
    {
        focusArea.Update(Target.Collider.bounds);
        focusPosition = focusArea.Center + Vector2.up * Properties.VerticalOffset;

        UpdateLookAheadProperties(PlayerInput.Movement.x, ref focusArea.Velocity.x, ref lookAheadDirection.x, ref currentLookAhead.x, ref targetLookAhead.x,
            ref smoothLookAheadVelocity.x, ref lookAheadXStopped);
        UpdateLookAheadProperties(PlayerInput.Movement.y, ref focusArea.Velocity.y, ref lookAheadDirection.y, ref currentLookAhead.y, ref targetLookAhead.y,
            ref smoothLookAheadVelocity.y, ref lookAheadYStopped);

        focusPosition += Vector2.right * currentLookAhead.x;
        focusPosition += Vector2.up * currentLookAhead.y;
        transform.position = (Vector3) focusPosition + offset;
    }

    private void UpdateLookAheadProperties(float input, ref float focusAreaVelocity, ref float direction, ref float current, ref float target, ref float lookAheadVelocity,
        ref bool stopped)
    {
        if (focusAreaVelocity != 0)
        {
            direction = Mathf.Sign(focusAreaVelocity);
            if (Mathf.Sign(input) == Mathf.Sign(focusAreaVelocity) && input != 0)
            {
                stopped = false;
                target = direction * Properties.LookAheadDistance;
            }
            else
            {
                if (!stopped)
                {
                    stopped = true;
                    target = current + (direction * Properties.LookAheadDistance - current) / 4f;
                }
            }
        }

        current = Mathf.SmoothDamp(current, target, ref lookAheadVelocity, Properties.LookAheadSmoothTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, .5f);
        Gizmos.DrawCube(focusArea.Center, Properties.FocusAreaSize);
    }

    private struct FocusArea
    {
        public Vector2 Center;
        public Vector2 Velocity;
        private float top, bottom;
        private float left, right;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2f;
            right = targetBounds.center.x + size.x / 2f;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            Velocity = Vector2.zero;
            Center = new Vector2((left + right) / 2f, (top + bottom) / 2f);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;

            if (targetBounds.min.x < left)
                shiftX = targetBounds.min.x - left;
            else if (targetBounds.max.x > right)
                shiftX = targetBounds.max.x - right;

            left += shiftX;
            right += shiftX;

            float shiftY = 0;

            if (targetBounds.min.y < bottom)
                shiftY = targetBounds.min.y - bottom;
            else if (targetBounds.max.y > top)
                shiftY = targetBounds.max.y - top;

            bottom += shiftY;
            top += shiftY;

            Center = new Vector2((left + right) / 2f, (top + bottom) / 2f);
            Velocity = new Vector2(shiftX, shiftY);
        }
    }
}