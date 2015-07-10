﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Portal : MonoBehaviour {

	public Transform destination;
	public AudioClip sound;

	AudioSource audioSource;
	GameObject target;
	Canvas usageKey;
	bool trasfering = false;

	void Awake() {
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag != "Player")
			return;

		target = collider.gameObject;

		if (!usageKey) {
			usageKey = Instantiate(GUIManager.instance.usageKeyPrefab);
			usageKey.transform.localPosition = new Vector2(0, 2.1f);
			usageKey.transform.SetParent(transform, false);
		}

		usageKey.enabled = true;
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject != target)
			return;

		target = null;
		usageKey.enabled = false;
	}

	IEnumerator Transfer(GameObject target) {
		audioSource.PlayOneShot (sound);

		trasfering = true;

		float duration = 0.25f;
		ScreenFader.FadeToBlack (duration);
		yield return new WaitForSeconds (duration);

		target.transform.SetParent (destination.parent);
		target.transform.position = destination.position;

		ScreenFader.FadeToClear (duration);

		trasfering = false;
	}

	void Update() {
		if (target && !trasfering && Input.GetButtonDown ("Use")) {
			StartCoroutine(Transfer(target));
		}
	}
}
