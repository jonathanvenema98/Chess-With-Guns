using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public abstract class Piece
{
    //fields
    public vector2int boardposition;
    public Team team;


    //constructor
    public Piece(int x, int y, Player player)
    {
        this.x = x;
        this.y = y;
        this.team = team;

    }



    //move method
    public abstract List<vector2int> ValidMoves()
    {
        List<Vector2Int> PossibleMoves = new List<Vector2Int>()

        Vector2Int moveup = new Vector2Int(0, 1);
        Vector2Int movedown = new Vector2Int(0, -1);
        Vector2Int moveleft = new Vector2Int(-1, 0);
        Vector2Int moveright = new Vector2Int(1, 0);

        if (BoardController.IsWithinBoard() && BoardController.IsBoardItemAt = 0)

    }

   











