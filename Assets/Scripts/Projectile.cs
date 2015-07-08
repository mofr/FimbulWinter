using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	[HideInInspector]
	public Vector2 velocity;

	[HideInInspector]
	public Character originator;

	[HideInInspector]
	public float damage;
	
	void Start ()
	{
	
	}

	void FixedUpdate ()
	{
		transform.position += (Vector3) velocity * Time.deltaTime;
		transform.Rotate (0, 0, 1200 * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject == originator.gameObject)
			return;

		Damageable target = collider.GetComponent<Damageable>();
		if (target)
		{
			target.TakeDamage(damage, originator, transform.position);

			Rigidbody2D rigidbody = target.GetComponent<Rigidbody2D>();
			if(rigidbody) {
				Vector2 pushForce = 400 * velocity.normalized;
				rigidbody.AddForce(pushForce);
			}

			Destroy(gameObject);
		}
	}
}

