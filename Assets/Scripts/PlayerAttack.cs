using Enso.Characters.Player;
using Enso.Interfaces;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	private bool isAttackColliderEnabled;
	private bool holdingAttackButton;
	private Collider2D[] enemiesToDamage;
	private float timeHoldingAttackButton;
	private Player player;

	[SerializeField] private LayerMask EnemiesLayerMask;
	[SerializeField] private Transform AttackAnchor;
	[SerializeField] private Transform AttackPosition;

	[Header("References")]
	[SerializeField] private PlayerDefense Defense;
	[SerializeField] private PlayerMovement Movement;
	[SerializeField] private PlayerProperties Properties;

	[HideInInspector] public bool IsPerformingSimpleAttack;
	[HideInInspector] public bool IsPerformingHeavyAttack;

	#region Delegates

	private void OnEnable()
	{
		PlayerInput.AttackInputDown += StartAttack;
		PlayerInput.AttackInputUp += ReleaseAttackButton;
		CharacterAnimations.EnableCollider += EnableAttackCollider;
		CharacterAnimations.DisableCollider += DisableAttackCollider;
		CharacterAnimations.EndAttackAnimation += EndAttack;
		CharacterAnimations.EndHeavyAttackAnimation += EndHeavyAttack;
	}

	private void OnDisable()
	{
		PlayerInput.AttackInputDown -= StartAttack;
		PlayerInput.AttackInputUp -= ReleaseAttackButton;
		CharacterAnimations.EnableCollider -= EnableAttackCollider;
		CharacterAnimations.DisableCollider -= DisableAttackCollider;
		CharacterAnimations.EndAttackAnimation -= EndAttack;
		CharacterAnimations.EndHeavyAttackAnimation -= EndHeavyAttack;
	}

	#endregion

	private void Start()
	{
		player = GetComponent<Player>();
	}
	
	private void Update()
	{
		float angle = Mathf.Atan2(Movement.Velocity.y, Movement.Velocity.x) * Mathf.Rad2Deg;
		AttackAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		if (holdingAttackButton)
			timeHoldingAttackButton += Time.deltaTime;

		if (isAttackColliderEnabled)
		{
			if (IsPerformingSimpleAttack)
				GetAttackCollisions();
			else if (IsPerformingHeavyAttack)
				GetHeavyAttackCollisions();
		}
	}

	private void StartAttack()
	{
		holdingAttackButton = true;

		if (!IsPerformingSimpleAttack && !IsPerformingHeavyAttack && !Defense.IsDefending)
		{
			IsPerformingSimpleAttack = true;

			//Randomizar animações de ataque
			player.Animator.Play(CharacterAnimations.BasicAttackState);
		}
		else
		{
			Debug.Log("Can't attack yet'");
		}
	}

	private void ReleaseAttackButton()
	{
		if (timeHoldingAttackButton >= Properties.HeavyAttackHoldingTime)
		{
			IsPerformingHeavyAttack = true;
			player.Animator.Play(CharacterAnimations.HeavyAttackState);
		}
		else
		{
			IsPerformingSimpleAttack = true;
			player.Animator.Play(CharacterAnimations.BasicAttackState);
		}

		timeHoldingAttackButton = 0;
		holdingAttackButton = false;
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
		IsPerformingSimpleAttack = false;
	}

	private void EndHeavyAttack()
	{
		IsPerformingHeavyAttack = false;
	}

	private void GetAttackCollisions()
	{
		enemiesToDamage = Physics2D.OverlapBoxAll(AttackPosition.position, Properties.AttackRange, 0, EnemiesLayerMask);
		foreach (Collider2D enemyToDamage in enemiesToDamage)
		{
			var enemy = enemyToDamage.GetComponent<IDamageable>();
			enemy?.TakeDamage(Properties.SimpleAttackDamage);
		}
	}

	private void GetHeavyAttackCollisions()
	{
		enemiesToDamage = Physics2D.OverlapBoxAll(AttackPosition.position, Properties.HeavyAttackRange, 0, EnemiesLayerMask);
		foreach (Collider2D enemyToDamage in enemiesToDamage)
		{
			var enemy = enemyToDamage.GetComponent<IDamageable>();
			enemy?.TakeDamage(Properties.HeavyAttackDamage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(AttackPosition.position, Properties.AttackRange);
	}
}