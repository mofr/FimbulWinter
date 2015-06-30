using UnityEngine;
using System.Collections;

public class Impaled : MonoBehaviour {

	public GameObject hitEffectPrefab;

	Damageable damageable;
	Animator anim;

	void Start () {
		damageable = GetComponent<Damageable>();
		damageable.OnDamage += TakeDamage;

		anim = GetComponent<Animator>();
	}
	
	void TakeDamage(Damage damage) {
		GameObject hitEffect = Instantiate (hitEffectPrefab) as GameObject;
		hitEffect.transform.position = transform.position;
		if (damage.originator.transform.position.x > transform.position.x) {
			hitEffect.transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		Destroy (hitEffect, 3);

		anim.SetTrigger ("Fall");
	}
}
