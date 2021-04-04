using System.Collections;
using UnityEngine;

public class Begin : State
{
    public Begin(StateMachine stateMachine) : base(stateMachine)
    {
    }
    
    public override IEnumerator OnStart()
    {
        Debug.Log("The match has started");
        
        StateMachine.SetState(new WhiteTurn(StateMachine));
        yield break;
    }
}
