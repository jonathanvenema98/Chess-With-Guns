using System.Collections;

public abstract class State<T> where T: StateMachine<T>
{
	protected T StateMachine { get; private set; }

	public State(T stateMachine)
	{
		StateMachine = stateMachine;
	}

	public virtual IEnumerator OnStart()
	{
		yield break;
	}

	public virtual IEnumerator OnMove()
	{
		yield break;
	}

	public virtual IEnumerator OnAttack()
	{
		yield break;
	}
}
