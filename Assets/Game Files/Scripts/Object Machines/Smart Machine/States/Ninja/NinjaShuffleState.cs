using UnityEngine;
using System;
[CreateAssetMenu(menuName = "SmartState/Ninja/ShuffleState")]
public class NinjaShuffleState : SmartState {
    public int thoughtRate;
	public float moveSpeed;
	public int MPRestoreTime;

    public int shuffleTimeRange, shuffleTimeMin;
    public int shuffleLoopTime;
    

    

	[SerializeField]
	private AnimationCurve forwardMovement;
	[SerializeField]
	private AnimationCurve strafeMovement;


	public override void OnEnter(SmartObject smartObject)
	{
        smartObject.currentTime = smartObject.stateMachine.savedTime;
		smartObject.anim.SetBool("Moving", true);
		smartObject.anim.Play("Move", 0, 0);
        smartObject.stateMachine.randomIntMem = smartObject.currentTime + shuffleTimeMin + UnityEngine.Random.Range(0,shuffleTimeRange);
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
        int flip = Mathf.FloorToInt(smartObject.currentTime/shuffleLoopTime)%2==0 ? -1 : 1;
        
        smartObject.velocity.x = flip*smartObject.stateMachine.pointMem.NinjaMove.x* moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;
        smartObject.velocity.z = flip*smartObject.stateMachine.pointMem.NinjaMove.y*moveSpeed * smartObject.stats.moveSpeed * smartObject.statMods.moveSpeedMod;
		HandleState(smartObject);
	}

	public override void OnExit(SmartObject smartObject)
	{
		smartObject.anim.SetBool("Moving", false);
		base.OnExit(smartObject);
		
	}

	public override void HandleState(SmartObject smartObject)
	{
        if(smartObject.currentTime > smartObject.stateMachine.randomIntMem){
            smartObject.stateMachine.savedTime = smartObject.currentTime;
            smartObject.stateMachine.ChangeState(StateEnums.Action);
        }
    }
}