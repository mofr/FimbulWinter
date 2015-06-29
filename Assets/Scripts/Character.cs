using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Damageable))]
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

	public bool grounded {
		get { return _grounded; }
	}

	public Collider2D groundedOn {
		get { return groundedOn; }
	}

	public Collider2D rightWall {
		get { return _rightWall; }
	}

	public Collider2D leftWall {
		get { return _leftWall; }
	}

	[HideInInspector]
	public BoxCollider2D collider;

	Animator anim;
	Rigidbody2D rigidBody;
	Puppet2D_GlobalControl puppet;
	Damageable damageable;

	bool facingRight;
	float recoveryRemains = 0f;
	float attackCooldown = 0f;
	bool block = false;
	bool _grounded = false;
	Collider2D _groundedOn;
	Collider2D _rightWall;
	Collider2D _leftWall;
	Attack attack;

	void Awake() {
		facingRight = initialRight;
		
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		puppet = GetComponent<Puppet2D_GlobalControl>();
		attack = GetComponentInChildren<Attack>();
		collider = GetComponent<BoxCollider2D>();

		damageable = GetComponent<Damageable>();
		damageable.OnDamage += TakeDamage;
	}

	void FixedUpdate () {
		anim.SetFloat ("Speed", Mathf.Abs(rigidBody.velocity.x)/runSpeed);
		anim.SetFloat ("vSpeed", rigidBody.velocity.y);
		anim.SetBool ("Grounded", _grounded);

		recoveryRemains = Mathf.Max (0, recoveryRemains-Time.deltaTime);
		anim.SetFloat ("RecoveryRemains", recoveryRemains);

		attackCooldown = Mathf.Max (0, attackCooldown-Time.deltaTime);
	}

	public void SlipOffPlatform() {
		if (!_grounded)
			return;
		foreach (Collider2D collider in GetComponents<Collider2D>()) {
			collider.enabled = false;
			collider.enabled = true;
		}
	}

	public void Jump() {
		if (block)
			return;
		if (!_grounded)
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
		if (_rightWall && move > 0)
			return;
		if (_leftWall && move < 0)
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

		anim.SetTrigger ("Attack");
		attackCooldown = attackTime;
	}

	public void Block(bool block) {
		if (IsAttacking ())
			return;
		if (!_grounded)
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

	void TakeDamage(Damage damage) {
		if (dead)
			return;

		if (block && damage.canBeBlocked) {
			return;
		}

		if (damage.originator.enemy == enemy)
			return;

		LookAt (damage.originator.transform.position);
		health -= damage.amount;
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
		damageable.enabled = false;
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

		if (collision.collider == _rightWall) {
			_rightWall = null;
		}

		if (collision.collider == _leftWall) {
			_leftWall = null;
		}

		for (int i = 0; i < collision.contacts.Length; ++i) {
			ContactPoint2D contact = collision.contacts[i];
			if(contact.normal.y > 0) {
				_groundedOn = collision.collider;
				_grounded = true;
			}
			if(Mathf.Abs(contact.normal.x) > Mathf.Abs (contact.normal.y)){
				if(contact.normal.x < 0) {
					_rightWall = collision.collider;
				}
				if(contact.normal.x > 0) {
					_leftWall = collision.collider;
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.collider == _groundedOn) {
			_grounded = false;
			_groundedOn = null;
		}
		if (collision.collider == _rightWall) {
			_rightWall = null;
		}
		if (collision.collider == _leftWall) {
			_leftWall = null;
		}
	}

	void OnJump() {
		rigidBody.AddForce(new Vector2(0f, jumpForce));
	}

	void OnAttack() {
		attack.Perform ();
	}
}
