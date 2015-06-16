using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	public GameObject target;
	public float interpVelocity = 5f;
	public Vector3 offset;

	[Serializable]
	public class Limits {
		public float minX = -9.6f;
		public float maxX = 9.6f;
		public float minY = -5.4f;
		public float maxY = 5.4f;
	}

	public Limits limits;

	Camera camera;

	void Start () {
		camera = GetComponent<Camera> ();

		if (!target) {
			target = GameObject.FindWithTag("Player");
		}

		Vector3 pos = transform.position;
		pos.x = target.transform.position.x;
		pos.y = target.transform.position.y;
		transform.position = pos;
	}

	void LateUpdate () {
		if (!target)
			return;

		Vector3 targetPos = target.transform.position; 
		targetPos.z = transform.position.z;

		//clamp targetPos to limits
		float cameraWidth = camera.orthographicSize * camera.aspect;
		targetPos.x = Mathf.Clamp (targetPos.x, limits.minX+cameraWidth, limits.maxX-cameraWidth);
		targetPos.y = Mathf.Clamp (targetPos.y, limits.minY+camera.orthographicSize, limits.maxY-camera.orthographicSize);
			
		//interpolate
		transform.position = Vector3.Lerp (transform.position, targetPos, interpVelocity * Time.deltaTime) + offset;
	}
}