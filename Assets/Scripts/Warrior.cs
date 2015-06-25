using UnityEngine;
using System.Collections;

public class Warrior : MonoBehaviour {

	public float agroDistance = 1;

	Character character;
	BoxCollider2D collider;

	GameObject player;
	Character playerCharacter;

	float move = 0f;
	bool jump = false;

	Vector2 hitPosition;
	
	void Start () {
		character = GetComponent <Character>();
		collider = GetComponent<BoxCollider2D> ();

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
		while (!playerCharacter.dead) {
			move = Mathf.Sign(player.transform.position.x - transform.position.x);
			yield return new WaitForSeconds(Random.Range (0.7f, 1.5f));
		}
	}

	void FixedUpdate() {
		if (character.grounded && ((character.leftWall && move < 0) || (character.rightWall && move > 0)))
			move = -move;

		if (character.grounded) {
			RaycastHit2D hit = Physics2D.Raycast (collider.bounds.center, new Vector3 (3 * move, -1, 0), Mathf.Infinity, Layers.groundMask);
			if (Mathf.Abs (hit.normal.x) > 0.1) {
				character.Jump ();
			}
		}

		character.Move (move, true);
	}

	void Update() {
		Debug.DrawRay (collider.bounds.center, new Vector3 (3 * move, -1, 0));
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, agroDistance);
	}
}
