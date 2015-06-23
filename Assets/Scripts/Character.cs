using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public bool enemy = false;
	public bool initialRight = true;

	[Header("Locomotion")]
	public float walkSpeed = 2f;
	public float runSpeed = 6f;
	public float jumpForce = 350f;

	[Header("Battle")]
	public float health = 100f;
	public float maxHealth = 100f;
	public bool dead = false;
	public float recoveryTime = 1f;
	public float attackTime = 1f;

	Animator anim;
	Rigidbody2D rigidBody;
	Puppet2D_GlobalControl puppet;

	bool facingRight;
	float recoveryRemains = 0f;
	float attackCooldown = 0f;
	bool block = false;
	bool grounded = false;
	Collider2D groundedOn;
	Collider2D rightWall;
	Collider2D leftWall;
	Attack attack;

	void Awake() {
		facingRight = initialRight;
		
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		puppet = GetComponent<Puppet2D_GlobalControl>();

		attack = GetComponentInChildren<Attack>();
	}

	void FixedUpdate () {
		anim.SetFloat ("Speed", Mathf.Abs(rigidBody.velocity.x)/runSpeed);
		anim.SetFloat ("vSpeed", rigidBody.velocity.y);
		anim.SetBool ("Grounded", grounded);

		recoveryRemains = Mathf.Max (0, recoveryRemains-Time.deltaTime);
		anim.SetFloat ("RecoveryRemains", recoveryRemains);

		attackCooldown = Mathf.Max (0, attackCooldown-Time.deltaTime);
	}

	public void SlipOffPlatform() {
		if (!grounded)
			return;
		foreach (Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = false;
			collider.enabled = true;
		}
	}

	public void Jump() {
		if (block)
			return;
		if (!grounded)
			return;
		if (IsAttacking ())
			return;
		if (recoveryRemains > 0)
			return;

		anim.SetTrigger ("Jump");
	}

	public void Move(float move, bool run = false) {
		if (block)
			return;
		if (IsAttacking ())
			return;
		if (recoveryRemains > 0)
			return;
		if (move == 0 || dead)
			return;
		if (rightWall && move > 0)
			return;
		if (leftWall && move < 0)
			return;

		float speed = run ? runSpeed : walkSpeed;
		rigidBody.velocity = new Vector2 (move * speed, rigidBody.velocity.y);

		if (move > 0 != facingRight) {
			Flip ();
		}
	}

	public void Attack() {
		if (block)
			return;
		if (attackCooldown > 0)
			return;
		if (recoveryRemains > 0)
			return;
		if (!grounded)
			return;

		anim.SetTrigger ("Attack");
		attackCooldown = attackTime;
	}

	public void Block(bool block) {
		if (IsAttacking ())
			return;
		if (!grounded)
			return;
		if (recoveryRemains > 0)
			return;

		this.block = block;
		anim.SetBool ("Block", block);
	}

	public void LookAt(Vector3 position) {
		if (facingRight != position.x > transform.position.x) {
			Flip ();
		}
	}

	public void TakeDamage(float damage, Character originator) {
		if (dead)
			return;

		if (block) {
			return;
		}

		LookAt (originator.gameObject.transform.position);
		health -= damage;
		if (health <= 0) {
			Kill();
		} else {
			anim.SetTrigger ("TakeDamage");
			recoveryRemains = Mathf.Max(recoveryRemains, recoveryTime);
		}
	}
	
	public void Kill() {
		dead = true;
		gameObject.layer = LayerMask.NameToLayer("Dead");
		anim.SetTrigger ("Death");
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

	bool IsAttacking() {
		return anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack");
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject == gameObject)
			return;

		if (collision.collider == rightWall) {
			rightWall = null;
		}

		if (collision.collider == leftWall) {
			leftWall = null;
		}

		for (int i = 0; i < collision.contacts.Length; ++i) {
			ContactPoint2D contact = collision.contacts[i];
			if(contact.normal.y > 0) {
				groundedOn = collision.collider;
				grounded = true;
			}
			if(Mathf.Abs(contact.normal.x) > Mathf.Abs (contact.normal.y)){
				if(contact.normal.x < 0) {
					rightWall = collision.collider;
				}
				if(contact.normal.x > 0) {
					leftWall = collision.collider;
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.collider == groundedOn) {
			grounded = false;
			groundedOn = null;
		}
		if (collision.collider == rightWall) {
			rightWall = null;
		}
		if (collision.collider == leftWall) {
			leftWall = null;
		}
	}

	void OnJump() {
		rigidBody.AddForce(new Vector2(0f, jumpForce));
	}

	void OnAttack() {
		attack.Perform ();
	}
}
