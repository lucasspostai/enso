using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float actualTimeBetweenAttack;
    private Collider2D[] enemiesToDamage;

    [SerializeField] private float TimeBetweenAttack;
    [SerializeField] private Vector2 AttackRange;
    [SerializeField] private LayerMask EnemiesLayerMask;
    [SerializeField] private Transform AttackPosition;
    [SerializeField] private PlayerMovement Movement;

    #region Delegates

    private void OnEnable()
    {
        PlayerInput.AttackInputDown += Attack;
    }

    private void OnDisable()
    {
        PlayerInput.AttackInputDown -= Attack;
    }

    #endregion

    private void Update()
    {
        actualTimeBetweenAttack -= Time.deltaTime;
    }

    private void Attack()
    {
        if (actualTimeBetweenAttack <= 0)
        {
            actualTimeBetweenAttack = TimeBetweenAttack;

            enemiesToDamage = Physics2D.OverlapBoxAll(AttackPosition.position, AttackRange, 0, EnemiesLayerMask);

            foreach (Collider2D enemyToDamage in enemiesToDamage)
            {
                var enemy = enemyToDamage.GetComponent<Enemy>();
                
                if(enemy != null)
                    enemy.TakeDamage();
            }
        }
        else
        {
            Debug.Log("Can't attack yet'");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackPosition.position, AttackRange);
    }
}
