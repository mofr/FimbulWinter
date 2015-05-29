using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	Animator anim;
	Rigidbody2D rigidBody;
	bool facingRight = true;
	
	void Start () {
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Jump();
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			Move (-1.0f);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			Move (1.0f);
		}

		anim.SetFloat ("Speed", Mathf.Abs(rigidBody.velocity.x));
	}

	void Jump() {
		rigidBody.AddForce(new Vector2(0f, 250.0f));
		anim.SetTrigger ("Jump");
	}

	void Move(float move) {
		rigidBody.velocity = new Vector2 (move, rigidBody.velocity.y);

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
}
