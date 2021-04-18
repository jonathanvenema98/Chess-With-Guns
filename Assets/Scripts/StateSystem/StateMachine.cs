public class StateMachine : Singleton<StateMachine>
{
    public State State { get; private set; }

    public void SetState(State state)
    {
        if (State != null)
            State.OnExit();
        
        State = state;
        State.OnStart();
    }

    private void Update()
    {
        if (State == null)
            return;
        
        State.OnUpdate();
    }
}
