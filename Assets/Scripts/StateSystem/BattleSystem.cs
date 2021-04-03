public class BattleSystem : StateMachine
{
    private void Start()
    {
        SetState(new Begin(this));
    }
}
