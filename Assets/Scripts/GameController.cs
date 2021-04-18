using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
	[SerializeField] private List<Team> teams;

	[SerializeField] private Color focusedTileColour;
	[SerializeField] private Color moveTileColour;
	[SerializeField] private Color attackTileColour;
	[SerializeField] private GameMode gameMode;
	
	public static int Round { get; private set; }
	
	public static Team CurrentTeam { get; private set; }

	public static bool FirstRound => Round == 1;

	public static Color FocusedTileColour => Instance.focusedTileColour;
	public static Color MoveTileColour => Instance.moveTileColour;
	public static Color AttackTileColour => Instance.attackTileColour;

	public static GameMode GameMode => Instance.gameMode;

	public static void NextRound()
	{
		Round++;
	}

	public static void SetCurrentTeam(Team team)
	{
		CurrentTeam = team;
		if (CurrentTeam == Team.Blue)
			NextRound();
	}

	public static Team GetNextTeam(Team team)
	{
		int index = Instance.teams.IndexOf(team);
		index++;
		if (index == Instance.teams.Count)
			index = 0;

		return Instance.teams[index];
	}

	private new void Awake()
	{
		base.Awake();
		StateMachine.Instance.SetState(new BeginState());
	}

	// Update is called once per frame
	private void Update ()
	{

	}
}
