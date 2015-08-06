using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ImpactEffect : MonoBehaviour {

	public float velocityThreshold = 1f;
	public float velocityMult = 1f;
	public AudioClip[] sound;

	AudioSource audioSource;

	void Awake() {
		audioSource = GetComponent<AudioSource>();
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		OnCollision (collision);
	}

	void OnCollisionStay2D(Collision2D collision) {
		OnCollision (collision);
	}

	void OnCollision(Collision2D collision) {
		float magnitude = 0;
		foreach (ContactPoint2D contact in collision.contacts) {
			float projMagnitude = Vector3.Project(collision.relativeVelocity, contact.normal).magnitude;
			if(projMagnitude > magnitude && contact.normal.y > Mathf.Abs (contact.normal.x)) {
				magnitude = projMagnitude;
			}
		}

		if (magnitude < velocityThreshold) 
			return;

		float volume = Mathf.Clamp (magnitude * velocityMult, 0, 1);
		audioSource.PlayOneShot (sound[Random.Range(0, sound.Length)], volume);
	}
}