using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	public Transform darkWaves;
	public Transform lightWaves;

	public float length = 2f;

	public float darkAnimDuration = 7f;
	public AnimationCurve darkAnim;

	public float lightAnimDuration = 10f;
	public AnimationCurve lightAnim;

	public GameObject effectPrefab;

	Vector3 darkPos1;
	Vector3 lightPos1;
	
	void Start () {
		darkPos1 = darkWaves.position;
		lightPos1 = lightWaves.position;
	}

	void Update () {
		darkWaves.position = darkPos1 + new Vector3 (length, 0) * darkAnim.Evaluate((Time.time % darkAnimDuration)/darkAnimDuration);
		lightWaves.position = lightPos1 + new Vector3 (length, 0) * lightAnim.Evaluate((Time.time % lightAnimDuration)/lightAnimDuration);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Character character = collider.GetComponent<Character>();
		if (character) {
			character.KillSilently ();
		}

		collider.gameObject.SetActive (false);

		if (effectPrefab) {
			Vector2 position = new Vector2(collider.bounds.center.x, transform.position.y-0.5f);
			GameObject effect = Instantiate (effectPrefab, position, Quaternion.identity) as GameObject;
			Destroy (effect, 3);
		}
	}
}
