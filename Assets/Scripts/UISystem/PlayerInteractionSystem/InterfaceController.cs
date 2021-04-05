using UnityEngine;

public class InterfaceController : MonoBehaviour
{
	public delegate void TileClickedEvent(Vector2Int boardPosition);

	public static event TileClickedEvent OnTileClickedEvent;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (OnTileClickedEvent != null && Input.GetMouseButtonDown(Utils.LeftMouseButton))
		{
			Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2Int boardPosition = BoardController.WorldPositionToBoardPosition(clickPosition);

			if (BoardController.IsWithinBoard(boardPosition))
			{
				OnTileClickedEvent.Invoke(boardPosition);
			}
		}
	}
}
