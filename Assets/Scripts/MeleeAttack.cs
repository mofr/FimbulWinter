using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class MeleeAttack : Attack
{
	public float damage = 10;

	CircleCollider2D collider;

	void Awake() {
		collider = GetComponent<CircleCollider2D>();
	}

	override public void Perform ()
	{
		Vector2 center = new Vector2(collider.bounds.center.x, collider.bounds.center.y);
		Collider2D[] targets = Physics2D.OverlapCircleAll(center, collider.radius);
		foreach(Collider2D targetCollider in targets) {
			Damageable damageable = targetCollider.GetComponent<Damageable>();
			if(!damageable || !damageable.enabled) {
				continue;
			}

			damageable.TakeDamage(damage, character);

			Rigidbody2D rigidbody = targetCollider.GetComponent<Rigidbody2D>();
			if(rigidbody) {
				Vector2 force = 400 * (targetCollider.transform.position-character.transform.position).normalized;
				force.y += 200;
				rigidbody.AddForce(force);
			}
			break;
		}
	}
}

