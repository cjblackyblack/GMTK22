using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartState : ScriptableObject
{
	public bool interuptable;
	public virtual void OnEnter(SmartObject smartObject)
	{
		smartObject.activeTime = 0;
		smartObject.currentTime = 0;
	}

	public virtual void OnUpdate(SmartObject smartObject)
	{

	}

	public virtual void OnFixedUpdate(SmartObject smartObject)
	{
		HandleState(smartObject);
	}

	public virtual void OnExit(SmartObject smartObject)
	{

	}

	public virtual void HandleState(SmartObject smartObject)
	{

	}
}
