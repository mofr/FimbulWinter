﻿using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider) {
		Character character = collider.gameObject.GetComponent<Character>();
		if (!character)
			return;

		character.Kill ();
	}
}