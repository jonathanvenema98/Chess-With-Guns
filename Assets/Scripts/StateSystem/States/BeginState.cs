using UnityEngine;

public class BeginState : State
{
    public override void OnStart()
    {
        Debug.Log("The match has started");
        
        StateMachine.Instance.SetState(new PlayerTurnState(Team.White));
    }
}
