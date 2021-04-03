using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State State { get; private set; }

    public void SetState(State state)
    {
        State = state;
        StartCoroutine(State.OnStart());
    }
}
