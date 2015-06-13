﻿using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public bool enemy = false;
	public bool initialRight = true;

	[Header("Locomotion")]
	public float walkSpeed = 2f;
	public float runSpeed = 6f;
	public float jumpForce = 350f;

	[Header("Battle")]
	public float health = 100f;
	public bool dead = false;
	public float attackTime = 1f;
	public float attackDamage = 10f;
	public CircleCollider2D attackCollider;
	public float recoveryTime = 1f;

	Animator anim;
	Rigidbody2D rigidBody;
	AudioSource audioSource;
	Puppet2D_GlobalControl puppet;
	bool facingRight;
	float recoveryRemains = 0f;
	float attackCooldown = 0f;

	void Awake() {
		facingRight = initialRight;
		
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
		puppet = GetComponent<Puppet2D_GlobalControl>();
	}
	
	void Start () {

	}

	void FixedUpdate () {
		anim.SetFloat ("Speed", Mathf.Abs(rigidBody.velocity.x)/runSpeed);
		anim.SetFloat ("vSpeed", rigidBody.velocity.y);
		anim.SetBool ("Grounded", Mathf.Abs(rigidBody.velocity.y) < 0.1);

		recoveryRemains = Mathf.Max (0, recoveryRemains-Time.deltaTime);
		anim.SetFloat ("RecoveryRemains", recoveryRemains);

		attackCooldown = Mathf.Max (0, attackCooldown-Time.deltaTime);
	}

	public void Jump() {
		if (IsAttacking ())
			return;
		if (recoveryRemains > 0)
			return;

		anim.SetTrigger ("Jump");
	}

	public void Move(float move, bool run = false) {
		if (IsAttacking ())
			return;
		if (recoveryRemains > 0)
			return;
		if (move == 0 || dead)
			return;

		float moveSpeed = run ? runSpeed : walkSpeed;
		rigidBody.velocity = new Vector2 (move*moveSpeed, rigidBody.velocity.y);

		if (move > 0 != facingRight) {
			Flip ();
		}
	}

	public void Attack() {
		if (attackCooldown > 0)
			return;
		if (recoveryRemains > 0)
			return;

		anim.SetTrigger ("Attack");
		attackCooldown = attackTime;
	}

	public void LookAt(Vector3 position) {
		if (facingRight != position.x > transform.position.x) {
			Flip ();
		}
	}

	public void TakeDamage(float damage, Character originator) {
		if (dead)
			return;

		LookAt (originator.gameObject.transform.position);
		health -= damage;
		if (health <= 0) {
			Kill();
		} else {
			anim.SetTrigger ("TakeDamage");
			recoveryRemains = Mathf.Max(recoveryRemains, recoveryTime);
		}
	}

	void Flip() {
		facingRight = !facingRight;

		if (puppet) {
			puppet.flip = !puppet.flip;
		} else {
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	void Kill() {
		dead = true;
		gameObject.layer = LayerMask.NameToLayer("Dead");
		anim.SetTrigger ("Death");
	}

	bool IsAttacking() {
		return anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack");
	}
	
	void OnStep() {
		if (!audioSource)
			return;
	}

	void OnJump() {
		rigidBody.AddForce(new Vector2(0f, jumpForce));
	}

	void OnAttack() {
		Vector2 center = new Vector2(attackCollider.bounds.center.x, attackCollider.bounds.center.y);
		Collider2D[] targets = Physics2D.OverlapCircleAll(center, attackCollider.radius);
		foreach(Collider2D collider in targets) {
			Character targetCharacter = collider.gameObject.GetComponent<Character>();
			if(targetCharacter == null || targetCharacter.enemy == enemy || targetCharacter.dead) {
				continue;
			}
			
			targetCharacter.TakeDamage(attackDamage, this);
			break;
		}
	}
}
