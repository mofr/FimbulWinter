using UnityEngine;
using System.Collections;

public class RangedAttack : Attack
{
	public float damage;
	public float speed = 10;
	public GameObject projectilePrefab;

	[HideInInspector]
	public Transform target;

	public override void Perform ()
	{
		GameObject projectile = Instantiate (projectilePrefab, transform.position, new Quaternion()) as GameObject;
		Destroy (projectile, 10);

		Collider2D targetCollider = target.GetComponent<Collider2D> ();

		Vector3 diff = targetCollider.bounds.center - transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		projectile.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

		projectile.GetComponent<Projectile> ().velocity = speed * (target.position - projectile.transform.position).normalized;
		projectile.GetComponent<Projectile> ().originator = character;
		projectile.GetComponent<Projectile> ().damage = damage;
	}
}

