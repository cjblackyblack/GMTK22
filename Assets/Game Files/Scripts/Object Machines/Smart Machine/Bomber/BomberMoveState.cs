using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SmartState/Bomber/MoveState")]
public class BomberMoveState : SmartState {
    public int thoughtRate;
	public float moveSpeed;
	public int MPRestoreTime;
	public AnimationCurve strafeMultiplier;

    public float kazeDist;

	[SerializeField]
	private AnimationCurve forwardMovement;
	[SerializeField]
	private AnimationCurve strafeMovement;


	public override void OnEnter(SmartObject smartObject)
	{
		base.OnEnter(smartObject);
        smartObject.stateMachine.playerTargetMem = UnityEngine.Random.Range(0, PlayerManager.current.Party.Length);
		smartObject.anim.SetBool("Moving", true);
		smartObject.anim.Play("Move", 0, 0);
	}

	public override void OnUpdate(SmartObject smartObject)
	{
		base.OnUpdate(smartObject);
		smartObject.SetFacingDir(false);
        Vector3 targetDir = (PlayerManager.current.Party[smartObject.stateMachine.playerTargetMem].tform.position-smartObject.tform.position).normalized;
        smartObject._inputDir = new Vector2(targetDir.x,targetDir.z);
	}
	public override void OnFixedUpdate(SmartObject smartObject)
	{
		smartObject.velocity.x = (((smartObject._inputDir.normalized * forwardMovement.Evaluate(smartObject.currentTime)) + (smartObject._inputDir.normalized.Rotate(-90) * strafeMovement.Evaluate(smartObject.currentTime))).ConvertVector2()).x * moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;
		smartObject.velocity.z = (((smartObject._inputDir.normalized * forwardMovement.Evaluate(smartObject.currentTime)) + (smartObject._inputDir.normalized.Rotate(-90) * strafeMovement.Evaluate(smartObject.currentTime))).ConvertVector2()).z * moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;

		HandleState(smartObject);
	}

	public override void OnExit(SmartObject smartObject)
	{
		smartObject.anim.SetBool("Moving", false);
		base.OnExit(smartObject);
		smartObject.inputDir = Vector3.zero;
		smartObject._inputDir = Vector3.zero;

	}

	public override void HandleState(SmartObject smartObject)
	{
        if(Vector3.Distance(PlayerManager.current.Party[smartObject.stateMachine.playerTargetMem].tform.position,smartObject.tform.position) < kazeDist){
            smartObject.stateMachine.ChangeState(StateEnums.Action);
        }
	}
}