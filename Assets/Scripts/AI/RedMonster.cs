using UnityEngine;
using System.Collections;

public class RedMonster : MonoBehaviour {

	public float agroDistance = 5f;
	public float attackDistance = 1.2f;

	Character character;
	CharacterMovement movement;
	float move = 0;
	GameObject player;
	Character playerCharacter;
	Vector3 startPos;
	
	void Start () {
		character = GetComponent<Character>();
		movement = GetComponent <CharacterMovement>();
		player = GameObject.FindWithTag("Player");
		playerCharacter = player.GetComponent<Character> ();
		startPos = transform.position;

		StartCoroutine ("Patrol");
		StartCoroutine ("SearchPlayer");
	}

	void Update() {
		movement.Move (move);
	}

	IEnumerator Patrol() {
		while (!character.dead) {
			move = Mathf.Sign(startPos.x - transform.position.x);
			yield return new WaitForSeconds (Random.Range (1.0f, 3.0f));
			move = 0;
			yield return new WaitForSeconds (Random.Range (0.7f, 2.0f));
		}
	}

	IEnumerator SearchPlayer() {
		while (!character.dead) {
			float distance = (transform.position - player.transform.position).magnitude;
			if(distance < agroDistance) {
				StopCoroutine("Patrol");
				move = 0f;
				StartCoroutine ("Fight");
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator Fight() {
		while (!playerCharacter.dead && !character.dead) {
			float distance = (transform.position - player.transform.position).magnitude;
			float diffX = player.transform.position.x - transform.position.x;

			if(distance < attackDistance) {
				movement.LookAt(player.transform.position);
				character.Attack();
				move = 0;
				yield return new WaitForSeconds(1.0f);
			} else {
				move = Mathf.Sign(diffX);
				yield return new WaitForSeconds(0.2f);
			}
		}

		StartCoroutine ("Patrol");
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, agroDistance);
	}
}
