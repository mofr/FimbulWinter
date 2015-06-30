using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviour {

	public bool enemy = false;
	public GameObject deathEffectPrefab;

	[Header("Battle")]
	public float health = 100f;
	public float maxHealth = 100f;
	public bool dead = false;
	public float recoveryTime = 1f;
	public float attackTime = 1f;

	public delegate void OnDeath();
	public event OnDeath onDeath;

	Animator anim;
	Damageable damageable;
	CharacterMovement movement;
	BoxCollider2D collider;
	
	float recoveryRemains = 0f;
	float attackCooldown = 0f;
	bool block = false;
	Attack attack;

	void Awake() {
		anim = GetComponent<Animator>();
		attack = GetComponentInChildren<Attack>();
		movement = GetComponent<CharacterMovement>();
		collider = GetComponent<BoxCollider2D>();

		damageable = GetComponent<Damageable>();
		damageable.OnDamage += TakeDamage;
	}

	void FixedUpdate () {
		if (recoveryRemains > 0) {
			recoveryRemains -= Time.deltaTime;
			if (recoveryRemains <= 0) {
				recoveryRemains = 0;
				movement.canMove = true;
			}

			anim.SetFloat ("RecoveryRemains", recoveryRemains);
		}

		attackCooldown = Mathf.Max (0, attackCooldown-Time.deltaTime);
	}

	public void Attack() {
		if (block)
			return;
		if (attackCooldown > 0)
			return;
		if (recoveryRemains > 0)
			return;

		anim.SetTrigger ("Attack");
		attackCooldown = attackTime;
	}

	public void Block(bool block) {
		if (recoveryRemains > 0)
			return;

		if (this.block != block) {
			this.block = block;
			anim.SetBool ("Block", block);
			movement.canMove = !block;
		}
	}

	void TakeDamage(Damage damage) {
		if (dead)
			return;

		if (block && damage.canBeBlocked) {
			return;
		}

		if (damage.originator.enemy == enemy)
			return;

		health -= damage.amount;
		movement.LookAt (damage.originator.transform.position);
		if (health <= 0) {
			Kill();
		} else {
			anim.SetTrigger ("TakeDamage");
			recoveryRemains = Mathf.Max(recoveryRemains, recoveryTime);
			movement.canMove = false;
		}
	}
	
	public void Kill() {
		dead = true;
		gameObject.layer = LayerMask.NameToLayer("Dead");
		anim.SetTrigger ("Death");
		damageable.enabled = false;
		movement.canMove = false;

		if(onDeath != null)
			onDeath ();

		if (deathEffectPrefab) {
			GameObject deathEffect = Instantiate (deathEffectPrefab, collider.bounds.center, transform.rotation) as GameObject;
			Destroy (deathEffect, 4);
		}
	}

	void OnAttack() {
		attack.Perform ();
	}
}
