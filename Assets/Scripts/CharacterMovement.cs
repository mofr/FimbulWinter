using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMovement : MonoBehaviour
{
	public bool initialRight = true;
	public float walkSpeed = 2f;
	public float runSpeed = 6f;
	public float jumpForce = 350f;

	public GameObject jumpEffectPrefab;
	public AudioClip[] footsteps;
	public AudioClip[] jumpSound;
	public AudioClip landingSound;

	[HideInInspector]
	public bool canMove = true;

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

	Animator anim;
	Rigidbody2D rigidBody;
	Puppet2D_GlobalControl puppet;
	BoxCollider2D collider;
	AudioSource audioSource;

	bool facingRight;
	bool _grounded = false;
	Collider2D _groundedOn;
	Collider2D _rightWall;
	Collider2D _leftWall;

	void Awake() {
		facingRight = initialRight;

		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		puppet = GetComponent<Puppet2D_GlobalControl>();
		collider = GetComponent<BoxCollider2D>();
		audioSource = GetComponent<AudioSource>();

		if(puppet) {
			facingRight = puppet.flip;
		}
	}

	void FixedUpdate ()
	{
		anim.SetFloat ("Speed", Mathf.Abs(rigidBody.velocity.x)/runSpeed);
		anim.SetFloat ("vSpeed", rigidBody.velocity.y);
		anim.SetBool ("Grounded", _grounded);
		if (!_grounded) {
			anim.ResetTrigger("Jump");
		}
	}
	
	public void Jump() {
		if (!canMove)
			return;
		if (!_grounded)
			return;
		
		anim.SetTrigger ("Jump");
	}
	
	public void Move(float move, bool run = false) {
		if (!canMove)
			return;
		if (move == 0)
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

	public void LookAt(Vector3 position) {
		if (facingRight != position.x > transform.position.x) {
			Flip ();
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

	void OnCollisionEnter2D(Collision2D collision) {
		CheckGround (collision);
	}
	
	void OnCollisionStay2D(Collision2D collision) {
		CheckGround (collision);
	}

	void CheckGround(Collision2D collision) {
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
				if(!_grounded) {
					_grounded = true;
					if(collision.relativeVelocity.y < 0) {
						GroundedEffect (collision);
					}
				}
			}
			if(Mathf.Abs(contact.normal.x) > Mathf.Abs (contact.normal.y) && collision.gameObject.layer != Layers.objects){
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
		JumpEffect ();

		if (jumpSound.Length > 0) {
			audioSource.PlayOneShot(jumpSound[Random.Range (0, jumpSound.Length)]);
		}
	}

	void JumpEffect() {
		if (!jumpEffectPrefab)
			return;

		GameObject jumpEffect = Instantiate (jumpEffectPrefab, transform.position, transform.rotation) as GameObject;
		Destroy (jumpEffect, 2);
	}

	void GroundedEffect(Collision2D collision) {
		if (jumpEffectPrefab) {
			GameObject jumpEffect = Instantiate (jumpEffectPrefab, transform.position, transform.rotation) as GameObject;
			Destroy (jumpEffect, 2);
		}

		if (landingSound) {
			float volume = Mathf.Clamp (collision.relativeVelocity.magnitude/10, 0, 1);
			audioSource.PlayOneShot (landingSound, volume);
		}
	}

	void OnFootstep() {
		if (footsteps.Length > 0) {
			float volume = Mathf.Clamp (rigidBody.velocity.magnitude / runSpeed, 0f, 1f);
			audioSource.PlayOneShot (footsteps [Random.Range (0, footsteps.Length)], volume);
		}
	}
}

