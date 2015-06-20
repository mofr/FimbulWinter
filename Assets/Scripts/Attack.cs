using UnityEngine;
using System.Collections;

public abstract class Attack : MonoBehaviour
{
	protected Character character;
	
	void Start()
	{
		character = GetComponentInParent<Character>();
	}

	abstract public void Perform ();
}

