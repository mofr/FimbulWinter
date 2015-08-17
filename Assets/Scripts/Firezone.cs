using UnityEngine;
using System.Collections;

public class Firezone : MonoBehaviour {

	public float damagePerSecond = 10;
	public GameObject firePrefab;
	public float firesPerSecond = 2;

	void OnTriggerStay2D(Collider2D collider) {
		Damageable damageable = collider.GetComponent<Damageable>();
		if (damageable) {
			damageable.TakeDamage (damagePerSecond * Time.deltaTime, gameObject, new Vector2(0,0));
		}

		if (Random.Range (0, 1) < Time.deltaTime) {
			BodyPart[] bodyParts = collider.GetComponentsInChildren<BodyPart> ();
			if(bodyParts.Length > 0) {
				BodyPart bodyPart = bodyParts[Random.Range (0, bodyParts.Length)];
				GameObject fire = Instantiate(firePrefab, bodyPart.transform.position, Quaternion.identity) as GameObject;
				fire.transform.SetParent(bodyPart.transform);
				Destroy (fire, 3);
			}
		}
	}

}
