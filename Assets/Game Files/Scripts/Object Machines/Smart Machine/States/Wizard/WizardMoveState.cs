using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SmartState/Wizard/MoveState")]
public class WizardMoveState : SmartState
{
	public int thoughtRate;
	public float moveSpeed;
	public int MPRestoreTime;

    public int teleportFrame;
    public int cooldownFrames;
	public AnimationCurve strafeMultiplier;

	[SerializeField]
	private AnimationCurve forwardMovement;
	[SerializeField]
	private AnimationCurve strafeMovement;


	public override void OnEnter(SmartObject smartObject)
	{
		base.OnEnter(smartObject);

		smartObject.anim.SetBool("Moving", true);
		smartObject.anim.Play("Move", 0, 0);
	}

	public override void OnUpdate(SmartObject smartObject)
	{
		base.OnUpdate(smartObject);
		smartObject.SetFacingDir(false);
	}
	public override void OnFixedUpdate(SmartObject smartObject)
	{
		//smartObject.velocity.x = (((smartObject._inputDir.normalized * forwardMovement.Evaluate(smartObject.currentTime)) + (smartObject._inputDir.normalized.Rotate(-90) * strafeMovement.Evaluate(smartObject.currentTime))).ConvertVector2()).x * moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;
		//smartObject.velocity.z = (((smartObject._inputDir.normalized * forwardMovement.Evaluate(smartObject.currentTime)) + (smartObject._inputDir.normalized.Rotate(-90) * strafeMovement.Evaluate(smartObject.currentTime))).ConvertVector2()).z * moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;

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
        if(smartObject.currentTime == teleportFrame){
            Vector3 rpos = EnemyManager.enemyManager.GetWizardPoint();
            smartObject.tform.position = new Vector3(rpos.x,smartObject.tform.position.y,rpos.z);
        }
        if(smartObject.currentTime == teleportFrame + cooldownFrames){
            smartObject.stateMachine.ChangeState(StateEnums.Idle);
        }
    }
}