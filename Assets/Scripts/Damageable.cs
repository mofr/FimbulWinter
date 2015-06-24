using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Damageable : MonoBehaviour {

	public delegate void DamageHandler (float damage, Character originator);
	public event DamageHandler OnDamage;
	public UnityEvent OnDamageAction;

	public void TakeDamage(float damage, Character originator) {
		if (!enabled)
			return;
		OnDamage (damage, originator);
		OnDamageAction.Invoke ();
	}
}
