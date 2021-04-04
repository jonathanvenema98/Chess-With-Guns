using System;

[Flags]
public enum MoveOptions
{
    None = 0,
    
    FirstTurn = 1 << 0,
    Enemy     = 1 << 1,
    Friendly  = 1 << 2,
    Obstacle  = 1 << 3,
    
    Piece     = Enemy | Friendly,
    BoardItem = Piece | Obstacle
}
