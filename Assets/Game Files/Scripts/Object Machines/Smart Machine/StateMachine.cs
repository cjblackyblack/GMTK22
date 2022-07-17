using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-11)]
public class StateMachine : MonoBehaviour
{
    public SmartObject smartObject => GetComponent<SmartObject>();
    public static int RollDice {
        get{
            return UnityEngine.Random.Range(0,6);
        }
    }
    public Dictionary<StateEnums, SmartState> stateDictionary;
    public StateEnums currentStateEnum;
    public SmartState currentState;
    public SmartState previousState;

    public int playerTargetMem;
    public int randomIntMem;

    public int savedTime;

    public Vector2 dirMem;

    public PointConfig pointMem;

    bool busyChange;

    public void StartMachine(List<SmartState> states)
	{
        stateDictionary = new Dictionary<StateEnums, SmartState>();
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i] == null)
                continue;
            stateDictionary.Add((StateEnums)i, states[i]);
        }

        previousState = stateDictionary[StateEnums.Idle];
        currentState = stateDictionary[StateEnums.Idle];
        ChangeState(StateEnums.Idle);
    }

    public void OnUpdate()
	{
        currentState?.OnUpdate(smartObject);
    }
    
    public void OnFixedUpdate()
	{
        currentState?.OnFixedUpdate(smartObject);
    }

    public void ChangeState(StateEnums newState)
	{
        //probs move this to hurt? or maybe another another check in there, lets see if just this works, but I'm thinking not enough 
        if ((newState == StateEnums.Idle || newState == StateEnums.Move) && smartObject.effectMachine.OverrideState() != null) 
            ChangeState(smartObject.effectMachine.OverrideState());
       
        else if (!busyChange)
           ChangeStateWait(newState);
    }

    public void ChangeState(SmartState newState)
    {
        if (!busyChange)
           ChangeStateWait(newState);
    }


    void ChangeStateWait(StateEnums newState)
    {
        busyChange = true;

        currentState.OnExit(smartObject);
        currentState = stateDictionary[newState];
        currentState.OnEnter(smartObject);
        smartObject.OnStateChange?.Invoke(smartObject, newState);
        currentStateEnum = newState;
        //yield return new WaitForFixedUpdate();
        busyChange = false;
    }

    void ChangeStateWait(SmartState newState)
    {
        busyChange = true;

        currentState.OnExit(smartObject);
        smartObject.OnStateChangeOverride?.Invoke(smartObject, newState);
        currentState = newState;
        currentState.OnEnter(smartObject);
        currentStateEnum = StateEnums.Action;

       
        busyChange = false;
    }
}