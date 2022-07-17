using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : SmartObject
{
	public EnemyTypes type;
	public AnimationCurve flyCurve;
	void Awake(){
		EnemyManager.enemyManager.enemyDict[type].currentNumber++;
	}
	public override void SetFacingDir(bool useVelocity)
	{
		
	}

	public override void ObjectUpdate()
	{
		rbody.velocity = velocity;
		transform.position = new Vector3(transform.position.x, flyCurve.Evaluate(currentTime), transform.position.z);
	}
}
