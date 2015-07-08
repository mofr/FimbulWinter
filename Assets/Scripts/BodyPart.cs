using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	int collisionCount = 0;

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Dispose() {
		StartCoroutine (Dispose (1, 2));
	}

	IEnumerator Dispose(float pause, float duration) {
		yield return new WaitForSeconds (pause);

		float startTime = Time.time;
		Color color = spriteRenderer.color;

		while (color.a > 0) {
			spriteRenderer.color = color;
			color.a = 1 - (Time.time - startTime) / duration;
			yield return null;
		}

		Destroy (gameObject);
	}

	void OnCollisionEnter2D() {
		collisionCount += 1;
		if (collisionCount > 2) {
			GetComponent <Collider2D>().enabled = false;
		}
	}
}

