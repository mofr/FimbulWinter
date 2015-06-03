using UnityEngine;
using System.Collections;

public class Cruise : MonoBehaviour {

	public Animator guiBackgroundAnim;
	public Animator ambientSoundAnim;

	bool stirTalked = false;
	bool yarlTalked = false;

	void Start() {
	}

	public void OnStirTalk() {
		stirTalked = true;
		Check ();
	}

	public void OnYarlTalk() {
		yarlTalked = true;
		Check ();
	}

	void Check() {
		if (yarlTalked && stirTalked) {
			StartCoroutine(Finish());
		}
	}

	IEnumerator Finish() {
		yield return new WaitForSeconds(1);
		guiBackgroundAnim.SetTrigger ("FadeIn");
		ambientSoundAnim.SetTrigger ("FadeOut");
		yield return new WaitForSeconds(4);
		Application.LoadLevel ("Beach");
	}
}
