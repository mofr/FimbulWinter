using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	public Sprite firstSprite;
	public Sprite[] loopingSprites;

	public float speed = 1.0f;
	public float yOffset = 0f;
	public float xOffset = 0f;

	GameObject bg1;
	GameObject bg2;

	SpriteRenderer renderer1;
	SpriteRenderer renderer2;
	
	void Start () {
		GameObject scrollingBackground = new GameObject ("Scrolling Background");

		bg1 = new GameObject ("bg1");
		bg1.transform.parent = scrollingBackground.transform;
		renderer1 = bg1.AddComponent<SpriteRenderer>();
		renderer1.sortingLayerName = "Background";
		renderer1.sprite = firstSprite;

		bg2 = new GameObject ("bg2");
		bg2.transform.parent = scrollingBackground.transform;
		renderer2 = bg2.AddComponent<SpriteRenderer>();
		renderer2.sortingLayerName = "Background";
		renderer2.sprite = randomSprite();

		bg1.transform.Translate (xOffset, yOffset, 0);
		bg2.transform.Translate (xOffset + renderer1.sprite.bounds.size.x/2 + renderer2.sprite.bounds.size.x/2 - 0.01f, yOffset, 0);
	}

	void Update () {
		float offset = Time.deltaTime * -speed;

		bg1.transform.Translate (offset, 0, 0);
		bg2.transform.Translate (offset, 0, 0);

		if (bg1.transform.position.x <= -renderer1.sprite.bounds.size.x) {
			Swap();
		}
	}

	Sprite randomSprite(Sprite permitted = null) {
		Sprite sprite;
		do {
			sprite = loopingSprites[Random.Range (0, loopingSprites.Length)];
		} while(sprite == permitted && loopingSprites.Length > 1);
		return sprite;
	}

	void Swap() {
		renderer1.sprite = randomSprite(renderer2.sprite);
		bg1.transform.position = bg2.transform.position + new Vector3 (renderer2.bounds.size.x/2 + renderer1.bounds.size.x/2 - 0.01f, 0, 0);

		GameObject tmpObject = bg1;
		bg1 = bg2;
		bg2 = tmpObject;

		SpriteRenderer tmpRenderer = renderer1;
		renderer1 = renderer2;
		renderer2 = tmpRenderer;
	}
}
