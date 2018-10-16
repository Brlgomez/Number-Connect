using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    public GameObject firstPlayPopUp;
    public GameObject plusFirstPlayPopUp;
    public GameObject newGameMenu;
    public GameObject moreMenu;
    public GameObject winMenu;
    public GameObject settingsMenu;
    public GameObject statsMenu;
    public GameObject howToPlayMenu;
    public GameObject soundEffectsSlider, timeSlider, lineSlider, highlightSlider;
    public Text wins, bestTime, averageTime, currentWinStreak, longestWinStreak, hintCount, averageHintCount;
    public List<GameObject> difficultyButtons;
    int currentDifficultyIndex;
    string currentDifficultyMenu;
    public List<GameObject> confirmation;
    public Difficulties tutorial, easy, medium, hard, expert, easyDiag, mediumDiag, hardDiag, expertDiag;

    public void Awake()
    {
        tutorial = new Difficulties
        {
            difficulty = "Tutorial",
            boardCount = 50,
            maxBoardSize = 14,
            percentageEmpty = 0.75f,
            diagonals = false
        };
        easy = new Difficulties
        {
            difficulty = "Easy",
            boardCount = 100,
            maxBoardSize = 14,
            percentageEmpty = 0.80f,
            diagonals = false
        };
        medium = new Difficulties
        {
            difficulty = "Medium",
            boardCount = 110,
            maxBoardSize = 13,
            percentageEmpty = 0.85f,
            diagonals = false
        };
        hard = new Difficulties
        {
            difficulty = "Hard",
            boardCount = 120,
            maxBoardSize = 13,
            percentageEmpty = 0.90f,
            diagonals = false
        };
        expert = new Difficulties
        {
            difficulty = "Expert",
            boardCount = 130,
            maxBoardSize = 13,
            percentageEmpty = 0.95f,
            diagonals = false
        };
        easyDiag = new Difficulties
        {
            difficulty = "Easy +",
            boardCount = 60,
            maxBoardSize = 14,
            percentageEmpty = 0.75f,
            diagonals = true
        };
        mediumDiag = new Difficulties
        {
            difficulty = "Medium +",
            boardCount = 80,
            maxBoardSize = 14,
            percentageEmpty = 0.80f,
            diagonals = true
        };
        hardDiag = new Difficulties
        {
            difficulty = "Hard +",
            boardCount = 100,
            maxBoardSize = 13,
            percentageEmpty = 0.85f,
            diagonals = true
        };
        expertDiag = new Difficulties
        {
            difficulty = "Expert +",
            boardCount = 120,
            maxBoardSize = 12,
            percentageEmpty = 0.90f,
            diagonals = true
        };
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 0)
        {
            soundEffectsSlider.GetComponent<Slider>().value = 0;
        }
        else
        {
            soundEffectsSlider.GetComponent<Slider>().value = 1;
        }
        if (PlayerPrefs.GetInt(PlayerPrefsManager.showTime, 1) == 0)
        {
            timeSlider.GetComponent<Slider>().value = 0;
            GetComponent<Timer>().timerText.gameObject.SetActive(false);
        }
        else
        {
            timeSlider.GetComponent<Slider>().value = 1;
            GetComponent<Timer>().timerText.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt(PlayerPrefsManager.showLines, 1) == 0)
        {
            lineSlider.GetComponent<Slider>().value = 0;
            GetComponent<BoardCreator>().lineHolder.SetActive(false);
            GetComponent<Appearance>().settingLines.SetActive(false);
        }
        else
        {
            lineSlider.GetComponent<Slider>().value = 1;
            GetComponent<BoardCreator>().lineHolder.SetActive(true);
            GetComponent<Appearance>().settingLines.SetActive(true);
        }
        if (PlayerPrefs.GetInt(PlayerPrefsManager.showNodeHighlights, 0) == 0)
        {
            highlightSlider.GetComponent<Slider>().value = 0;
            GetComponent<Appearance>().highlightHolder.SetActive(false);
        }
        else
        {
            highlightSlider.GetComponent<Slider>().value = 1;
            GetComponent<Appearance>().highlightHolder.SetActive(true);
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            ChangeMoreMenuForAndroid();
        }
    }

    void ChangeMoreMenuForAndroid()
    {
        for (int i = 0; i < moreMenu.transform.childCount - 1; i++)
        {
            if (moreMenu.transform.GetChild(i).name == "Restore Purchases")
            {
                moreMenu.transform.GetChild(i).gameObject.SetActive(false);
            }
            Vector3 newPosition = new Vector2(
                moreMenu.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x,
                (moreMenu.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y - 100));
            Debug.Log(newPosition);
            moreMenu.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPosition;
        }
    }

    public void NewGameMenuOpen()
    {
        GetComponent<Timer>().PauseTimer();
        if (!GetComponent<BoardCreator>().GetWinStatus())
        {
            GetComponent<BoardCreator>().boxHolders.SetActive(false);
            GetComponent<BoardCreator>().textHolder.SetActive(false);
            GetComponent<BoardCreator>().lineHolder.SetActive(false);
            GetComponent<Appearance>().highlightHolder.SetActive(false);
        }
        GetComponent<NumberScroller>().scrollView.StopMovement();
        newGameMenu.SetActive(true);
    }

    public void NewGameMenuClose()
    {
        GetComponent<Timer>().UnPauseTimer();
        GetComponent<BoardCreator>().boxHolders.SetActive(true);
        GetComponent<BoardCreator>().textHolder.SetActive(true);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.showLines, 1) == 1)
        {
            GetComponent<BoardCreator>().lineHolder.SetActive(true);
        }
        if (PlayerPrefs.GetInt(PlayerPrefsManager.showNodeHighlights, 0) == 1)
        {
            GetComponent<Appearance>().highlightHolder.SetActive(true);
        }
        newGameMenu.SetActive(false);
    }

    public void MoreMenuOpen()
    {
        GetComponent<Timer>().PauseTimer();
        if (!GetComponent<BoardCreator>().GetWinStatus())
        {
            GetComponent<BoardCreator>().boxHolders.SetActive(false);
            GetComponent<BoardCreator>().textHolder.SetActive(false);
            GetComponent<BoardCreator>().lineHolder.SetActive(false);
            GetComponent<Appearance>().highlightHolder.SetActive(false);
        }
        GetComponent<NumberScroller>().scrollView.StopMovement();
        moreMenu.SetActive(true);
    }

    public void MoreMenuClose()
    {
        GetComponent<Timer>().UnPauseTimer();
        GetComponent<BoardCreator>().boxHolders.SetActive(true);
        GetComponent<BoardCreator>().textHolder.SetActive(true);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.showLines, 1) == 1)
        {
            GetComponent<BoardCreator>().lineHolder.SetActive(true);
        }
        if (PlayerPrefs.GetInt(PlayerPrefsManager.showNodeHighlights, 0) == 1)
        {
            GetComponent<Appearance>().highlightHolder.SetActive(true);
        }
        moreMenu.SetActive(false);
    }

    public void WinMenuOpen()
    {
        winMenu.SetActive(true);
    }

    public void WinMenuClose()
    {
        winMenu.SetActive(false);
        NewGameMenuOpen();
    }

    public void SettingsOpen()
    {
        settingsMenu.SetActive(true);
    }

    public void SettingsClose()
    {
        GetComponent<Appearance>().SaveThickness();
        settingsMenu.SetActive(false);
    }

    public void SoundEffects()
    {
        if (!soundEffectsSlider.GetComponent<SliderMovement>())
        {
            GetComponent<HapticFeedback>().LightTapticFeedback();
            soundEffectsSlider.AddComponent<SliderMovement>();
            if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.soundEffects, 1);
                soundEffectsSlider.GetComponent<SliderMovement>().SetGoTowards(1);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.soundEffects, 0);
                soundEffectsSlider.GetComponent<SliderMovement>().SetGoTowards(0);
            }
            PlayerPrefs.Save();
        }
    }

    public void ShowTime()
    {
        if (!timeSlider.GetComponent<SliderMovement>())
        {
            GetComponent<HapticFeedback>().LightTapticFeedback();
            timeSlider.AddComponent<SliderMovement>();
            if (PlayerPrefs.GetInt(PlayerPrefsManager.showTime, 1) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showTime, 1);
                timeSlider.GetComponent<SliderMovement>().SetGoTowards(1);
                GetComponent<Timer>().timerText.gameObject.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showTime, 0);
                timeSlider.GetComponent<SliderMovement>().SetGoTowards(0);
                GetComponent<Timer>().timerText.gameObject.SetActive(false);
            }
            PlayerPrefs.Save();
        }
    }

    public void ShowLines()
    {
        if (!lineSlider.GetComponent<SliderMovement>())
        {
            GetComponent<HapticFeedback>().LightTapticFeedback();
            lineSlider.AddComponent<SliderMovement>();
            if (PlayerPrefs.GetInt(PlayerPrefsManager.showLines, 1) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showLines, 1);
                lineSlider.GetComponent<SliderMovement>().SetGoTowards(1);
                GetComponent<Appearance>().settingLines.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showLines, 0);
                lineSlider.GetComponent<SliderMovement>().SetGoTowards(0);
                GetComponent<Appearance>().settingLines.SetActive(false);
            }
            PlayerPrefs.Save();
        }
    }

    public void ShowHighlights()
    {
        if (!highlightSlider.GetComponent<SliderMovement>())
        {
            GetComponent<HapticFeedback>().LightTapticFeedback();
            highlightSlider.AddComponent<SliderMovement>();
            if (PlayerPrefs.GetInt(PlayerPrefsManager.showNodeHighlights) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showNodeHighlights, 1);
                highlightSlider.GetComponent<SliderMovement>().SetGoTowards(1);
                GetComponent<Appearance>().highlightHolder.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showNodeHighlights, 0);
                highlightSlider.GetComponent<SliderMovement>().SetGoTowards(0);
                GetComponent<Appearance>().highlightHolder.SetActive(false);
            }
            PlayerPrefs.Save();
        }
    }

    public void LightTheme()
    {
        GetComponent<Appearance>().ChangeThemes(Themes.ThemeName.light);
    }

    public void DarkTheme()
    {
        GetComponent<Appearance>().ChangeThemes(Themes.ThemeName.dark);
    }

    public void PaperTheme()
    {
        GetComponent<Appearance>().ChangeThemes(Themes.ThemeName.paper);
    }

    public void StatsOpen()
    {
        statsMenu.SetActive(true);
        StatsEasy();
    }

    public void StatsClose()
    {
        statsMenu.SetActive(false);
        CloseConfirmation();
    }

    public void StatsEasy()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 0;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(easy.difficulty);
    }

    public void StatsMedium()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 1;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(medium.difficulty);
    }

    public void StatsHard()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 2;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(hard.difficulty);
    }

    public void StatsExpert()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 3;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(expert.difficulty);
    }

    public void StatsEasyPlus()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 4;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(easyDiag.difficulty);
    }

    public void StatsMediumPlus()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 5;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(mediumDiag.difficulty);
    }

    public void StatsHardPlus()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 6;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(hardDiag.difficulty);
    }

    public void StatsExpertPlus()
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = 7;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(expertDiag.difficulty);
    }

    void FillStatsText(string diff)
    {
        if (currentDifficultyMenu != diff)
        {
            CloseConfirmation();
        }
        currentDifficultyMenu = diff;
        float best = PlayerPrefs.GetFloat(diff + PlayerPrefsManager.bestTime, Mathf.Infinity);
        float average = PlayerPrefs.GetFloat(diff + PlayerPrefsManager.averageTime, 0);
        wins.text = PlayerPrefs.GetInt(diff + PlayerPrefsManager.winCount, 0).ToString();
        if (!float.IsPositiveInfinity(best))
        {
            bestTime.text = GetComponent<Timer>().ConvertTime(best);
        }
        else
        {
            bestTime.text = "No time";
        }
        averageTime.text = GetComponent<Timer>().ConvertTime(average);
        currentWinStreak.text = PlayerPrefs.GetInt(diff + PlayerPrefsManager.currentWinStreak, 0).ToString();
        longestWinStreak.text = PlayerPrefs.GetInt(diff + PlayerPrefsManager.longestWinStreak, 0).ToString();
        hintCount.text = PlayerPrefs.GetInt(diff + PlayerPrefsManager.totalHintCount, 0).ToString();
        averageHintCount.text = PlayerPrefs.GetFloat(diff + PlayerPrefsManager.averageHintCount, 0).ToString("F2");
    }

    public void ShowConfirmation()
    {
        if (!confirmation[0].activeInHierarchy)
        {
            GetComponent<HapticFeedback>().WarningTapticFeedback();
            for (int i = 0; i < confirmation.Count; i++)
            {
                confirmation[i].SetActive(true);
            }
        }
        else
        {
            CloseConfirmation();
        }
    }

    public void CloseConfirmation()
    {
        for (int i = 0; i < confirmation.Count; i++)
        {
            confirmation[i].SetActive(false);
        }
    }

    public void EraseStats()
    {
        PlayerPrefs.SetInt(currentDifficultyMenu + PlayerPrefsManager.winCount, 0);
        PlayerPrefs.SetFloat(currentDifficultyMenu + PlayerPrefsManager.averageTime, 0);
        PlayerPrefs.SetFloat(currentDifficultyMenu + PlayerPrefsManager.bestTime, Mathf.Infinity);
        PlayerPrefs.SetInt(currentDifficultyMenu + PlayerPrefsManager.currentWinStreak, 0);
        PlayerPrefs.SetInt(currentDifficultyMenu + PlayerPrefsManager.longestWinStreak, 0);
        PlayerPrefs.SetInt(currentDifficultyMenu + PlayerPrefsManager.totalHintCount, 0);
        PlayerPrefs.SetFloat(currentDifficultyMenu + PlayerPrefsManager.averageHintCount, 0);
        PlayerPrefs.Save();
        FillStatsText(currentDifficultyMenu);
        CloseConfirmation();
    }

    public void HowToPlayOpen()
    {
        howToPlayMenu.SetActive(true);
    }

    public void HowToPlayClose()
    {
        howToPlayMenu.SetActive(false);
    }

    public void FirstPlayPopUpOpen()
    {
        GetComponent<Timer>().PauseTimer();
        gameObject.AddComponent<TutorialTransition>().TurnOn(firstPlayPopUp);
    }

    public void FirstPlayPopUpClose()
    {
        GetComponent<Timer>().UnPauseTimer();
        if (gameObject.GetComponent<TutorialTransition>())
        {
            gameObject.GetComponent<TutorialTransition>().TurnOff();
            PlayerPrefs.SetInt(PlayerPrefsManager.firstStartUp, 1);
        }
    }

    public void PlusFirstPlayPopUpOpen()
    {
        GetComponent<Timer>().PauseTimer();
        gameObject.AddComponent<TutorialTransition>().TurnOn(plusFirstPlayPopUp);
    }

    public void PlusFirstPlayPopUpClose()
    {
        GetComponent<Timer>().UnPauseTimer();
        if (gameObject.GetComponent<TutorialTransition>())
        {
            gameObject.GetComponent<TutorialTransition>().TurnOff();
            PlayerPrefs.SetInt(PlayerPrefsManager.plusFirstStartUp, 1);
        }
    }

    public void Easy()
    {
        SetDifficulty(easy);
    }

    public void EasyDiagonal()
    {
        SetDifficulty(easyDiag);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.plusFirstStartUp) == 0)
        {
            PlusFirstPlayPopUpOpen();
        }
    }

    public void Medium()
    {
        SetDifficulty(medium);
    }

    public void MediumDiagonal()
    {
        SetDifficulty(mediumDiag);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.plusFirstStartUp) == 0)
        {
            PlusFirstPlayPopUpOpen();
        }
    }

    public void Hard()
    {
        SetDifficulty(hard);
    }

    public void HardDiagonal()
    {
        SetDifficulty(hardDiag);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.plusFirstStartUp) == 0)
        {
            PlusFirstPlayPopUpOpen();
        }
    }

    public void Expert()
    {
        SetDifficulty(expert);
    }

    public void ExpertDiagonal()
    {
        SetDifficulty(expertDiag);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.plusFirstStartUp) == 0)
        {
            PlusFirstPlayPopUpOpen();
        }
    }

    void SetDifficulty(Difficulties diff)
    {
        PlayerPrefs.SetInt(PlayerPrefsManager.boardCompleted, 0);
        PlayerPrefs.SetInt(PlayerPrefsManager.currentHintCount, 0);
        PlayerPrefs.SetInt(PlayerPrefsManager.hintCount, 1);
        GetComponent<Appearance>().DestroyAllCircles();
        GetComponent<Appearance>().hint.gameObject.GetComponent<UnityAds>().SetHint(1);
        CheckIfRestartCurrentWinStreak();
        GetComponent<BoardCreator>().ClearBoard();
        GetComponent<BoardCreator>().NewBoard(diff.boardCount, diff.percentageEmpty, diff.difficulty, diff.maxBoardSize, diff.diagonals);
        GetComponent<NumberScroller>().ClearNumberScroller();
        GetComponent<NumberScroller>().SetUpNumberScroller();
        GetComponent<NumberScroller>().GoToFirstButton();
        GetComponent<Appearance>().RestartButtonSave();
        NewGameMenuClose();
    }

    public void Restart()
    {
        CheckIfRestartCurrentWinStreak();
        GetComponent<Appearance>().DestroyAllCircles();
        GetComponent<BoardCreator>().Restart();
        NewGameMenuClose();
    }

    void CheckIfRestartCurrentWinStreak()
    {
        if (!GetComponent<BoardCreator>().GetWinStatus())
        {
            PlayerPrefs.SetInt(PlayerPrefs.GetString(PlayerPrefsManager.difficulty) + PlayerPrefsManager.currentWinStreak, 0);
        }
    }

    public class Difficulties
    {
        public string difficulty;
        public int maxBoardSize;
        public int boardCount;
        public float percentageEmpty;
        public bool diagonals;
    }
}
