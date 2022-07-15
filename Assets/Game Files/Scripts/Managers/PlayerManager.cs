using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager current;
	public Vector2 input;
	public int[] buttonBuffer;
	public int[] buttonReleaseBuffer;
	public GambitTarget hittable;

	public SmartObject[] Party;
	public SmartObject PartyLeader;

	public int currentFormation;
	public PlayerFormations[] PlayerFormations;

	private void Start()
	{
		if (current != null && current != this)
		{ Destroy(this.gameObject); return; }

		current = this;

	}

	public void BufferMovement(InputAction.CallbackContext ctx)
	{
		input = ctx.ReadValue<Vector2>();
	}

	public void BufferInput(int input)
	{
		buttonBuffer[input] = 6;
	}

	public void BufferRelease(int input)
	{
		buttonReleaseBuffer[input] = 6;
	}
}


[System.Serializable]
public class PlayerFormations
{
	public PlayerPositions[] playerPositions;
}

[System.Serializable]
public class PlayerPositions 
{
	public Vector3 LocalOffset;
	public Vector2 FacingDir;
}