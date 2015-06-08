using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	
	public Character character;
	public RectTransform healthBar;

	void Start() {
		if (!character) {
			character = GameObject.FindWithTag ("Player").GetComponent<Character> ();
		}
	}

	void LateUpdate () {
		Vector3 scale = healthBar.transform.localScale;
		scale.x = Mathf.Clamp (character.health / 100f, 0f, 1f);
		healthBar.transform.localScale = scale;
	}
}
