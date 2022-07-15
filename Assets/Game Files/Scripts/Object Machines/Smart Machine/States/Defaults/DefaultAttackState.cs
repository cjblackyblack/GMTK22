using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SmartState/DefaultAttack")]
public class DefaultAttackState : SmartState
{
	public override void HandleState(SmartObject smartObject)
	{
		base.HandleState(smartObject);
		if (smartObject.currentTime >= 0)
			smartObject.stateMachine.ChangeState(StateEnums.Action); //might have to change to job 
	}
}