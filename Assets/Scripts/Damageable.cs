using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Damageable : MonoBehaviour {

	public delegate void DamageHandler (Damage damage);
	public event DamageHandler OnDamage;
	public UnityEvent OnDamageAction;

	public void TakeDamage(Damage damage) {
		if (!enabled)
			return;
		OnDamage (damage);
		OnDamageAction.Invoke ();
	}

	public void TakeDamage(float amount, GameObject originator, Vector2 point) {
		TakeDamage (new Damage(amount, originator, point));
	}
}
