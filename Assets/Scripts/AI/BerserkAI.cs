using UnityEngine;
using System.Collections;

public class BerserkAI : MonoBehaviour
{
	public bool attacking {
		set {
			if(value == _attacking) 
				return;

			_attacking = value;
			if(_attacking) {
				StartCoroutine ("Attacking");
			} else {
				StopCoroutine ("Attacking");
			}
		}
	}

	Character character;
	CharacterMovement movement;
	RangedAttack attack;
	GameObject player;
	Character playerCharacter;
	bool _attacking = false;
	
	void Start ()
	{
		character = GetComponent<Character>();
		movement = GetComponent<CharacterMovement>();
		attack = GetComponentInChildren<RangedAttack>();
		player = GameObject.FindWithTag("Player");
		playerCharacter = player.GetComponent<Character>();
		attack.target = player.transform;
	}

	IEnumerator Attacking()
	{
		yield return new WaitForSeconds (Random.Range (1f, 2f));
		while (!playerCharacter.dead && !character.dead) {
			movement.LookAt (player.transform.position);
			character.Attack ();
			yield return new WaitForSeconds (Random.Range (2f, 5f));
		}
	}
}

