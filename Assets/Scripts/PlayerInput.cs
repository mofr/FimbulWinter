using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public GameObject pawn;
	Character character;
	CharacterMovement movement;
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

		movement = pawn.GetComponent<CharacterMovement>();
	}

	void Update() {
		if (Input.GetButtonDown ("Jump")) {
			jumpPressed = true;
		}
	}

	void FixedUpdate ()
	{
		if (character.dead)
			return;

		bool slipOffPlatforms = Input.GetButton ("Vertical") && Input.GetAxis ("Vertical") < 0;
		Physics2D.IgnoreLayerCollision (characterLayer, platformsLayer, slipOffPlatforms);

		if (jumpPressed) {
			if(slipOffPlatforms) {
				movement.SlipOffPlatform();
			} else {
				movement.Jump ();
			}
			jumpPressed = false;
		}

		movement.Move (Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.LeftShift));

		if (Input.GetButton ("Fire1")) {
			character.Attack();
		}

		character.Block (Input.GetButton ("Fire2"));
	}
}

