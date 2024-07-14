using TMPro;
using UnityEngine;

public class MainMenu : Singleton<MainMenu>
{
    [SerializeField] private TMP_Dropdown difficultyDropDown;
    public int Difficulty;

    public void GetDifficultyDropDownValue()
    {
        int pickedEntryIndex = difficultyDropDown.value;


        Difficulty = pickedEntryIndex;
        print(Difficulty);

    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


}
