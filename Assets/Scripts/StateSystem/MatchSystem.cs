public class MatchSystem : StateMachine
{
    private void Start()
    {
        SetState(new BeginState());
    }
}
