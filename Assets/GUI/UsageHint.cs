using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UsageHint : MonoBehaviour {

	Text text;

	static UsageHint instance;
	
	void Start () {
		instance = this;
		text = GetComponent<Text> ();
	}

	static public void Show() {
		instance.text.enabled = true;
	}

	static public void Hide() {
		instance.text.enabled = false;
	}
}
