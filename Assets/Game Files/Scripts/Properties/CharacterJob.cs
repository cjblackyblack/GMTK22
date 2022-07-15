using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[CreateAssetMenu(menuName = "CharacterJob")]
public class CharacterJob : ScriptableObject
{
	public AttackState Attack;
	public AttackState SpecialAttack;
	public string jobName;
	[Header("Data")]
	public BaseStats jobStats;

	[Header("Visuals")]
	public RuntimeAnimatorController animator;
	public Sprite sprite;
}