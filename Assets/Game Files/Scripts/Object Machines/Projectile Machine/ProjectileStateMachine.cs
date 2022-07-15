using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-11)]
public class ProjectileStateMachine : MonoBehaviour
{
    public ProjectileObject projectileObject => GetComponent<ProjectileObject>();

    public ProjectileState currentState;

    bool busyChange;

    public void OnUpdate()
    {
        currentState?.OnUpdate(projectileObject);
    }

    public void OnFixedUpdate()
    {
        currentState?.OnFixedUpdate(projectileObject);
    }

    public void ChangeState(ProjectileState newState)
    {
        if (!busyChange)
            ChangeStateWait(newState);
    }

    void ChangeStateWait(ProjectileState newState)
    {
        busyChange = true;
        currentState.OnExit(projectileObject);
        currentState = newState;
        currentState.OnEnter(projectileObject);
        busyChange = false;
    }
}
