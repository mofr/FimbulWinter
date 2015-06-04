using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject window;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if(!window.activeSelf) {
				Pause();
			} else {
				Continue ();
			}
		}
	}

	public void Pause() {
		window.SetActive(true);
		Time.timeScale = 0.0f;
	}

	public void Continue() {
		window.SetActive(false);
		Time.timeScale = 1.0f;
	}

	public void Exit() {
		Application.Quit ();
	}
}
