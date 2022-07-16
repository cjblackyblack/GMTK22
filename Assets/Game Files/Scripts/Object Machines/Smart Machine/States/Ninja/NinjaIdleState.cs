using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SmartState/Ninja/IdleState")]
public class NinjaIdleState : SmartState {
    public SmartState approachMovement;
    public override void HandleState(SmartObject smartObject){
        
        smartObject.stateMachine.ChangeState(approachMovement);
    }
}