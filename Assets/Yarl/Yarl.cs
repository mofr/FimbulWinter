using UnityEngine;
using System.Collections;

public class Yarl : MonoBehaviour {

	bool conversation = false;
	
	void Start () {
	
	}

	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			conversation = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Player") {
			conversation = false;
		}
	}

	void OnGUI() {
		if (conversation) {
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			Rect rect = new Rect (0, 0, 100, 100);
			Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
			rect.center = new Vector2(pos.x, Screen.height - pos.y - 60);
			GUI.Label (rect, "О, привет!");
		}
	}
}
