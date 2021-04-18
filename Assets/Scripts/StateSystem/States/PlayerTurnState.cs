using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurnState : State
{
    private int remainingActions = 2;
    private readonly Team currentTeam;
    private Piece currentPiece;
    private Vector2Int currentPiecePosition = Utils.Empty;
    private IEnumerable<Vector2Int> currentAttacks = new List<Vector2Int>();
    private IEnumerable<Vector2Int> currentMoves = new List<Vector2Int>();
    
    public PlayerTurnState(Team currentTeam)
    {
        this.currentTeam = currentTeam;
    }

    public override void OnStart()
    {
        Debug.Log($"Currently {currentTeam}'s turn");
        
        GameController.SetCurrentTeam(currentTeam);
        InterfaceController.OnTileLeftClickedEvent += CameraController.Instance.OnTileLeftClickedSubscriber;
        InterfaceController.OnTileLeftClickedEvent += OnTileLeftClickedSubscriber;
    }

    public override void OnExit()
    {
        InterfaceController.OnTileLeftClickedEvent -= CameraController.Instance.OnTileLeftClickedSubscriber;
        InterfaceController.OnTileLeftClickedEvent -= OnTileLeftClickedSubscriber;
        CameraController.Instance.UnfocusCamera();
    }

    private void OnTileLeftClickedSubscriber(Vector2Int boardPosition)
    {
        if (currentPiecePosition == boardPosition)
        {
            HideAllBorders();
            currentPiecePosition = Utils.Empty;
        }
        else if (currentPiecePosition != Utils.Empty)
        {
            if (currentMoves.Contains(boardPosition))
            {
                HideAllBorders();
                UseAction();
                currentPiecePosition = Utils.Empty;
            
                BoardController.MoveBoardItemTo(currentPiece, boardPosition);
            }

            else if (currentAttacks.Contains(boardPosition))
            {
                HideAllBorders();
                UseAction();
                currentPiecePosition = Utils.Empty;
            
                Piece piece = BoardController.GetBoardItemAt<Piece>(boardPosition);
                piece.TakeDamage(currentPiece.AttackDamage);
                FadingUIManager.Instance.CreateFadingText(
                    boardPosition,
                    $"-{currentPiece.AttackDamage}",
                    GameController.AttackTileColour);
            }
        }

        else if (BoardController.IsFriendlyAt(boardPosition, currentTeam))
        {
            HideAllBorders();
            currentPiece = BoardController.GetBoardItemAt<Piece>(boardPosition);
            currentPiecePosition = currentPiece.BoardPosition;
            currentMoves = currentPiece.GetMoves();
            currentAttacks = currentPiece.GetAttacks();
            
            DisplayAllBorders();
        }
    }

    private void HideAllBorders()
    {
        currentAttacks.ForEach(BoardController.DestroyBorderAt);
        currentMoves.ForEach(BoardController.DestroyBorderAt);
    }

    private void DisplayAllBorders()
    {
        currentMoves
            .ForEach(p => BoardController.ShowBorderAt(p, GameController.MoveTileColour));
        currentAttacks
            .ForEach(p => BoardController.ShowBorderAt(p, GameController.AttackTileColour));
    }

    private void UseAction()
    {
        remainingActions--;
        Debug.Log($"Remaining actions: {remainingActions}");
        if (remainingActions == 0)
        {
            StateMachine.Instance.SetState(new PlayerTurnState(GameController.GetNextTeam(currentTeam)));
        }
    }
}
