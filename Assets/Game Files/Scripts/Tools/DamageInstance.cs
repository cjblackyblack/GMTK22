using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageInstance 
{
	public readonly List<StatusEffect> statusEffects;
	public readonly PhysicalObject origin;
	public readonly float damage;
	public float knockbackStrength;
	public readonly Vector2 knockback; 
	public readonly float hitStopTime; 
	public readonly bool armorPierce;
	public readonly int hitStun;
	public readonly bool flatDamage;
	public readonly bool ignoreProtections;
	public readonly bool useMagic;

	public DamageInstance(PhysicalObject _origin, List<StatusEffect> _statusEffects, float _damage, float _knockbackStrength, Vector2 _knockback, float _hitStopTime, bool _armorPierce, int _hitStun, bool _flatDamage, bool _ignoreProtections, bool _useMagic)
	{
		origin = _origin;
		statusEffects = _statusEffects;
		damage = _damage;
		knockbackStrength = _knockbackStrength;
		knockback = _knockback;
		hitStopTime = _hitStopTime;
		armorPierce = _armorPierce;
		hitStun = _hitStun;
		flatDamage = _flatDamage;
		ignoreProtections = _ignoreProtections;
		useMagic = _useMagic;
	}
}