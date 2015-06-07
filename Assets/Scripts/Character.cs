using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float speed = 2f;
	public float jumpForce = 350f;
	public AudioClip[] steps;

	Animator anim;
	Rigidbody2D rigidBody;
	AudioSource audioSource;
	Puppet2D_GlobalControl puppet;
	bool facingRight = true;

	void Awake() {
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
		puppet = GetComponent<Puppet2D_GlobalControl>();
	}
	
	void Start () {

	}

	void FixedUpdate () {
		anim.SetFloat ("Speed", Mathf.Abs(rigidBody.velocity.x));
		anim.SetFloat ("vSpeed", rigidBody.velocity.y);
		anim.SetBool ("Grounded", Mathf.Abs(rigidBody.velocity.y) < 0.1);
	}

	public void Jump() {
		anim.SetTrigger ("Jump");
	}

	public void Move(float move) {
		if (move == 0)
			return;
		rigidBody.velocity = new Vector2 (move*speed, rigidBody.velocity.y);

		if (move > 0 != facingRight) {
			Flip ();
		}
	}

	public void Attack() {
		anim.SetTrigger ("Attack");
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
	
	void OnStep() {
		if (!audioSource)
			return;
		AudioClip audioClip = steps[Random.Range (0, steps.Length)];
		audioSource.PlayOneShot (audioClip);
	}

	void OnJump() {
		rigidBody.AddForce(new Vector2(0f, jumpForce));
	}
}
