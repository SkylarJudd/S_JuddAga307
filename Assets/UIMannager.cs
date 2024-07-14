using UnityEngine;
using TMPro;


public class UIMannager : Singleton<UIMannager>
{
    [SerializeField] TMP_Text ammoCountText;
    [SerializeField] TMP_Text weponTypeText;
    [SerializeField] TMP_Text difficultyText;
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] TMP_Text EnemiesText;
    [SerializeField] TMP_Text PlayerHealthText;

    [SerializeField] GameObject GameWindow;
    [SerializeField] GameObject PauseWindow;
    [SerializeField] GameObject GameWinWindow;
    [SerializeField] GameObject GameLoseWindow;

    [SerializeField] private TMP_Dropdown difficultyDropDown;

    public int Difficulty;

    bool PauseMenuOpen;
    float menuPressCoolDown;


    public void UpdateEnemies(int _enemies, int _maxEnemies)
    {
        EnemiesText.text = ($"{_enemies.ToString()}/{_maxEnemies.ToString()}");
    }
    public void UpdateTimer(float _timer)
    {
        if (_timer < 5)
        {
            ScoreText.color = Color.red;
        }
        else if (_timer < 10)
        {
            ScoreText.color = new Color(1f, 0.5f, 0f);
        }
        else if (_timer < 15)
        {
            
            ScoreText.color = Color.yellow;
        }
        else
        {
            ScoreText.color = Color.white;
        }

        int seconds = Mathf.FloorToInt(_timer); // Get whole seconds
        int milliseconds = Mathf.FloorToInt((_timer - seconds) * 100); // Get milliseconds
        string formattedTime = string.Format("{0:00}:{1:00}", seconds, milliseconds);
        //Debug.Log("Formatted Time: " + formattedTime); // Check the console output
        ScoreText.text = formattedTime;
    }
    public void UpdateDifficulty(Difficulty _difficulty)
    {
        if (_GM.gameState == GameState.Playing)
        {
            difficultyText.text = _difficulty.ToString();
        }

    }
    public void UpdatePlayerHealth(int _PlayerHealth)
    {
        PlayerHealthText.text = ($"{_PlayerHealth} Hearts");
    }
    public void UpdateWepon(Wepon _wepon)
    {
        weponTypeText.text = ($"Current Wepon: {_wepon.ToString()}");
    }
    public void updateAmmo(int _CurrentAmmo, int _MaxAmmo)
    {
        ammoCountText.text = ($"Ammo: {_CurrentAmmo}/{_MaxAmmo}");
    }

    

    

    public void ShowEndScreenLoss()
    {

    }
    public void ShowEndScreenWin()
    {

    }

}
