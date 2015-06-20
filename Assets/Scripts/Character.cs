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
	AudioSource audioSource;
	Puppet2D_GlobalControl puppet;

	bool facingRight;
	float recoveryRemains = 0f;
	float attackCooldown = 0f;
	bool block = false;
	bool grounded = false;
	HashSet<Collider2D> groundedOn = new HashSet<Collider2D>();
	Attack attack;

	void Awake() {
		facingRight = initialRight;
		
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
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

	public void Jump() {
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

		float moveSpeed = run ? runSpeed : walkSpeed;
		rigidBody.velocity = new Vector2 (move*moveSpeed, rigidBody.velocity.y);

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

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject == gameObject)
			return;

		for (int i = 0; i < collision.contacts.Length; ++i) {
			ContactPoint2D contact = collision.contacts[i];
			if(contact.normal.y > 0) {
				groundedOn.Add(collision.collider);
			}
		}

		grounded = groundedOn.Count > 0;
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject == gameObject)
			return;

		groundedOn.Remove (collision.collider);

		grounded = groundedOn.Count > 0;
	}
	
	void OnStep() {
		if (!audioSource)
			return;
	}

	void OnJump() {
		rigidBody.AddForce(new Vector2(0f, jumpForce));
	}

	void OnAttack() {
		attack.Perform ();
	}
}
