using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Rigidbody rbody => GetComponent<Rigidbody>();
	public Transform LineCollider;
	public Transform TriangleCollider;

	public bool started;
	private void Start()
	{
		StartCoroutine(StartObject());
	}

	IEnumerator StartObject()
	{
		yield return new WaitUntil(() => started == true);
		PlayerManager.current.playerController = this;
		PlayerManager.current.SetFormation(0);
	}
	public void Update()
	{
		if (!started)
			return;

		foreach(SmartObject smartObject in PlayerManager.current.Party)
			smartObject._inputDir = PlayerManager.current.input;
	}

	public void FixedUpdate()
	{
		if (!started)
			return;

		foreach (SmartObject smartObject in PlayerManager.current.Party)
		{
			if (smartObject.stateMachine.currentStateEnum != StateEnums.Action)
				if (smartObject._inputDir != Vector2.zero)
					smartObject.storedInput = smartObject._inputDir;
			if (smartObject.stateMachine.currentStateEnum == StateEnums.Move || smartObject.stateMachine.currentStateEnum == StateEnums.Idle)
				if (PlayerManager.current.buttonBuffer[0] > 0)
					smartObject.stateMachine.ChangeState(StateEnums.Action);
		}

		for (int i = 0; i < PlayerManager.current.buttonBuffer.Length; i++)
			if (PlayerManager.current.buttonBuffer[i] > 0)
				PlayerManager.current.buttonBuffer[i]--;

		for (int i = 0; i < PlayerManager.current.buttonReleaseBuffer.Length; i++)
			if (PlayerManager.current.buttonReleaseBuffer[i] > 0)
				PlayerManager.current.buttonReleaseBuffer[i]--;


		LineCollider.transform.localEulerAngles = Vector3.Slerp(LineCollider.transform.localEulerAngles, new Vector3(0, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].ColliderRotation, 0), 0.1f);
		TriangleCollider.transform.localEulerAngles = Vector3.Slerp(TriangleCollider.transform.localEulerAngles, new Vector3(0, PlayerManager.current.PlayerFormations[PlayerManager.current.currentFormation].ColliderRotation, 0), 0.1f);
	}
}