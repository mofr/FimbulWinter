using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TriggerArea : MonoBehaviour {
	
	public UnityAction<Collider2D> onEnter;
	public UnityAction<Collider2D> onExit;

	void OnTriggerEnter2D(Collider2D collider) {
		onEnter.Invoke (collider);
	}

	void OnTriggerExit2D(Collider2D collider) {
		onExit.Invoke (collider);
	}
}
