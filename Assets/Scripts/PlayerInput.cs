using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public GameObject pawn;
	CharacterMovement movement;
	Bero bero;
	bool jumpPressed = false;

	public static PlayerInput instance;

	void Awake() {
		instance = this;
	}
	
	void Start ()
	{
		if (!pawn) {
			pawn = GameObject.FindWithTag("Player");
		}

		movement = pawn.GetComponent<CharacterMovement>();
		bero = pawn.GetComponent<Bero>();
	}

	void Update() {
		if (Input.GetButtonDown ("Jump")) {
			jumpPressed = true;
		}
	}

	void FixedUpdate ()
	{
		bool slipOffPlatforms = Input.GetButton ("Vertical") && Input.GetAxis ("Vertical") < 0;
		Physics2D.IgnoreLayerCollision (Layers.characters, Layers.platform, slipOffPlatforms);

		if (jumpPressed) {
			if(slipOffPlatforms) {
				pawn.layer = pawn.layer;
			} else {
				movement.Jump ();
			}
			jumpPressed = false;
		}

		movement.Move (Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.LeftShift));

		if (Input.GetButton ("Fire1")) {
			bero.Attack();
		}
	}
}

