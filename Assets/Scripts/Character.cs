using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviour {

	public bool enemy = false;
	public GameObject deathEffectPrefab;
	public GameObject takeDamageEffectPrefab;
	public AudioClip[] damageSound;
	public AudioClip[] deathSound;

	[Header("Battle")]
	public float health = 100f;
	public float maxHealth = 100f;
	public bool dead = false;
	public float recoveryTime = 1f;

	public delegate void OnDeath();
	public event OnDeath onDeath;

	Animator anim;
	Damageable damageable;
	CharacterMovement movement;
	BoxCollider2D collider;
	AudioSource audioSource;

	[HideInInspector]
	public float recoveryRemains = 0f;

	void Awake() {
		anim = GetComponent<Animator>();
		movement = GetComponent<CharacterMovement>();
		collider = GetComponent<BoxCollider2D>();
		damageable = GetComponent<Damageable>();
		audioSource = GetComponent<AudioSource>();

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
	}

	void TakeDamage(Damage damage) {
		if (dead)
			return;

		Character originatorCharacter = damage.originator.GetComponent<Character>();
		if (originatorCharacter) {
			if (originatorCharacter.enemy == enemy)
				return;
			movement.LookAt (damage.originator.transform.position);
		}

		health -= damage.amount;
		if (health <= 0) {
			Kill(damage);
		} else {
			anim.SetTrigger ("TakeDamage");
			recoveryRemains = Mathf.Max(recoveryRemains, recoveryTime);
			movement.canMove = false;

			if (takeDamageEffectPrefab) {
				GameObject takeDamageEffect = Instantiate (takeDamageEffectPrefab, collider.bounds.center, transform.rotation) as GameObject;
				Destroy (takeDamageEffect, 4);
			}

			if(damageSound.Length > 0) {
				audioSource.PlayOneShot(damageSound[Random.Range(0, damageSound.Length)]);
			}
		}
	}

	public void Heal (float healAmount)
	{
		health = Mathf.Min (maxHealth, health + healAmount);
	}
	
	public void Kill(Damage damage = null) {
		KillSilently ();

		if (deathEffectPrefab) {
			GameObject deathEffect = Instantiate (deathEffectPrefab, collider.bounds.center, transform.rotation) as GameObject;
			Destroy (deathEffect, 4);
		}

		if (deathSound.Length > 0) {
			audioSource.PlayOneShot (deathSound [Random.Range (0, deathSound.Length)]);
		}
	}

	public void KillSilently() {
		dead = true;
		gameObject.layer = LayerMask.NameToLayer("Dead");
		damageable.enabled = false;
		movement.canMove = false;
		anim.SetTrigger ("Death");
		
		if(onDeath != null)
			onDeath ();
	}
}
