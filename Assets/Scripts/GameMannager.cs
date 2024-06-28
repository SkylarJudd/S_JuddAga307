using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    Paused,
    GameOver,
}

public enum Difficuty
{
    Eazy,
    Medium,
    Hard,
}

public class GameMannager : MonoBehaviour
{
    public GameState gameState;
    public Difficuty difficuty;

    public int score;
    float scoreMultiplier = 1;

    private void Start()
    {
       switch (difficuty)
        {
            case Difficuty.Eazy:
                scoreMultiplier = 1;
                break;
            case Difficuty.Medium:
                scoreMultiplier = 1.5f;
                break;
            case Difficuty.Hard:
                scoreMultiplier = 2;
                break;
        }
    }
}
