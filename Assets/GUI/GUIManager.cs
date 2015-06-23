using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
	public static GUIManager instance;

	public Canvas usageKeyPrefab;

	void Awake() {
		instance = this;
	}
}

