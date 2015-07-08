using UnityEngine;
using System.Collections;

public class BodyExplosion
{
	public static bool Explode(GameObject gameObject, Vector2 dir) {
		BodyPart[] bodyParts = gameObject.GetComponentsInChildren<BodyPart>();
		if (bodyParts.Length == 0)
			return false;
		
		foreach(BodyPart bodyPart in bodyParts) {
			bodyPart.Dispose();
			bodyPart.gameObject.layer = Layers.bodyPart;
			bodyPart.transform.SetParent(null);
			bodyPart.gameObject.AddComponent<PolygonCollider2D>();
			Rigidbody2D rigidBody = bodyPart.gameObject.AddComponent<Rigidbody2D>();
			rigidBody.gravityScale = 0.5f;
			rigidBody.mass = 0.3f;
			rigidBody.velocity = 8 * dir + new Vector2(Random.Range(-3, 3), 0);
			rigidBody.angularVelocity = Random.Range(-360, 360)*5;
		}

		return true;
	}
}

