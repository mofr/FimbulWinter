using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Key : MonoBehaviour {

	public AudioClip pickupSound;
	public GameObject pickupEffectPrefab;
	public UnityEvent pickupEvent;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag != "Player")
			return;

		AudioSource.PlayClipAtPoint (pickupSound, transform.position);

		GameObject pickupEffect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity) as GameObject;
		Destroy (pickupEffect, 3);

		Destroy (gameObject);

		Inventory inventory = collider.GetComponent<Inventory>();
		inventory.items.Add ("gold key");
	}
}
