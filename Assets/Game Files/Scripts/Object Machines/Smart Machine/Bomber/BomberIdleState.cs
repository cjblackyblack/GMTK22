using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SmartState/Bomber/IdleState")]
public class BomberIdleState : SmartState {

    public override void OnEnter(SmartObject smartObject){
        base.OnEnter(smartObject);
		smartObject.properties.objectTangibility = PhysicalObjectTangibility.Normal; ;
		smartObject.anim.Play("Idle", 0, 0);
    }
    public override void HandleState(SmartObject smartObject){
        smartObject.stateMachine.ChangeState(StateEnums.Move);
    }
}