using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SmartState/DeadState")]
public class DeadState : SmartState
{
	public int maxTime;
	public GameObject deadFX;
	public GameObject powerUpPrefab;
	public int powerUpDC;
	public override void OnEnter(SmartObject smartObject)
	{
		base.OnEnter(smartObject);
		EnemyObject eobj = smartObject.GetComponent<EnemyObject>();
		if(eobj){
			EnemyManager.enemyManager.enemyDict[eobj.type].maxNumber--;
		}
		smartObject.velocity *= 0;
		smartObject.properties.objectTangibility = PhysicalObjectTangibility.Intangible;

		smartObject.anim.Play("Dead", 0, 0);

		if(powerUpPrefab && StateMachine.RollD20 > powerUpDC)
		{
			Instantiate(powerUpPrefab, smartObject.tform.position, Quaternion.identity);
		}
		if (deadFX != null)
			Instantiate(deadFX, smartObject.transform.position, Quaternion.identity);
	}

	public override void OnFixedUpdate(SmartObject smartObject)
	{
		base.OnFixedUpdate(smartObject);
		if (smartObject.currentTime > 60)
		{
			smartObject.currentTime = 0;
			smartObject.anim.Play("Dead", 0, 1);
		}
	}
}
