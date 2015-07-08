using UnityEngine;
using System.Collections;

public class Berserk : MonoBehaviour {

	public float agroDistance = 1;
	public float damage = 0;

	Character character;
	CharacterMovement movement;
	BoxCollider2D collider;

	GameObject player;
	Character playerCharacter;

	float move = 0f;

	Vector2 hitPosition;
	
	void Start () {
		character = GetComponent <Character>();
		movement = GetComponent<CharacterMovement>();
		collider = GetComponent<BoxCollider2D>();

		player = GameObject.FindWithTag ("Player");
		playerCharacter = player.GetComponent <Character>();

		StartCoroutine ("WaitForPlayer");
	}

	IEnumerator WaitForPlayer() {
		while (true) {
			float dist = (player.transform.position - transform.position).magnitude;
			if (dist < agroDistance) {
				StartCoroutine ("Fight");
				break;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator Fight() {
		while (!playerCharacter.dead && !character.dead) {
			move = Mathf.Sign(player.transform.position.x - transform.position.x);
			yield return new WaitForSeconds(Random.Range (0.7f, 1.5f));
		}
	}

	void FixedUpdate() {
		if (character.dead)
			return;

		if (movement.grounded && ((movement.leftWall && move < 0) || (movement.rightWall && move > 0)))
			move = -move;

		if (movement.grounded) {
			RaycastHit2D hit = Physics2D.Raycast (collider.bounds.center, new Vector3 (3 * move, -1, 0), Mathf.Infinity, Layers.groundMask);
			if (Mathf.Abs (hit.normal.x) > 0.1) {
				movement.Jump ();
			}
		}

		movement.Move (move, true);
	}

	void Update() {
		Debug.DrawRay (collider.bounds.center, new Vector3 (3 * move, -1, 0));
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (character.dead)
			return;
		
		if (collider.gameObject == player) {
			Damage damage = new Damage(this.damage, character, transform.position);
			collider.gameObject.GetComponent<Damageable>().TakeDamage(damage);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere (transform.position, agroDistance);
	}
}
