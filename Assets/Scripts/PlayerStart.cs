using UnityEngine;
using System.Collections;

public class PlayerStart : MonoBehaviour
{
	public enum Direction {
		Left,
		Right
	};

	public Direction direction;
	public GameObject pawnPrefab;

	void Awake() {
		Vector3 pos = PlayFromViewport.TakePosition(transform.position);

		GameObject pawn = Instantiate (pawnPrefab, pos, Quaternion.identity) as GameObject;
		pawn.transform.SetParent (transform.parent, true);
		pawn.tag = "Player";

		CharacterMovement characterMovement = pawn.GetComponent<CharacterMovement>();
		switch (direction) {
		case Direction.Left:
			characterMovement.LookAt(pawn.transform.position + new Vector3(-1, 0));
			break;
		case Direction.Right:
			characterMovement.LookAt(pawn.transform.position + new Vector3(1, 0));
			break;
		}
	}

	void OnDrawGizmos() {
		if (direction == Direction.Left) {
			Gizmos.DrawIcon (transform.position, "PlayerStartLeft");
		} else {
			Gizmos.DrawIcon (transform.position, "PlayerStartRight");
		}
	}
}

