using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProjectileState/ExitState")]
public class DefaultProjectileExitState : ProjectileState
{
	public GameObject AllyFX;
	public GameObject EnemyFX;
	public override void OnEnter(ProjectileObject projectileObject)
	{
		projectileObject.activeTime = 0;
		if(projectileObject.origin.properties.baseAlliance == Alliance.Ally)
		{
			Instantiate(AllyFX, projectileObject.transform.position, projectileObject.transform.rotation);
		}
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
			Destroy(projectileObject.gameObject);
	}
}