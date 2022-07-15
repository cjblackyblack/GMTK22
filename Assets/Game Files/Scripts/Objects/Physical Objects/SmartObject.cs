using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmartObject : PhysicalObject
{

	public PlayerController playerController;// => GetComponentInParent<PlayerController>();
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
	public bool drawHitboxes;

	public int PlayerPosition;
	public float lerpTime;

	public Vector3 targetPos;
	public Vector3 hurtPos;

	public void Start()
	{
		playerController = GetComponentInParent<PlayerController>();
		InitHurtID();
		StartCoroutine(StartObject());
	}
	IEnumerator StartObject()
	{

		yield return new WaitForEndOfFrame();
		stateMachine.StartMachine(states);

	}

	private void OnDestroy()
	{

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
			velocity.y += gravity;
			if (velocity.y < terminalYVel)
				velocity.y = terminalYVel;
			ObjectUpdate();



		}
		velocity = new Vector3(velocity.x * friction, velocity.y, velocity.z * friction);
		currentTime = Mathf.RoundToInt(activeTime);
	}


	public override void ObjectUpdate()
	{
		if (playerController == null && rbody != null)
			rbody.velocity = velocity;
		else if (playerController != null)
		{
			if (stateMachine.currentStateEnum != StateEnums.Hurt && stateMachine.currentStateEnum != StateEnums.Dead)
			{
				if (transform.parent == null && playerController != null)
				{
					transform.parent = playerController.transform;
					transform.SetSiblingIndex(PlayerPosition);
				}

				transform.localPosition = Vector3.Slerp(transform.localPosition, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset, lerpTime);
				if (Vector3.Distance(transform.localPosition, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset) < 0.25f)
					transform.localPosition = PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset;

				if (stateMachine.currentStateEnum != StateEnums.Hurt && stateMachine.currentStateEnum != StateEnums.Dead)
					playerController.rbody.velocity = velocity;
			}
		}
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
					}
					break;
				case PhysicalObjectTangibility.Armor:
					{
						effectMachine.OnTakeDamage(damageInstance);
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
			}
			stateMachine.ChangeState(StateEnums.Hurt);
		}
		else if (stats.HP > 0 && stateMachine.currentStateEnum == StateEnums.Dead && damageInstance.damage < 0)//we were dead but have been healed
		{
			stateMachine.ChangeState(StateEnums.Idle);
		}

		return properties.objectTangibility;
	}

	public void SetFacingDir(bool useVelocity)
	{
		if (playerController)
			facingDir = PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].FacingDir;

		anim.SetFloat("xDir", Mathf.RoundToInt(facingDir.x));
		anim.SetFloat("yDir", Mathf.RoundToInt(facingDir.y));
	}

	public void SetStats(bool setActions)
	{
		if (!job) return;
		//SET STATS HERE
		spriteRenderer.sprite = job.sprite;
		anim.runtimeAnimatorController = job.animator;
	}

	public void SetAlliance(Alliance alliance)
	{
		properties.alliance = alliance;
		properties.baseAlliance = alliance;
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
		if (!Application.isPlaying)
			SetStats(true);
		else
			SetStats(false);
	}
}