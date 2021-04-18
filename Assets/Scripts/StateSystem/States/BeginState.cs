using UnityEngine;

public class BeginState : State
{
    public override void OnStart()
    {
        Debug.Log("The match has started");
        
        BoardController.Instance.Initialise();
        BoardController.Instance.LoadLevel("Symmetrical Level");
        
        StateMachine.Instance.SetState(new PlayerTurnState(Team.Blue));
    }
}
