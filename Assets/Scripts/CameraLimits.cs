using UnityEngine;
using System;
using System.Collections;

public class CameraLimits : MonoBehaviour
{
	public Vector2 min = new Vector2(-9.6f, -5.4f);
	public Vector2 max = new Vector2(9.6f, 5.4f);

	public Vector3 worldMin {
		get {
			return transform.position + (Vector3)min;
		}
	}

	public Vector3 worldMax {
		get {
			return transform.position + (Vector3)max;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Vector3 center = (min + max)/2;
		Vector3 size = max - min;
		Gizmos.DrawWireCube (center + transform.position, size);
	}
}

