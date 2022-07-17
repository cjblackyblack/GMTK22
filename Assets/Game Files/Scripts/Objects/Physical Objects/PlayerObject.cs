using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : SmartObject
{
	public PlayerController playerController;
	[HideInInspector]
	public PartyElement PartyElement;
	public int PlayerPosition;
	public float lerpTime;

	public override IEnumerator StartObject()
	{
		playerController = GetComponentInParent<PlayerController>();
		SetPositionInFormation(transform.GetSiblingIndex());
		yield return new WaitForEndOfFrame();
		stateMachine.StartMachine(states);
		PlayerManager.current.Party[PlayerPosition] = this;
		PlayerManager.current.StaticParty[PlayerPosition] = this;
		PartyElement = UIManager.current.PartyElements[PlayerPosition];
		PartyElement.label.text = job.jobName;
		PartyElement.SetMaxHealth(stats.maxHP);
		PartyElement.UpdateHealth(stats.HP);
		playerController.started = true;
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

				transform.localPosition = Vector3.Slerp(transform.localPosition, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset.normalized, lerpTime);
				if (Vector3.Distance(transform.localPosition, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset.normalized) < 0.1f)
				{
					transform.localPosition = PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset.normalized;
				}

				if (stateMachine.currentStateEnum != StateEnums.Hurt && stateMachine.currentStateEnum != StateEnums.Dead)
					playerController.rbody.velocity = velocity;
			}
		}

			anim.SetBool("Moving", stateMachine.currentStateEnum == StateEnums.Idle && (Vector3.Distance(transform.localPosition, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset.normalized) > 0.1f));
		
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

		for (int i = 0; i < PlayerManager.current.StaticParty.Length; i++)
		{
			if (PlayerManager.current.StaticParty[i] == this)
				PlayerManager.current.storedHP[i] = stats.HP;

		}

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
		PartyElement.UpdateHealth(stats.HP);
		return properties.objectTangibility;
	}

	public override void SetJob(CharacterJob newJob, bool restoreHP)
	{
		//if (!job) return;
		//SET STATS HERE
		job = newJob;
		stats.maxHP = job.jobStats.maxHP;
		if (restoreHP)
		{
			stats.HP = stats.maxHP;
			for (int i = 0; i < PlayerManager.current.StaticParty.Length; i++)
			{
				if (PlayerManager.current.StaticParty[i] == this)
					PlayerManager.current.storedHP[i] = stats.HP;

			}
		}
		else
			for (int i = 0; i < PlayerManager.current.StaticParty.Length; i++)
			{
				if (PlayerManager.current.StaticParty[i] == this)
					stats.HP = PlayerManager.current.storedHP[i];

			}
		if (stats.HP == 0)
			stats.HP = 2;

		spriteRenderer.sprite = job.sprite;
		anim.runtimeAnimatorController = job.animator;

		PartyElement.label.text = job.jobName;
		PartyElement.SetMaxHealth(stats.maxHP);
		PartyElement.UpdateHealth(stats.HP);
		stateMachine.ChangeState(StateEnums.Idle);
	}

	public override void SetFacingDir(bool useVelocity)
	{
		facingDir = PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].FacingDir;

		anim.SetFloat("xDir", Mathf.RoundToInt(facingDir.x));
		anim.SetFloat("yDir", Mathf.RoundToInt(facingDir.y));
	}

	public void SetPositionInFormation(int index)
	{
		PlayerPosition = index;
		transform.SetSiblingIndex(PlayerPosition);
	}

}
