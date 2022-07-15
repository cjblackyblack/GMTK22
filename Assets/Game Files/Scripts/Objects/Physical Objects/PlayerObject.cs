using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : SmartObject
{
	public PlayerController playerController;

	public int PlayerPosition;
	public float lerpTime;

	public override IEnumerator StartObject()
	{
		playerController = GetComponentInParent<PlayerController>();
		PlayerPosition = transform.GetSiblingIndex();
		yield return new WaitForEndOfFrame();
		stateMachine.StartMachine(states);
		PlayerManager.current.Party[PlayerPosition] = this;
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
				if (Vector3.Distance(transform.localPosition, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset.normalized) < 0.25f)
					transform.localPosition = PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].LocalOffset.normalized;

				if (stateMachine.currentStateEnum != StateEnums.Hurt && stateMachine.currentStateEnum != StateEnums.Dead)
					playerController.rbody.velocity = velocity;
			}
		}
	}

	public override void SetFacingDir(bool useVelocity)
	{
		facingDir = PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].playerPositions[PlayerPosition].FacingDir;

		anim.SetFloat("xDir", Mathf.RoundToInt(facingDir.x));
		anim.SetFloat("yDir", Mathf.RoundToInt(facingDir.y));
	}
}
