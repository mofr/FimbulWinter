using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	public float agroDistance = 5f;
	public float attackDistance = 1.2f;

	Character character;
	float move = 0;
	GameObject player;
	Character playerCharacter;
	Vector3 startPos;
	
	void Start () {
		character = GetComponent<Character>();
		player = GameObject.FindWithTag("Player");
		playerCharacter = player.GetComponent<Character> ();
		startPos = transform.position;

		StartCoroutine ("Patrol");
		StartCoroutine ("SearchPlayer");
	}

	void Update() {
		character.Move (move);
	}

	IEnumerator Patrol() {
		while (true) {
			move = Mathf.Sign(startPos.x - transform.position.x);
			yield return new WaitForSeconds (Random.Range (1.0f, 3.0f));
			move = 0;
			yield return new WaitForSeconds (Random.Range (0.7f, 2.0f));
		}
	}

	IEnumerator SearchPlayer() {
		while (true) {
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
		while (!playerCharacter.dead) {
			float distance = (transform.position - player.transform.position).magnitude;
			float diffX = player.transform.position.x - transform.position.x;

			if(distance < attackDistance) {
				character.LookAt(player.transform.position);
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
}
