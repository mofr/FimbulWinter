using UnityEngine;
using UnityEditor;
using System.Collections;

public static class PlayFromViewport {

	static public Vector3 TakePosition(Vector3 defaultValue) {
		bool enabled = EditorPrefs.GetBool ("Tools.PlayFromViewPort.enabled", false);
		if (!enabled) {
			return defaultValue;
		}
		Vector3 pos = new Vector3 (EditorPrefs.GetFloat ("Tools.PlayFromViewPort.x", 0),
		                           EditorPrefs.GetFloat ("Tools.PlayFromViewPort.y", 0));
		EditorPrefs.SetBool ("Tools.PlayFromViewPort.enabled", false);
		return pos;
	}

	[MenuItem("Tools/Play from view port")]
	static public void Enable() {
		EditorPrefs.SetBool ("Tools.PlayFromViewPort.enabled", true);
		EditorPrefs.SetFloat ("Tools.PlayFromViewPort.x", SceneView.lastActiveSceneView.pivot.x);
		EditorPrefs.SetFloat ("Tools.PlayFromViewPort.y", SceneView.lastActiveSceneView.pivot.y);

		EditorApplication.isPlaying = true;
	}
}
