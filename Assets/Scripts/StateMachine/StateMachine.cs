using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T: StateMachine<T>
{
    protected State<T> State { get; private set; }

    public virtual void SetState(State<T> state)
    {
        State = state;
        StartCoroutine(State.OnStart());
    }
}
