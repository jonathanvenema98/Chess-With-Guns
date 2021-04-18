using UnityEngine;

public class GameOverState : State
{
    public override void OnStart()
    {
        Debug.Log("Game Over!");
    }
}
