using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMachine : MonoBehaviour
{
	public SmartObject smartObject => GetComponent<SmartObject>();
	public List<EffectContainer> statusEffects;

	public SmartState OverrideState()
	{
		foreach (EffectContainer effectContainer in statusEffects)
			if (effectContainer.effect.overrideState != null)
				return effectContainer.effect.overrideState;
		return null;
	}

	public void OnFixedUpdate()
	{
		foreach (EffectContainer effectContainer in statusEffects)
			effectContainer?.OnFixedUpdate(smartObject);
	}

	public void OnTakeDamage(DamageInstance damageInstance)
	{
		if (damageInstance.statusEffects?.Count > 0)
			foreach (StatusEffect statusEffect in damageInstance.statusEffects)
				GetComponent<EffectMachine>().AddEffect(statusEffect);

		foreach (EffectContainer effectContainer in statusEffects)
			effectContainer?.OnTakeDamage(smartObject);
	}

	public void AddEffect(StatusEffect effect)
	{
		EffectContainer effectContainer = new EffectContainer();
		effectContainer.effect = effect;
		statusEffects.Add(effectContainer);
		effectContainer.OnAdd(smartObject);
	}

	public void RemoveEffect(EffectContainer effectContainer)
	{
		if (statusEffects.Contains(effectContainer))
			StartCoroutine(RemoveEffectWait(effectContainer));
	}

	IEnumerator RemoveEffectWait(EffectContainer effectContainer)
	{
		yield return new WaitForEndOfFrame();
		statusEffects.Remove(effectContainer);
		effectContainer.OnRemove(smartObject);
	}
}
