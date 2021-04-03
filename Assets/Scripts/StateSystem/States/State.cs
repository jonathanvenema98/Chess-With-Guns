using System.Collections;

public abstract class State
{
	protected StateMachine StateMachine { get; private set; }

	public State(StateMachine stateMachine)
	{
		StateMachine = stateMachine;
	}

	// ReSharper disable Unity.PerformanceAnalysis
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
