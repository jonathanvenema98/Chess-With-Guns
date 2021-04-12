using System;

[Flags]
public enum ActionOption
{
    None = 0,                  //000000

    IsEnemy          = 1 << 0, //000001
    IsFriendly       = 1 << 1, //000010
    IsObstacle       = 1 << 2, //000100
    IsUnoccupied     = 1 << 3, //001000
    UntilBlocked     = 1 << 4, //010000
    ValidTerrainType = 1 << 5, //100000
}
