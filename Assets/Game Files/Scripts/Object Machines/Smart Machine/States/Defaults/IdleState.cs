using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SmartState/IdleState")]
public class IdleState : SmartState
{
	public override void OnEnter(SmartObject smartObject)
	{
		base.OnEnter(smartObject);
		smartObject._inputDir = Vector2.zero;
		smartObject.inputDir = Vector2.zero;
		smartObject.anim.SetBool("Moving", false);
		smartObject.anim.Play("Idle", 0, 0);
	}

	public override void OnUpdate(SmartObject smartObject)
	{
		base.OnUpdate(smartObject);
		smartObject.SetFacingDir(false);
	}

	public override void HandleState(SmartObject smartObject)
	{
		if (smartObject.inputDir != Vector3.zero)
			smartObject.stateMachine.ChangeState(StateEnums.Move);
	}
}