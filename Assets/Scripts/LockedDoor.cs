using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {
	
	public string itemNeeded;
	public AudioClip openSound;
	public AudioClip failSound;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag != "Player")
			return;

		Inventory inventory = collider.GetComponent<Inventory>();
		if (inventory.items.Remove (itemNeeded)) {
			AudioSource.PlayClipAtPoint (openSound, transform.position);
			Destroy (gameObject);
		} else {
			AudioSource.PlayClipAtPoint (failSound, transform.position);
		}
	}
}
