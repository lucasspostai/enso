using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 velocity;
    
    [Header("References")]
    [SerializeField] private PlayerCollisions Collisions;
    [SerializeField] private PlayerProperties Properties;
    
    private void Update()
    {
        velocity = PlayerInput.Movement * Properties.MoveSpeed;
        Move(velocity * Time.deltaTime);
    }
    
    private void Move(Vector3 velocity)
    {
        Collisions.UpdateRaycastOrigins();

        if (velocity.x != 0)
            Collisions.GetHorizontalCollisions(ref velocity);

        if (velocity.y != 0)
            Collisions.GetVerticalCollisions(ref velocity);

        transform.Translate(velocity);
    }
}
