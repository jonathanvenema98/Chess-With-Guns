using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Soldier
{



    //constructor
	public Soldier()
	{

	}





    //movement method for the soldier
    public abstract List<vector2int> ValidMoves()
    {
        List<Vector2Int> PossibleMoves = new List<Vector2Int>()

        Vector2Int moveUp = new Vector2Int(0, 1);
        Vector2Int moveDown = new Vector2Int(0, -1);
        Vector2Int moveLeft = new Vector2Int(-1, 0);
        Vector2Int moveRight = new Vector2Int(1, 0);

        if (BoardController.IsWithinBoard(BoardController.BoardPosition + moveUp) && BoardController.IsBoardItemAt(BoardController.BoardPosition + moveUp) = 0)
            PossibleMoves.Add(new Vector2Int(BoardController.BoardPosition + moveUp));

        if (BoardController.IsWithinBoard(BoardController.BoardPosition + moveDown) && BoardController.IsBoardItemAt(BoardController.BoardPosition + moveDown) = 0)
            PossibleMoves.Add(new Vector2Int(BoardController.BoardPosition + moveDown));

        if (BoardController.IsWithinBoard(BoardController.BoardPosition + moveLeft) && BoardController.IsBoardItemAt(BoardController.BoardPosition + moveLeft) = 0)
            PossibleMoves.Add(new Vector2Int(BoardController.BoardPosition + moveLeft));

        if (BoardController.IsWithinBoard(BoardController.BoardPosition + moveRight) && BoardController.IsBoardItemAt(BoardController.BoardPosition + moveRight) = 0)
            PossibleMoves.Add(new Vector2Int(BoardController.BoardPosition + moveRight));

        return PossibleMoves;
    }
    

    //shooting method for the soldier
    public abstract List<vector2int> ValidShoot()
    {
        List<Vector2Int> PossibleShoot = new List<Vector2Int>()

        //how many tiles away the soldier can shoot
        int shootDistance = 3


        //For loop cycles through different PossibleShoot distances starting with 1
        //3 possibilities each time it cycles through:
        //first possibility: enemy on tile, therefore can shoot. End the for loop after returning its coordinates
        //second possibility: friendly or obstable on tile, therefore can't shoot. End the for loop without returning any coordinates
        //third possibility: nothing on tile, in which case the for loop continues to check the next square until shootDistance has been reached.
        For(int i = 0;int < 3;int++)
        {
            Vector2Int relativeUpVector = new Vector2Int(0, i+1)

            if (BoardController.IsWithinBoard(BoardController.BoardPosition + relativeUpVector) &&
                BoardController.IsEnemyAt(BoardController.BoardPosition + relativeUpVector))
            {
                PossibleShoot.Add(new Vector2Int(BoardController.BoardPosition + relativeUpVector)
                int i = 3;
            }

            else if (BoardController.IsWithinBoard(BoardController.BoardPosition + relativeUpVector) &&
                (BoardController.IsObstacleAt(BoardController.BoardPosition + relativeUpVector) || 
                BoardController.IsFriendlyAt(BoardController.BoardPosition + relativeUpVector)))
            {
                int i = 3;
            }

            else if (BoardController.IsWithinBoard(BoardController.BoardPosition + relativeUpVector) && 
                BoardController.IsBoardItemAt(BoardController.BoardPosition + relativeUpVector)
            {
                //continue looping through
            }







        }

    }




}
