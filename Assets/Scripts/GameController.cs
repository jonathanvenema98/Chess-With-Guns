using UnityEngine;

public class GameController : Singleton<GameController>
{
	[SerializeField] private Obstacle boardItem;
	[SerializeField] private Vector2Int target;

	[InspectorButton]
	private void MoveToTarget()
	{
		//For testing purposes
		BoardController.MoveBoardItemTo(boardItem, target);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
