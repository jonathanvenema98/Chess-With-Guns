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
        InterfaceController.OnTileLeftClickedEvent += CameraController.Instance.OnTileLeftClickedSubscriber;
    }

    public override void OnExit()
    {
        InterfaceController.OnTileLeftClickedEvent -= CameraController.Instance.OnTileLeftClickedSubscriber;
        CameraController.Instance.UnfocusCamera();
    }
}
