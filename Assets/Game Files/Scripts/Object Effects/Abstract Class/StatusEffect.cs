using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
	public int maxTime;
	public int tickRate;
	public SmartState overrideState;
	public virtual void OnEnter(SmartObject smartObject)
	{

	}

	public virtual void OnTakeDamage(SmartObject smartObject)
	{

	}

	public virtual void OnTick(SmartObject smartObject)
	{

	}

	public virtual void OnExit(SmartObject smartObject)
	{

	}
}
