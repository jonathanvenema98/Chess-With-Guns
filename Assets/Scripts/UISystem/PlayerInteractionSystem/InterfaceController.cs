using UnityEngine;

public class InterfaceController : MonoBehaviour
{
	public delegate void TileClickedEvent(Vector2Int boardPosition);

	public static event TileClickedEvent OnTileLeftClickedEvent;
	public static event TileClickedEvent OnTileRightClickedEvent;
	
	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
		if (Input.GetMouseButtonDown(Utils.LeftMouseButton))
		{
			IfWithinBoardInvokeEvent(OnTileLeftClickedEvent);
		}

		if (Input.GetMouseButtonDown(Utils.RightMouseButton))
		{
			IfWithinBoardInvokeEvent(OnTileRightClickedEvent);
		}
	}

	private static void IfWithinBoardInvokeEvent(TileClickedEvent tileClickedEvent)
	{
		Vector3 clickPosition = Utils.MouseWorldPosition;
		Vector2Int boardPosition = BoardController.WorldPositionToBoardPosition(clickPosition);

		if (BoardController.IsWithinBoard(boardPosition))
		{
			tileClickedEvent?.Invoke(boardPosition);
		}
	}
}
