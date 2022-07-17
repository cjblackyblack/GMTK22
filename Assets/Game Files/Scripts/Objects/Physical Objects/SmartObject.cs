using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmartObject : PhysicalObject
{

	public StateMachine stateMachine => GetComponent<StateMachine>();
	public EffectMachine effectMachine => GetComponent<EffectMachine>();

	public List<SmartState> states;

	public Vector2 facingDir = new Vector2(1, 0);
	public Vector2 _inputDir; //(X, 0, Y) for Vector2 Conversions (like a playerInput)
	public Vector3 inputDir;
	public Vector2 storedInput;


	public Action<SmartObject, StateEnums> OnStateChange;
	public Action<SmartObject, SmartState> OnStateChangeOverride;

	public CharacterJob job;

	public Vector3 targetPos;
	[HideInInspector]
	public Vector3 hurtPos;

	public bool drawHitboxes;
	public void Start()
	{
		InitHurtID();
		StartCoroutine(StartObject());
	}
	public virtual IEnumerator StartObject()
	{
		yield return new WaitForEndOfFrame();
		stateMachine.StartMachine(states);
	}

	private void Update()
	{

		inputDir = (_inputDir.Rotate(statMods.inputMod)).ConvertVector2();
		stateMachine.OnUpdate();
	}

	private void FixedUpdate()
	{

		activeTime += (stats.scaledTime * statMods.timeMod);
		if (activeTime - currentTime >= 1)
		{
			effectMachine.OnFixedUpdate();
			stateMachine.OnFixedUpdate();
			ObjectUpdate();
		}
		velocity = new Vector3(velocity.x * friction, 0, velocity.z * friction);
		currentTime = Mathf.RoundToInt(activeTime);
	}

	public override PhysicalObjectTangibility TakeDamage(DamageInstance damageInstance)
	{
		if (damageInstance.hitStun > 0)
		{
			switch (properties.objectTangibility)
			{
				case PhysicalObjectTangibility.Normal:
					{
						effectMachine.OnTakeDamage(damageInstance);
						stateMachine.ChangeState(StateEnums.Hurt);
						velocity = (transform.position - damageInstance.origin.transform.position).normalized * damageInstance.knockbackStrength;
						hurtPos = transform.position;
						transform.parent = null;
						stats.HP -= (int)damageInstance.damage;
					}
					break;
				case PhysicalObjectTangibility.Armor:
					{
						effectMachine.OnTakeDamage(damageInstance);
						stats.HP -= (int)damageInstance.damage;

						if (damageInstance.armorPierce)
						{
							velocity = (transform.position - damageInstance.origin.transform.position).normalized * damageInstance.knockbackStrength;
							stateMachine.ChangeState(StateEnums.Hurt);
							hurtPos = transform.position;
							transform.parent = null;
						}
					}
					break;
				case PhysicalObjectTangibility.Guard:
					{
						if (damageInstance.armorPierce)
						{
							effectMachine.OnTakeDamage(damageInstance);
							velocity = (transform.position - damageInstance.origin.transform.position).normalized * damageInstance.knockbackStrength;
							stateMachine.ChangeState(StateEnums.Hurt);
							hurtPos = transform.position;
							transform.parent = null;
							stats.HP -= (int)damageInstance.damage;
						}
					}
					break;
				case PhysicalObjectTangibility.Invincible:
					{

					}
					break;
				case PhysicalObjectTangibility.Intangible:
					{

					}
					break;
			}
		}
		else
		{
			if (properties.objectTangibility == PhysicalObjectTangibility.Invincible && damageInstance.damage > 0)
				return properties.objectTangibility;
			OnStateChange?.Invoke(this, StateEnums.Hurt);
		}

		stats.HP = Mathf.Clamp(stats.HP, 0, stats.maxHP);

		if (stats.HP <= 0 && stateMachine.currentStateEnum != StateEnums.Dead) //we need to die (ie poisoned to death)
		{
			SmartObject smartOrigin = damageInstance.origin as SmartObject;
			if (smartOrigin != null)
			{
				//Reward XP
			}
			stateMachine.ChangeState(StateEnums.Hurt);
		}
		else if (stats.HP > 0 && stateMachine.currentStateEnum == StateEnums.Dead && damageInstance.damage < 0)//we were dead but have been healed
		{
			stateMachine.ChangeState(StateEnums.Idle);
		}

		return properties.objectTangibility;
	}

	public virtual void SetFacingDir(bool useVelocity)
	{

	}

	public virtual void SetJob(CharacterJob newJob, bool restoreHP)
	{
		job = newJob;
		SetStats(restoreHP);
	}

	public void SetStats(bool restoreHP)
	{
		if (!job) return;
		//SET STATS HERE
		stats.maxHP = job.jobStats.maxHP;
		if (restoreHP)
			stats.HP = stats.maxHP;
		spriteRenderer.sprite = job.sprite;
		anim.runtimeAnimatorController = job.animator;
	}
	public IEnumerator WaitDestroyObject()
	{
		yield return new WaitForFixedUpdate();
		Destroy(this.gameObject);
	}

	private void OnDrawGizmos()
	{
		if (drawHitboxes)
		{
			Gizmos.color = new Color(255, 0, 0, 0.5f);
			AttackState attackState = stateMachine.currentState as AttackState;
			if (attackState)
				for (int j = 0; j < attackState.hitboxTime.Length; j++)
					if (currentTime >= attackState.hitboxTime[j].x && currentTime <= attackState.hitboxTime[j].y)
					{
						Gizmos.DrawSphere(attackState.hitboxPos[j].RotateVector(facingDir) + transform.position, attackState.hitboxSize[j]); // + ((statMods.rangeMod - 1) * 0.25f));
						Gizmos.color = new Color(255, 255, 255, 0.75f);
						Gizmos.DrawWireSphere(attackState.hitboxPos[j].RotateVector(facingDir) + transform.position, attackState.hitboxSize[j]); // + ((statMods.rangeMod - 1) * 0.25f));
					}
		}
	}

	private void OnValidate()
	{
		SetStats(false);
	}
}