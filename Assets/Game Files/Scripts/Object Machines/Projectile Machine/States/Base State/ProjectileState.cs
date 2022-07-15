using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-10)]
public abstract class ProjectileState : ScriptableObject
{
	public int maxTime;
	public virtual void OnEnter(ProjectileObject projectileObject)
	{
		projectileObject.activeTime = 0;
	}

	public virtual void OnUpdate(ProjectileObject projectileObject)
	{

	}

	public virtual void OnFixedUpdate(ProjectileObject projectileObject)
	{
		HandleState(projectileObject);
	}

	public virtual void OnExit(ProjectileObject projectileObject)
	{

	}

	public virtual void HandleState(ProjectileObject projectileObject)
	{

	}
}