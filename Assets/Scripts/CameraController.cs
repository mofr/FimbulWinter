using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float interpVelocity = 5f;
	public Vector2 offset;

	Camera camera;
	CameraLimits limits;
	Transform targetParent;

	void Start () {
		camera = GetComponent<Camera> ();

		if (!target) {
			target = GameObject.FindWithTag("Player").transform;
		}

		targetParent = target.parent;
		limits = target.parent.GetComponentInParent<CameraLimits>();
		transform.position = CalcTargetPos();
	}

	void LateUpdate () {
		if (target.parent != targetParent) {
			targetParent = target.parent;
			limits = target.parent.GetComponentInParent<CameraLimits>();
			transform.position = CalcTargetPos();
		} else {
			Vector3 targetPos = CalcTargetPos ();
			transform.position = Vector3.Lerp (transform.position, targetPos, interpVelocity * Time.deltaTime);
		}
	}

	Vector3 CalcTargetPos() {
		Vector3 targetPos = target.position; 
		targetPos.z = transform.position.z;
		targetPos += (Vector3)offset;
		
		//clamp targetPos to limits
		float cameraWidth = camera.orthographicSize * camera.aspect;
		targetPos.x = Mathf.Clamp (targetPos.x, limits.minX+cameraWidth, limits.maxX-cameraWidth);
		targetPos.y = Mathf.Clamp (targetPos.y, limits.minY+camera.orthographicSize, limits.maxY-camera.orthographicSize);

		return targetPos;
	}
}