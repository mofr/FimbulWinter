using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour
{

	void Start ()
	{
		Character character = GameObject.FindWithTag ("Player").GetComponent<Character>();
		character.onDeath += OnDeath;
	}

	void OnDeath() {
		StartCoroutine (Restart ());
	}
	
	IEnumerator Restart() {
		ScreenFader.FadeToBlack (4);
		yield return new WaitForSeconds (4);
		Application.LoadLevel (Application.loadedLevel);
	}
}

