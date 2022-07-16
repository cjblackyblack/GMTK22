using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(100)]
public class GrassObject : TangibleObject
{
	private void Start()
	{
		InitHurtID();
		if (GetComponent<SpriteRenderer>())
			transform.position += new Vector3(Random.Range(-0.05f, 0.05f), 0, Random.Range(-0.2f, 0.2f));
		GameManager.current.Grass.Add(this);
	}
}