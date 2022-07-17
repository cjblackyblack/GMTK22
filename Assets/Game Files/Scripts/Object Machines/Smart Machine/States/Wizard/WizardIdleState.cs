using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SmartState/Wizard/IdleState")]
public class WizardIdleState : SmartState {

    public int attackDelayFrames;
    public override void OnEnter(SmartObject smartObject)
    {
        base.OnEnter(smartObject);
        smartObject._inputDir = Vector2.zero;
        smartObject.inputDir = Vector2.zero;
        smartObject.anim.SetBool("Moving", false);
        smartObject.anim.Play("Idle", 0, 0);
    }

    public override void HandleState(SmartObject smartObject)
    {
        if(smartObject.currentTime > attackDelayFrames){
            
            smartObject.stateMachine.ChangeState(StateEnums.Action);
        }
    }

    
}