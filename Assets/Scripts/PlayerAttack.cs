using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isAttackColliderEnabled;
    private float actualTimeBetweenAttack;
    private Collider2D[] enemiesToDamage;

    [SerializeField] private float TimeBetweenAttack;
    [SerializeField] private Vector2 AttackRange;
    [SerializeField] private LayerMask EnemiesLayerMask;
    [SerializeField] private Transform AttackAnchor;
    [SerializeField] private Transform AttackPosition;

    [Header("References")] 
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private PlayerMovement Movement;

    [HideInInspector] public bool IsAttacking;

    #region Delegates

    private void OnEnable()
    {
        PlayerInput.AttackInputDown += StartAttack;
        PlayerAnimations.EnableCollider += EnableAttackCollider;
        PlayerAnimations.DisableCollider += DisableAttackCollider;
        PlayerAnimations.EndAttackAnimation += EndAttack;
    }

    private void OnDisable()
    {
        PlayerInput.AttackInputDown -= StartAttack;
        PlayerAnimations.EnableCollider -= EnableAttackCollider;
        PlayerAnimations.DisableCollider -= DisableAttackCollider;
        PlayerAnimations.EndAttackAnimation -= EndAttack;
    }

    #endregion

    private void Update()
    {
        float angle = Mathf.Atan2(Movement.Velocity.y, Movement.Velocity.x) * Mathf.Rad2Deg;
        AttackAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        actualTimeBetweenAttack -= Time.deltaTime;

        if (isAttackColliderEnabled)
            GetAttackCollisions();
    }

    private void StartAttack()
    {
        if (actualTimeBetweenAttack <= 0)
        {
            actualTimeBetweenAttack = TimeBetweenAttack;

            IsAttacking = true;

            PlayerAnimator.Play(PlayerAnimations.BasicAttackState);
        }
        else
        {
            Debug.Log("Can't attack yet'");
        }
    }

    private void EnableAttackCollider()
    {
        isAttackColliderEnabled = true;
    }
    
    private void DisableAttackCollider()
    {
        isAttackColliderEnabled = false;
    }

    private void EndAttack()
    {
        IsAttacking = false;
    }

    private void GetAttackCollisions()
    {
        enemiesToDamage = Physics2D.OverlapBoxAll(AttackPosition.position, AttackRange, 0, EnemiesLayerMask);

        foreach (Collider2D enemyToDamage in enemiesToDamage)
        {
            var enemy = enemyToDamage.GetComponent<Enemy>();
                
            if(enemy != null)
                enemy.TakeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackPosition.position, AttackRange);
    }
}
