using UnityEngine;
using System.Collections;

public class Damage
{
	public float amount;
	public Character originator;

	public Damage(float amount, Character originator)
	{
		this.amount = amount;
		this.originator = originator;
	}
}

