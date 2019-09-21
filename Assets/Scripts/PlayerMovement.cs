using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Vector3 currentVelocity;

    [Header("References")] 
    [SerializeField] private PlayerCollisions Collisions;
    [SerializeField] private PlayerProperties Properties;

    private void Update()
    {
        if (Collisions.Info.Above || Collisions.Info.Below)
            velocity.y = 0;

        if (Collisions.Info.Left || Collisions.Info.Right)
            velocity.x = 0;

        targetVelocity = PlayerInput.Movement * Properties.MoveSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref currentVelocity, Properties.AccelerationTime);
        Move(velocity * Time.deltaTime);
    }

    private void Move(Vector2 moveAmount)
    {
        Collisions.UpdateRaycastOrigins();
        Collisions.Info.Reset();

        if (moveAmount.x != 0)
            Collisions.GetHorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
            Collisions.GetVerticalCollisions(ref moveAmount);

        transform.Translate(moveAmount);
    }
}