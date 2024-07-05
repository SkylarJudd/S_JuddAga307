using Unity.VisualScripting;
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

public class GameMannager : Singleton<GameMannager>
{
    public GameState gameState;
    public Difficuty difficuty;

    private int score;
    private int playerScore;
    int scoreMultiplier = 1;


    private void Start()
    {
        switch (difficuty)
        {
            case Difficuty.Eazy:
                scoreMultiplier = 1;
                break;
            case Difficuty.Medium:
                scoreMultiplier = 2;
                break;
            case Difficuty.Hard:
                scoreMultiplier = 3;
                break;
        }
    }

    public void AddScore(int _score)
    {
        playerScore = playerScore + _score;
        print(playerScore);
        print($"Score is: {playerScore}");
    }

    private void GameEvents_OnEnermyHit(GameObject _go)
    {
        AddScore(10);
    }

    private void GameEvents_OnEnermyDie(GameObject obj)
    {
        AddScore(100);
    }

    private void OnEnable()
    {
        GameEvents.OnEnermyHit += GameEvents_OnEnermyHit;
        GameEvents.OnEnermyDie += GameEvents_OnEnermyDie;
    }

   

    private void OnDisable()
    {
        GameEvents.OnEnermyHit -= GameEvents_OnEnermyHit;
        GameEvents.OnEnermyDie -= GameEvents_OnEnermyDie;
    }
}
