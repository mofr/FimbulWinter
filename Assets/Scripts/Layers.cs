using UnityEngine;
using System.Collections;

public class Layers
{
	static public int ground = LayerMask.NameToLayer("Ground");
	static public int groundMask = 1 << ground;

	static public int characters = LayerMask.NameToLayer("Characters");
	static public int platform = LayerMask.NameToLayer("Platform");
}

