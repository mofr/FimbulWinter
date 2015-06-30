using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
	static ScreenFader instance;

	RawImage overlay;
	float t = 0;
	float speed = 0;

	void Awake() {
		instance = this;

		GameObject overlayObject = new GameObject ("ScreenFader");
		overlay = overlayObject.AddComponent<RawImage>();
		overlay.color = new Color (0, 0, 0, 0.0f);

		Canvas canvas = GameObject.Find ("Canvas").GetComponent<Canvas>();
		overlay.transform.SetParent (canvas.transform);
		overlay.transform.localScale = new Vector3 (1,1,1);
		overlay.rectTransform.anchorMin = new Vector2 (0, 0);
		overlay.rectTransform.anchorMax = new Vector2 (1, 1);
	}

	void Start() {
		ScreenFader.FadeToClear ();
	}

	public static void FadeToBlack(float duration = 1f) {
		instance.speed = -1/duration;
	}

	public static void FadeToClear(float duration = 1f) {
		instance.speed = 1/duration;
	}
	
	void Update ()
	{
		if (speed != 0) {
			t += speed * Time.deltaTime;
			if (t < 0 || t > 1) {
				speed = 0;
				t = Mathf.Clamp (t, 0, 1);
			}
			overlay.color = Color.Lerp (Color.black, Color.clear, t);
		}
	}
}

