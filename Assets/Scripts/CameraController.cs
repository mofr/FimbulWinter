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
	Vector3 lastTargetPos;

	void Start () {
		camera = GetComponent<Camera> ();

		if (!target) {
			target = GameObject.FindWithTag("Player").transform;
		}

		targetParent = target.parent;
		lastTargetPos = target.position;
		limits = target.parent.GetComponentInParent<CameraLimits>();
		transform.position = CalcTargetPos();
	}

	void LateUpdate () {
		CameraLimits newLimits = limits;

		if (target) {
			lastTargetPos = target.position;
		}

		if (target && target.parent != targetParent) {
			targetParent = target.parent;
			newLimits = target.parent.GetComponentInParent<CameraLimits> ();
		}

		if(newLimits != limits) {
			limits = newLimits;
			transform.position = CalcTargetPos();
		} else {
			Vector3 targetPos = CalcTargetPos ();
			transform.position = Vector3.Lerp (transform.position, targetPos, interpVelocity * Time.deltaTime);
		}
	}

	Vector3 CalcTargetPos() {
		Vector3 targetPos = lastTargetPos; 
		targetPos.z = transform.position.z;
		targetPos += (Vector3)offset;

		//clamp targetPos to limits
		Vector3 cameraSize = new Vector3 (camera.orthographicSize * camera.aspect, camera.orthographicSize);
		Vector3 min = limits.worldMin + cameraSize;
		Vector3 max = limits.worldMax - cameraSize;
		targetPos.x = Mathf.Clamp (targetPos.x, min.x, max.x);
		targetPos.y = Mathf.Clamp (targetPos.y, min.y, max.y);

		return targetPos;
	}
}