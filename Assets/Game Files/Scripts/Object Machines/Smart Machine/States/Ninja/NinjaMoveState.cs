using UnityEngine;
using System;
[CreateAssetMenu(menuName = "SmartState/Ninja/MoveState")]
public class NinjaMoveState : SmartState {
    public int thoughtRate;
	public float moveSpeed;
	public int MPRestoreTime;

    public int rthreshold;
	public AnimationCurve strafeMultiplier;

	[SerializeField]
	private AnimationCurve forwardMovement;
	[SerializeField]
	private AnimationCurve strafeMovement;


	public override void OnEnter(SmartObject smartObject)
	{
		base.OnEnter(smartObject);
        smartObject.stateMachine.pointMem = EnemyManager.enemyManager.GetNinjaPoint();
        Debug.Log(smartObject.stateMachine.pointMem);
        
		smartObject.anim.SetBool("Moving", true);
		smartObject.anim.Play("Move", 0, 0);
	}

	public override void OnUpdate(SmartObject smartObject)
	{
		base.OnUpdate(smartObject);
		smartObject.SetFacingDir(false);
        Vector3 targetDir = (smartObject.stateMachine.pointMem.point.position-smartObject.tform.position).normalized;
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
        smartObject.inputDir = smartObject.stateMachine.pointMem.NinjaMove;
	}

	public override void HandleState(SmartObject smartObject)
	{
        if(Vector3.Distance(smartObject.stateMachine.pointMem.point.position,smartObject.tform.position) < 1){
            smartObject.stateMachine.savedTime = -1;
            smartObject.stateMachine.ChangeState(StateEnums.Move);
        }
    }
}