using UnityEngine;
using System.Collections;

public class Breath : MonoBehaviour {

	public GameObject particleSystem;
	public GameObject attachTo;

	public void OnBreathOut() {
		GameObject ps = Instantiate (particleSystem);
		if (attachTo) {
			ps.transform.SetParent(attachTo.transform, false);
		}
		Destroy (ps, 3);
	}
}
