using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public GameObject pawn;
	Character character;
	bool jumpPressed = false;

	public static PlayerInput instance;
	static int characterLayer = LayerMask.NameToLayer("Characters");
	static int platformsLayer = LayerMask.NameToLayer("Platform");

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

	void Update() {
		if (Input.GetButtonDown ("Jump")) {
			jumpPressed = true;
		}
	}

	void FixedUpdate ()
	{
		bool slipOffPlatforms = Input.GetButton ("Vertical") && Input.GetAxis ("Vertical") < 0;
		Physics2D.IgnoreLayerCollision (characterLayer, platformsLayer, slipOffPlatforms);

		if (jumpPressed) {
			if(slipOffPlatforms) {
				character.SlipOffPlatform();
			} else {
				character.Jump ();
			}
		}

		character.Move (Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.LeftShift));

		if (Input.GetButton ("Fire1")) {
			character.Attack();
		}

		character.Block (Input.GetButton ("Fire2"));

		jumpPressed = false;
	}
}

