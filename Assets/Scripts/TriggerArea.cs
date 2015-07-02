using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TriggerArea : MonoBehaviour {
	
	public bool playerOnly = true;
	public UnityEvent enterEvent;
	public UnityEvent exitEvent;

	GameObject player;

	void Start() {
		player = GameObject.FindWithTag ("Player");
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (playerOnly && collider.gameObject != player)
			return;

		enterEvent.Invoke ();
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (playerOnly && collider.gameObject != player)
			return;

		exitEvent.Invoke ();
	}
}
