using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {

	public float healAmount = 30f;
	public AudioClip pickupSound;
	public GameObject pickupEffectPrefab;

	void OnTriggerEnter2D(Collider2D inCollider) {
		if (inCollider.tag != "Player")
			return;

		Character character = inCollider.GetComponent<Character>();
		character.Heal (healAmount);

		Collider2D targetCollider = character.GetComponent<Collider2D> ();

		AudioSource.PlayClipAtPoint (pickupSound, transform.position, 0.5f);

		GameObject pickupEffect = Instantiate (pickupEffectPrefab, targetCollider.bounds.center, Quaternion.identity) as GameObject;
		pickupEffect.transform.SetParent (inCollider.transform, true);
		Destroy (pickupEffect, 3);

		Destroy (gameObject);
	}
}
