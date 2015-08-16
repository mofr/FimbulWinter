using UnityEngine;
using System.Collections;

public class Firezone : MonoBehaviour {

	public float damagePerSecond = 10;

	void OnTriggerStay2D(Collider2D collider) {
		Damageable damageable = collider.GetComponent<Damageable>();
		if (damageable) {
			damageable.TakeDamage (damagePerSecond * Time.deltaTime, gameObject, new Vector2(0,0));
		}
	}

}
