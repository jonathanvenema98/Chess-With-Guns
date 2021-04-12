using System;

[Flags]
public enum ActionOption
{
    None = 0,

    IsEnemy      = 1 << 0,
    IsFriendly   = 1 << 1,
    IsObstacle   = 1 << 2,
    IsUnoccupied = 1 << 3,
    UntilBlocked = 1 << 4,
}
