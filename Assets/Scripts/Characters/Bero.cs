using UnityEngine;
using System.Collections;

public class Bero : MonoBehaviour {

	public float attackDamage = 10;
	public float attackTime = 1f;
	public BoxCollider2D attackCollider;

	Character character;
	CharacterMovement characterMovement;
	Animator anim;

	float attackCooldown = 0f;
	
	void Awake () {
		character = GetComponent<Character>();
		characterMovement = GetComponent<CharacterMovement>();
		anim = GetComponent<Animator>();
	}

	void Update () {
		attackCooldown = Mathf.Max (0, attackCooldown-Time.deltaTime);
	}

	public void Attack() {
		if (attackCooldown > 0)
			return;
		if (character.recoveryRemains > 0)
			return;
		if (!characterMovement.grounded)
			return;
		
		anim.SetTrigger ("Attack");
		attackCooldown = attackTime;
	}

	void OnAttack() {
		Collider2D[] targets = Physics2D.OverlapAreaAll(attackCollider.bounds.min, attackCollider.bounds.max);
		foreach(Collider2D targetCollider in targets) {
			Damageable damageable = targetCollider.GetComponent<Damageable>();
			if(!damageable || !damageable.enabled) {
				continue;
			}
			
			damageable.TakeDamage(attackDamage, character);
		}
	}
}
