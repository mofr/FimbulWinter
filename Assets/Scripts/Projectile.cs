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
		Character target = collider.gameObject.GetComponent<Character>();
		if (target && target.enemy != originator.enemy)
		{
			target.TakeDamage(damage, originator);
			Vector2 pushForce = 400 * velocity.normalized;
			target.GetComponent<Rigidbody2D>().AddForce(pushForce);
			Destroy(gameObject);
		}
	}
}

