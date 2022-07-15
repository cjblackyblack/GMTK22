using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SmartState/HurtState")]
public class HurtState : SmartState
{
	public int maxTime;
	public AudioClip hurtFX;
	public AnimationCurve frictionCurve;


	public override void OnEnter(SmartObject smartObject)
	{
		base.OnEnter(smartObject);
		if (smartObject.stats.HP <= 0)
		{
			smartObject.properties.objectTangibility = PhysicalObjectTangibility.Intangible;
		}
		smartObject.anim.Play("Hurt", 0, 0);
		//smartObject.audioSource.PlayOneShot(hurtFX);
	}

	public override void OnFixedUpdate(SmartObject smartObject)
	{
		base.OnFixedUpdate(smartObject);
		smartObject.velocity *= frictionCurve.Evaluate(smartObject.currentTime);
		if(smartObject.currentTime < Mathf.RoundToInt(maxTime * 0.5f))
			smartObject.transform.position = smartObject.hurtPos;
	}

	public override void HandleState(SmartObject smartObject)
	{
		if (smartObject.currentTime > maxTime)
		{
			if (smartObject.stats.HP <= 0)
				smartObject.stateMachine.ChangeState(StateEnums.Dead);
			else if (smartObject.stats.HP > 0)
				smartObject.stateMachine.ChangeState(StateEnums.Idle);
		}
	}
}
