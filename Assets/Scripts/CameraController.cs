using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float interpVelocity = 5f;
	public Vector2 offset;

	CameraLimits _limits;
	public CameraLimits limits {
		set {
			if(_limits != value) {
				_limits = value;
				transform.position = CalcTargetPos();
			}
		}
		get {
			return _limits;
		}
	}

	Camera camera;
	Vector3 lastTargetPos;

	void Start () {
		camera = GetComponent<Camera> ();

		if (!target) {
			target = GameObject.FindWithTag("Player").transform;
		}
	}

	void LateUpdate () {
		if (limits) {
			Vector3 targetPos = CalcTargetPos ();
			transform.position = Vector3.Lerp (transform.position, targetPos, interpVelocity * Time.deltaTime);
		} else {
			transform.position = target.position;
		}
	}

	Vector3 CalcTargetPos() {
		Vector3 targetPos = target.position;
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