using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager current;
	[HideInInspector]
	public PlayerController playerController;
	public Vector2 input;
	public int[] buttonBuffer;
	public int[] buttonReleaseBuffer;

	public SmartObject[] Party = new SmartObject[3];
	public SmartObject PartyLeader;

	public int currentFormation;
	public PlayerFormations[] PlayerFormations;

	private float formationLockoutTimer = 0;
	private float lockoutTime = 0.5f;

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

	public void BufferButton1(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonBuffer[0] = 6;
		}
	}

	public void BufferRelease1(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonReleaseBuffer[0] = 6;
		}
	}

	public void BufferButton2(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonBuffer[1] = 6;
		}
	}

	public void BufferRelease2(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonReleaseBuffer[1] = 6;
		}
	}

	public void BufferButton3(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonBuffer[2] = 6;
		}
	}

	public void BufferRelease3(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonReleaseBuffer[2] = 6;
		}
	}

	public void BufferButton4(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonBuffer[3] = 6;
		}
	}

	public void BufferRelease4(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonReleaseBuffer[3] = 6;
		}
	}

	public void BufferButton5(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonBuffer[4] = 6;
		}
	}

	public void BufferRelease5(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			buttonReleaseBuffer[4] = 6;
		}
	}
	public void OnFormationChange()
	{
		switch (PlayerFormations[currentFormation].ColliderShape)
		{
			case ColliderShape.Line:
				{
					break;
				}
			case ColliderShape.Triangle:
				{
					break;
				}

		}

		playerController.LineCollider.gameObject.SetActive(PlayerFormations[currentFormation].ColliderShape == ColliderShape.Line);
		playerController.TriangleCollider.gameObject.SetActive(PlayerFormations[currentFormation].ColliderShape == ColliderShape.Triangle);
	}

	public void SetFormation(int index)
	{
		currentFormation = index;

		if (currentFormation >= PlayerFormations.Length)
			currentFormation = 0;
		if (currentFormation < 0)
			currentFormation = PlayerFormations.Length - 1;

		OnFormationChange();
	}

	public void IncrementFormation(int delta)
	{
		currentFormation += delta;

		if (currentFormation >= PlayerFormations.Length)
			currentFormation = 0;
		if (currentFormation < 0)
			currentFormation = PlayerFormations.Length-1;

		OnFormationChange();
	}

	private void Update()
	{
		if (formationLockoutTimer > 0)
			formationLockoutTimer -= Time.deltaTime;

		if(buttonBuffer[4] > 0 && formationLockoutTimer <= 0f)
		{
			buttonBuffer[4] = 0;

			formationLockoutTimer = lockoutTime;
			IncrementFormation(1);
		}
	}
}


[System.Serializable]
public class PlayerFormations
{
	public PlayerPositions[] playerPositions;
	public ColliderShape ColliderShape;
	public float ColliderRotation;
}

[System.Serializable]
public class PlayerPositions 
{
	public Vector3 LocalOffset;
	public Vector2 FacingDir;

}

public enum ColliderShape {Line, Triangle }