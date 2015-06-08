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
		scale.x = character.health / 100f;
		healthBar.transform.localScale = scale;
	}
}
