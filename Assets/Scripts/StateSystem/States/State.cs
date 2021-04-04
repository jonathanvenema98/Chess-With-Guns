using System.Collections;

public abstract class State
{
	public virtual void OnStart()
	{
	}
	
	public virtual void OnUpdate()
	{
	}

	public virtual void OnExit()
	{
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
