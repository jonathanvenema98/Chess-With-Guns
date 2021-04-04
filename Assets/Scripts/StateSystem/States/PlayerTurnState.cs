public class PlayerTurnState : State
{
    private readonly Team currentTeam;
    
    public PlayerTurnState(Team currentTeam)
    {
        this.currentTeam = currentTeam;
    }

    public override void OnStart()
    {
        GameController.SetCurrentTeam(currentTeam);
    }
}
