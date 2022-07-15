using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StatusEffect/Poison Effect")]
public class PoisonEffect : StatusEffect
{
	public int damage;

	public override void OnEnter(SmartObject smartObject)
	{
		OnTick(smartObject);
	}

	public override void OnTick(SmartObject smartObject)
	{
		DamageInstance damageInstance = new DamageInstance(smartObject ,null, damage, 0, Vector2.zero, 0, false, 0, true, false, true);
		smartObject.TakeDamage(damageInstance);
	}
}