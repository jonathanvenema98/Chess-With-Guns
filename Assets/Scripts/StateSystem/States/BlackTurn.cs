using System.Collections;

public class BlackTurn : State
{
    public BlackTurn(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override IEnumerator OnStart()
    {
        GameController.SetCurrentTeam(Team.Black);
        yield break;
    }
}
