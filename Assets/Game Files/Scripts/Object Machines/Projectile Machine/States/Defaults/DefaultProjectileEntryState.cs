using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProjectileState/EntryState")]
public class DefaultProjectileEntryState : ProjectileState
{
	public ProjectileState travelState;

	[SerializeField]
	private AnimationCurve forwardMovement;
	[SerializeField]
	private AnimationCurve verticalMovement;
	[SerializeField]
	private AnimationCurve strafeMovement;

	public override void OnEnter(ProjectileObject projectileObject)
	{
		projectileObject.activeTime = 0;
		projectileObject.hitID = Random.Range(0, 10000);
	}

	public override void OnUpdate(ProjectileObject projectileObject)
	{

	}

	public override void OnFixedUpdate(ProjectileObject projectileObject)
	{
		HandleState(projectileObject);
	}

	public override void OnExit(ProjectileObject projectileObject)
	{

	}

	public override void HandleState(ProjectileObject projectileObject)
	{
		if (projectileObject.currentTime > maxTime)
			projectileObject.stateMachine.ChangeState(travelState);
	}
}