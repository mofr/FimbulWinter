using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public GameObject pawn;
	Character character;

	public static PlayerInput instance;

	void Awake() {
		instance = this;
	}
	
	void Start ()
	{
		if (!pawn) {
			pawn = GameObject.FindWithTag("Player");
		}

		character = pawn.GetComponent<Character>();
		if (!character) {
			Debug.LogError("Pawn must have Character component");
		}
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Jump")) {
			character.Jump ();
		}
		
		if (Input.GetButton ("Horizontal")) {
			character.Move (Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.LeftShift));
		}

		if (Input.GetButton ("Fire1")) {
			character.Attack();
		}

		character.Block (Input.GetButton ("Fire2"));
	}
}

