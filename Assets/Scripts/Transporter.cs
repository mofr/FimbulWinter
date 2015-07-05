using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Transporter : MonoBehaviour {

	Dictionary<Collider2D, Transform> parents = new Dictionary<Collider2D, Transform>();

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.isTrigger)
			return;

		parents[collider] = collider.transform.parent;
		collider.transform.SetParent (transform);
	}

	void OnTriggerExit2D(Collider2D collider) {
		Transform parent;
		if (parents.TryGetValue (collider, out parent)) {
			collider.transform.SetParent (parent);
		}
	}
}
