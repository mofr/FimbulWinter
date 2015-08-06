using UnityEngine;
using System.Collections;

public class flickering : MonoBehaviour {
	private float intensity=0f;
	private float t=0f;
	// Use this for initialization
	void Start () {
	intensity = this.GetComponent<Light>().intensity;
	}
	
	// Update is called once per frame
	void Update () {
	t+=Time.deltaTime;
		this.GetComponent<Light>().intensity = intensity + Mathf.Sin(t*(1-Mathf.Sin(t*25f))*5f)*intensity/5f;
	}
}
