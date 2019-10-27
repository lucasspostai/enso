using System.Collections;
using UnityEngine;

public class Enemy : Fighter
{
    [SerializeField] private SpriteRenderer AnimationSpriteRenderer;
    [SerializeField] private EnemyProperties Properties;

    private void Start()
    {
        Health = Properties.Health;
        Balance = Properties.BalanceAmount;
    }

    public override void TakeDamage(int damage)
    {
        if (InvincibilityTime > 0)
            return;
        
        base.TakeDamage(damage);

        InvincibilityTime = Properties.InvincibilityTime;

        StartCoroutine(TintEnemy());
    }

    //Função para testes
    private IEnumerator TintEnemy()
    {
        AnimationSpriteRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        
        AnimationSpriteRenderer.material.color = Color.white;
    }
}
