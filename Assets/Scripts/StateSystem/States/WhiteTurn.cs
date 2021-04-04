using System.Collections;

public class WhiteTurn : State
{
    public WhiteTurn(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override IEnumerator OnStart()
    {
        GameController.SetCurrentTeam(Team.White);
        yield break;
    }
}
