using UnityEngine;
using System.Collections;

public class BerserkAI : MonoBehaviour
{
	public TriggerArea agroArea;

	Character character;
	RangedAttack attack;
	GameObject player;
	Character playerCharacter;
	
	void Start ()
	{
		character = GetComponent<Character>();
		attack = GetComponentInChildren<RangedAttack>();
		player = GameObject.FindWithTag("Player");
		playerCharacter = player.GetComponent<Character>();
		attack.target = player.transform;

		agroArea.onEnter += OnAgroAreaEnter;
		agroArea.onExit += OnAgroAreaExit;
	}

	void OnAgroAreaEnter(Collider2D collider) {
		if (collider.gameObject == player) {
			StartCoroutine ("Attacking");
		}
	}

	void OnAgroAreaExit(Collider2D collider) {
		if (collider.gameObject == player) {
			StopCoroutine ("Attacking");
		}
	}

	IEnumerator Attacking()
	{
		while (!playerCharacter.dead && !character.dead) {
			character.LookAt (player.transform.position);
			character.Attack ();
			yield return new WaitForSeconds (Random.Range (2f, 5f));
		}
	}
}

