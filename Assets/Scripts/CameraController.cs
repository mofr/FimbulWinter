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
	}

	void LateUpdate () {
		if (!target)
			return;

		Vector3 posNoZ = transform.position;
		posNoZ.z = target.transform.position.z;
			
		Vector3 targetDirection = (target.transform.position - posNoZ);
		Vector3 targetPos = transform.position + targetDirection.normalized * targetDirection.magnitude * interpVelocity * Time.deltaTime; 

		float cameraWidth = camera.orthographicSize * camera.aspect;
		targetPos.x = Mathf.Clamp (targetPos.x, limits.minX+cameraWidth, limits.maxX-cameraWidth);
		targetPos.y = Mathf.Clamp (targetPos.y, limits.minY+camera.orthographicSize, limits.maxY-camera.orthographicSize);
			
		transform.position = Vector3.Lerp (transform.position, targetPos, 0.25f) + offset;
	}
}