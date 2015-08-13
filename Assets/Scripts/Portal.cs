using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public Transform destination;
	public AudioClip sound;
	public bool autoTrigger = false;

	GameObject target;
	Canvas usageKey;
	bool transfering = false;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag != "Player")
			return;
		
		target = collider.gameObject;

		if (autoTrigger) {
			StartTransfer();
		} else {
			if (!usageKey) {
				usageKey = Instantiate (GUIManager.instance.usageKeyPrefab);
				usageKey.transform.localPosition = new Vector2 (0, 2.1f);
				usageKey.transform.SetParent (transform, false);
			}

			usageKey.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject != target)
			return;

		target = null;
		if (usageKey) {
			usageKey.enabled = false;
		}
	}

	void StartTransfer() {
		if (target && !transfering) {
			StartCoroutine(Transfer(target));
		}
	}

	IEnumerator Transfer(GameObject target) {
		AudioSource.PlayClipAtPoint (sound, transform.position);

		transfering = true;

		float duration = 0.25f;
		ScreenFader.FadeToBlack (duration);
		yield return new WaitForSeconds (duration);

		target.transform.SetParent (destination.parent);
		target.transform.position = destination.position;
		CharacterMovement characterMovement = target.GetComponent<CharacterMovement>();
		if (characterMovement) {
			characterMovement.LookAt(destination.position + destination.forward);
		}

		ScreenFader.FadeToClear (duration);

		transfering = false;
	}

	void Update() {
		if ( Input.GetButtonDown ("Use") ) {
			StartTransfer();
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawIcon (GetComponent<Collider2D>().bounds.center, "portal");
	}

	void OnDrawGizmosSelected() {
		if (!destination)
			return;

		Gizmos.color = Color.white;
		Gizmos.DrawLine (GetComponent<Collider2D>().bounds.center, destination.position);
		Gizmos.DrawLine (destination.position, destination.position + destination.forward);
	}
}
