﻿using UnityEngine;
using System.Collections;

public class Talkable : MonoBehaviour {
	
	public GameObject bubblePrefab;
	public string[] phrases = {
		"Привет!",
	};


	GameObject bubble;
	bool hot = false;
	bool opened = false;
	
	int currentPhrase;
	
	void Start () {

	}

	void Update () {
		if (opened) {
			if(Input.GetButtonDown ("Use")) {
				currentPhrase += 1;
				if(currentPhrase >= phrases.Length) {
					opened = false;
					Destroy (bubble);
					PlayerInput.instance.enabled = true;
				} else {
					bubble.GetComponent<SpeechBubble>().phrase.text = phrases[currentPhrase];
				}
			}
		} else if (hot) {
			if (Input.GetButtonDown ("Use")) {
				opened = true;
				bubble = Instantiate (bubblePrefab);
				bubble.transform.SetParent(gameObject.transform, false);
				PlayerInput.instance.enabled = false;

				currentPhrase = 0;
				bubble.GetComponent<SpeechBubble>().phrase.text = phrases[currentPhrase];
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			hot = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Player") {
			hot = false;
		}
	}
}