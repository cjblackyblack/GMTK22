using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : PhysicalObject
{
	public ProjectileStateMachine stateMachine => GetComponent<ProjectileStateMachine>();
	public bool local;
	public ProjectileState entryState;
	public SmartObject origin;
	public TangibleObject target;
	public Vector2 inputDir;
	public bool drawHitboxes;
	public GambitTarget targetAlliance;
	public int hitCount;
	private void Update()
	{
		if (!GameManager.current.started)
			return;

		stateMachine.OnUpdate();
	}

	private void FixedUpdate()
	{
		if (!GameManager.current.started)

			return;
		activeTime += (stats.scaledTime);
		if (activeTime - currentTime >= 1)
		{
			stateMachine.OnFixedUpdate();
			rbody.velocity = velocity;

			currentTime = Mathf.RoundToInt(activeTime);
		}
	}

	public override PhysicalObjectTangibility TakeDamage(DamageInstance damageInstance)
	{
		switch (properties.objectTangibility)
		{
			case PhysicalObjectTangibility.Normal:
				{
					stats.HP -= Mathf.FloorToInt(damageInstance.damage);
				}
				break;
			case PhysicalObjectTangibility.Armor:
				{
					stats.HP -= Mathf.FloorToInt(damageInstance.damage);
				}
				break;
			case PhysicalObjectTangibility.Guard:
				{
					if (damageInstance.armorPierce)
					{
						stats.HP -= Mathf.FloorToInt(damageInstance.damage);
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
		return properties.objectTangibility;
	}

	private void OnDrawGizmos()
	{
		if (drawHitboxes)
		{
			Gizmos.color = new Color(255, 0, 0, 0.5f);
			DefaultProjectileTravelState projectileState = stateMachine.currentState as DefaultProjectileTravelState;
			if (projectileState)
				for (int j = 0; j < projectileState.hitboxTime.Length; j++)
					if (currentTime >= projectileState.hitboxTime[j].x && currentTime <= projectileState.hitboxTime[j].y)
					{
						Gizmos.DrawSphere(projectileState.hitboxPos[j].RotateVector(inputDir) + transform.position, projectileState.hitboxSize[j]); // + ((statMods.rangeMod - 1) * 0.25f));
						Gizmos.color = new Color(255, 255, 255, 0.75f);
						Gizmos.DrawWireSphere(projectileState.hitboxPos[j].RotateVector(inputDir) + transform.position, projectileState.hitboxSize[j]); // + ((statMods.rangeMod - 1) * 0.25f));
					}
		}
	}
}