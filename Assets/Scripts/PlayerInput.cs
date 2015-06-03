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
			character.Move (Input.GetAxis("Horizontal"));
		}
	}
}
