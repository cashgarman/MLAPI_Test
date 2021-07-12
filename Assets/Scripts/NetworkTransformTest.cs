using System;
using MLAPI;
using UnityEngine;

public class NetworkTransformTest : NetworkBehaviour
{
	void Update()
	{
		if (IsClient)
		{
			transform.position = new Vector3(Mathf.Cos(Time.time), 1.0f, Mathf.Sin(Time.time));
		}
	}
}