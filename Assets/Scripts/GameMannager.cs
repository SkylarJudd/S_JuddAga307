
using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Paused,
    GameOver,
}

public enum Difficulty
{
    Eazy,
    Medium,
    Hard,
}

public class GameMannager : Singleton<GameMannager>
{
    public GameState gameState = GameState.Menu;
    public Difficulty difficuty;
    public float timer;
    public int playerScore;

    private int score;
    float scoreMultiplier = 1;


    private void Start()
    {
        timer = 30.0f;
        gameState = GameState.Playing;
        UpdateDifficuty(_MM.Difficulty);
        _CNM.StartMovement();
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void AddTime(float _time)
    {
        if(gameState == GameState.Playing)
            timer = timer + _time;

    }

    private void AddScore(int _score)
    {
        playerScore += _score;
    }

    private void UpdateTimer()
    {
        if (gameState == GameState.Playing)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
                EndGame();
            }

            _UIM.UpdateTimer(timer);
        }
           
    }

    private void GameEvents_OnChickenEnermyHit(GameObject _go)
    {
        AddTime(10.0f * scoreMultiplier);
    }

    private void GameEvents_OnChickenEnermyDie(GameObject obj)
    {
        AddTime(100.0f * scoreMultiplier);
    }

    public void UpdateDifficuty(int difficultyIndex)
    {
        print($"DifficultyIndex = {difficultyIndex}");
        print( _MM.Difficulty.ToString() );
        switch (difficultyIndex)
        {
            case 0:
                difficuty = Difficulty.Eazy; break;
            case 1:
                difficuty = Difficulty.Medium; break;
            case 2:
                difficuty = Difficulty.Hard; break;

        }

        switch (difficuty)
        {
            case Difficulty.Eazy:
                scoreMultiplier = 1;
                break;
            case Difficulty.Medium:
                scoreMultiplier = 0.5f;
                break;
            case Difficulty.Hard:
                scoreMultiplier = 0.1f;
                break;
        }      
        _UIM.UpdateDifficulty(difficuty);
    }

    private void EndGame()
    {
        gameState = GameState.GameOver;
        _CNM.StopMovement();
        _UIM.ShowEndScreenLoss();
        Cursor.lockState = CursorLockMode.None;
    }


    private void OnEnable()
    {
        GameEvents.OnChickenEnemyHit += GameEvents_OnChickenEnermyHit;
        GameEvents.OnChickenEnemyDie += GameEvents_OnChickenEnermyDie;
    }

   

    private void OnDisable()
    {
        GameEvents.OnChickenEnemyHit -= GameEvents_OnChickenEnermyHit;
        GameEvents.OnChickenEnemyDie -= GameEvents_OnChickenEnermyDie;
    }
}
