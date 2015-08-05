using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;

public class Talkable : MonoBehaviour {
	
	public GameObject bubblePrefab;
	public bool autoStart = false;
	public UnityEvent action;
	public AudioClip startSound;

	[Serializable]
	public class Phrase
	{
		public Sprite portrait;

		[TextArea(2,10)]
		public string text = "Привет!";
	}

	public Phrase[] phrases;

	SpeechBubble bubble;
	bool hot = false;
	bool opened = false;
	int currentPhrase;

	void Update () {
		if (opened) {
			if(Input.GetButtonDown ("Use")) {
				currentPhrase += 1;
				if(currentPhrase >= phrases.Length) {
					EndConversation();
				} else {
					bubble.phrase.text = phrases[currentPhrase].text;
					bubble.portrait.sprite = phrases[currentPhrase].portrait;
				}
			}
		} else if (hot) {
			if (Input.GetButtonDown ("Use")) {
				StartConversation ();
			}
		}
	}

	public void StartConversation() {
		opened = true;
		bubble = Instantiate (bubblePrefab).GetComponent<SpeechBubble>();
		bubble.transform.SetParent(gameObject.transform, false);
		PlayerInput.instance.enabled = false;
		
		currentPhrase = 0;
		bubble.phrase.text = phrases[currentPhrase].text;
		bubble.portrait.sprite = phrases[currentPhrase].portrait;

		if (!autoStart) {
			UsageHint.Hide ();
		}
	}

	void EndConversation() {
		opened = false;
		Destroy (bubble.gameObject);
		PlayerInput.instance.enabled = true;

		action.Invoke ();
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			if(autoStart) {
				StartConversation();
			} else {
				hot = true;
				UsageHint.Show();
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Player") {
			if(!autoStart) {
				hot = false;
				UsageHint.Hide();
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawIcon (GetComponent<Collider2D>().bounds.center, "Speech");
	}
}
