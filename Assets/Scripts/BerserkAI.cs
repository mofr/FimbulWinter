using UnityEngine;
using System.Collections;

public class BerserkAI : MonoBehaviour
{
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

		StartCoroutine (CheckVisibility());
	}

	void Update ()
	{

	}

	IEnumerator CheckVisibility() 
	{
		bool visible = false;
		float agroDistance = Camera.main.orthographicSize * Camera.main.aspect;
		while (!character.dead) {
			bool close = (transform.position.x - player.transform.position.x) <= agroDistance;
			if(close) {
				if(!visible) {
					StartCoroutine ("Attacking");
					visible = true;
				}
			} else if(visible){
				StopCoroutine ("Attacking");
				visible = false;
			}
			yield return new WaitForSeconds(0.5f);
		}

		StopCoroutine ("Attacking");
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

