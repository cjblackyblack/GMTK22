using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Vector3 PositionOffset;
	public Rigidbody rbody => GetComponent<Rigidbody>();

	public void Update()
	{
		foreach(SmartObject smartObject in PlayerManager.current.Party)
			smartObject._inputDir = PlayerManager.current.input;
	}

	public void FixedUpdate()
	{
		foreach (SmartObject smartObject in PlayerManager.current.Party)
		{
			if (smartObject.stateMachine.currentStateEnum != StateEnums.Action)
				if (smartObject._inputDir != Vector2.zero)
					smartObject.storedInput = smartObject._inputDir;
			if (smartObject.stateMachine.currentStateEnum == StateEnums.Move || smartObject.stateMachine.currentStateEnum == StateEnums.Idle)
				if (PlayerManager.current.buttonBuffer[0] > 0)
					smartObject.stateMachine.ChangeStateOverride(smartObject.job.Attack);
		}

		for (int i = 0; i < PlayerManager.current.buttonBuffer.Length; i++)
			if (PlayerManager.current.buttonBuffer[i] > 0)
				PlayerManager.current.buttonBuffer[i]--;

		for (int i = 0; i < PlayerManager.current.buttonReleaseBuffer.Length; i++)
			if (PlayerManager.current.buttonReleaseBuffer[i] > 0)
				PlayerManager.current.buttonReleaseBuffer[i]--;
	}
}
