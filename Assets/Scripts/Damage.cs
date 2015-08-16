using UnityEngine;
using System.Collections;

public class Damage
{
	public float amount;
	public GameObject originator;
	public Vector3 point;

	public Damage(float amount, GameObject originator, Vector3 point)
	{
		this.amount = amount;
		this.originator = originator;
		this.point = point;
	}
}

