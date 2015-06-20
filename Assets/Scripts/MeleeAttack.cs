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
			Character targetCharacter = targetCollider.gameObject.GetComponent<Character>();
			if(targetCharacter == null || targetCharacter.enemy == character.enemy || targetCharacter.dead) {
				continue;
			}
			
			targetCharacter.TakeDamage(damage, character);
			
			Vector2 force = 400 * (targetCharacter.transform.position-character.transform.position).normalized;
			force.y += 200;
			targetCharacter.GetComponent<Rigidbody2D>().AddForce(force);
			break;
		}
	}
}

