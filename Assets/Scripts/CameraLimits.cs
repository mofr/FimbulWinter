using UnityEngine;
using System.Collections;

public class CameraLimits : MonoBehaviour
{
	public float minX = -9.6f;
	public float maxX = 9.6f;
	public float minY = -5.4f;
	public float maxY = 5.4f;

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube (new Vector2((maxX+minX)/2, (maxY+minY)/2), 
		                     new Vector2(maxX-minX, maxY-minY));
	}
}

