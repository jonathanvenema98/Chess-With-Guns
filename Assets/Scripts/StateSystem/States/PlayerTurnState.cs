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
        InterfaceController.OnTileLeftClickedEvent += OnTileLeftClickedSubscriber;
    }

    public override void OnExit()
    {
        InterfaceController.OnTileLeftClickedEvent -= OnTileLeftClickedSubscriber;
        CameraController.Instance.UnfocusCamera();
    }

    private void OnTileLeftClickedSubscriber(Vector2Int boardPosition)
    {
        HideAllBorders();
        if (currentPiecePosition == boardPosition)
        {
            currentPiecePosition = Utils.Empty;
            CameraController.Instance.UnfocusCamera();
        }
        else if (currentPiecePosition != Utils.Empty && currentMoves.Contains(boardPosition))
        {
            UseAction();
            currentPiecePosition = Utils.Empty;
        
            BoardController.MoveBoardItemTo(currentPiece, boardPosition);
            CameraController.Instance.FocusCamera(boardPosition);
        }

        else if (currentPiecePosition != Utils.Empty && currentAttacks.Contains(boardPosition))
        {
            UseAction();
            currentPiecePosition = Utils.Empty;
        
            Piece piece = BoardController.GetBoardItemAt<Piece>(boardPosition);
            piece.TakeDamage(currentPiece.AttackDamage);
            FadingUIManager.Instance.CreateFadingText(
                boardPosition,
                $"-{currentPiece.AttackDamage}",
                GameController.AttackTileColour);
        }

        else if (BoardController.IsFriendlyAt(boardPosition, currentTeam))
        {
            currentPiece = BoardController.GetBoardItemAt<Piece>(boardPosition);
            currentPiecePosition = currentPiece.BoardPosition;
            currentMoves = currentPiece.GetMoves();
            currentAttacks = currentPiece.GetAttacks();
            
            DisplayAllBorders();
            CameraController.Instance.FocusCamera(currentPiecePosition);
        }
        else //If you clicked on a random tile
        {
            currentPiecePosition = Utils.Empty;
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
            CameraController.Instance.UnfocusCamera();
            StateMachine.Instance.SetState(new PlayerTurnState(GameController.GetNextTeam(currentTeam)));
        }

        // Every update, update values in UIUpdater to update Game HUD
        //UIUpdater.SetTeam(currentTeam);
        //UIUpdater.SetRemainingActions(remainingActions);
    }

    public int GetRemainingActions()
    {
        return remainingActions;
    }

    public Team GetTeam()
    {
        return currentTeam;
    }
}
