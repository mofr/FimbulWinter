using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	public GameObject target;
	public float interpVelocity = 5f;
	public Vector2 offset;
	public CameraLimits limits;

	Camera camera;

	void Start () {
		camera = GetComponent<Camera> ();

		if (!target) {
			target = GameObject.FindWithTag("Player");
		}

		transform.position = CalcTargetPos();
	}

	public void SetLimits(CameraLimits limits) {
		if (this.limits != limits) {
			this.limits = limits;
			transform.position = CalcTargetPos ();
		}
	}

	void LateUpdate () {
		Vector3 targetPos = CalcTargetPos ();
		transform.position = Vector3.Lerp (transform.position, targetPos, interpVelocity * Time.deltaTime);
	}

	Vector3 CalcTargetPos() {
		Vector3 targetPos = target.transform.position; 
		targetPos.z = transform.position.z;
		targetPos += (Vector3)offset;
		
		//clamp targetPos to limits
		float cameraWidth = camera.orthographicSize * camera.aspect;
		targetPos.x = Mathf.Clamp (targetPos.x, limits.minX+cameraWidth, limits.maxX-cameraWidth);
		targetPos.y = Mathf.Clamp (targetPos.y, limits.minY+camera.orthographicSize, limits.maxY-camera.orthographicSize);

		return targetPos;
	}
}