using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : GameBehaviour
{
    [SerializeField] GameObject GameWindow;
    [SerializeField] GameObject PauseWindow;
    [SerializeField] GameObject GameWinWindow;
    [SerializeField] GameObject GameLoseWindow;

    [SerializeField] private TMP_Dropdown difficultyDropDown;

    public int Difficulty;

    bool PauseMenuOpen;
    float menuPressCoolDown;

    private void Update()
    {
        if (menuPressCoolDown > 0)
        {
            menuPressCoolDown -= Time.deltaTime;
        }
        else
        {
            menuPressCoolDown = 0;
        }
    }

    public void ToggleMenu(InputAction.CallbackContext _context)
    {
        print($"Escape Pressed {_context.ReadValue<float>()}");
        if (_context.ReadValue<float>() == 1 && !PauseMenuOpen && menuPressCoolDown == 0)
        {
            print("MenuOpend");
            PauseMenuOpen = true;
            GameWindow.SetActive(false);
            PauseWindow.SetActive(true);
            menuPressCoolDown = 0.1f;
            Cursor.lockState = CursorLockMode.None;
            _GM.gameState = GameState.Paused;
            _CNM.StopMovement();

        }
        else if (_context.ReadValue<float>() == 1 && PauseMenuOpen && menuPressCoolDown == 0)
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        print("MenuClosed");
        Cursor.lockState = CursorLockMode.Locked;
        GameWindow.SetActive(true);
        PauseWindow.SetActive(false);
        menuPressCoolDown = 0.1f;
        PauseMenuOpen = false;
        _GM.gameState = GameState.Playing;
        _UIM.UpdateDifficulty(_GM.difficuty);
        _CNM.StartMovement();
    }

    public void GetDifficultyDropDownValue()
    {
        int pickedEntryIndex = difficultyDropDown.value;

        _GM.UpdateDifficuty(pickedEntryIndex);


    }
}
