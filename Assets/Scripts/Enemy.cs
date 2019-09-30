using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer AnimationSpriteRenderer;

    public void TakeDamage()
    {
        Debug.Log("Hit");
        
        StartCoroutine(TintEnemy());
    }

    //Função para testes
    private IEnumerator TintEnemy()
    {
        AnimationSpriteRenderer.material.color = Color.red;

        yield return  new WaitForSeconds(0.1f);
        
        AnimationSpriteRenderer.material.color = Color.white;
    }
}
