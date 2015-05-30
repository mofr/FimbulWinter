using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float speed = 2f;
	public float jumpForce = 350f;
	public AudioClip[] steps;

	Animator anim;
	Rigidbody2D rigidBody;
	AudioSource audioSource;
	bool facingRight = true;
	
	void Start () {
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			anim.SetTrigger ("Jump");
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			Move (-1.0f);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			Move (1.0f);
		}

		anim.SetFloat ("Speed", Mathf.Abs(rigidBody.velocity.x));
		anim.SetFloat ("vSpeed", rigidBody.velocity.y);
		anim.SetBool ("Grounded", Mathf.Abs(rigidBody.velocity.y) < 0.1);
	}

	void Jump() {
		rigidBody.AddForce(new Vector2(0f, jumpForce));
	}

	void Move(float move) {
		rigidBody.velocity = new Vector2 (move*speed, rigidBody.velocity.y);

		if (move > 0 && !facingRight)
		{
			Flip();
		}
		else if (move < 0 && facingRight)
		{
			Flip();
		}
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void OnStep() {
		AudioClip audioClip = steps[Random.Range (0, steps.Length)];
		audioSource.PlayOneShot (audioClip);
	}
}
