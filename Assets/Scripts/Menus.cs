﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    public GameObject firstPlayPopUp, plusFirstPlayPopUp;
    public GameObject newGameMenu, moreMenu, winMenu;
    public GameObject settingsMenu, statsMenu, howToPlayMenu, loadingBackground, loadingText;
    public GameObject soundEffectsSlider, timeSlider, lineSlider, highlightSlider;
    public Slider thicknessSlider;
    public Text wins, bestTime, averageTime, currentWinStreak, longestWinStreak, hintCount, averageHintCount;
    public List<GameObject> difficultyButtons;
    public List<GameObject> confirmation;
    int currentDifficultyIndex;
    string currentDifficultyMenu;

    public void Awake()
    {
        soundEffectsSlider.GetComponent<Slider>().value = PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1);
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
        thicknessSlider.value = PlayerPrefs.GetInt(PlayerPrefsManager.lineThickness, 10);
        GetComponent<Appearance>().ChangeSettingLinesThickness((int)thicknessSlider.value);
        if (Application.platform == RuntimePlatform.Android)
        {
            ChangeMoreMenuForAndroid();
        }
    }

    void ChangeMoreMenuForAndroid()
    {
        for (int i = 0; i < moreMenu.transform.GetChild(0).childCount - 1; i++)
        {
            Debug.Log(moreMenu.transform.GetChild(0).GetChild(i).name);
            if (moreMenu.transform.GetChild(0).GetChild(i).name == "Restore Purchases")
            {
                moreMenu.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
            Vector3 newPosition = new Vector2(
                moreMenu.transform.GetChild(0).GetChild(i).GetComponent<RectTransform>().anchoredPosition.x,
                (moreMenu.transform.GetChild(0).GetChild(i).GetComponent<RectTransform>().anchoredPosition.y - 100));
            moreMenu.transform.GetChild(0).GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPosition;
        }
    }

    /*
     * New Game Menu
     */

    public void NewGameMenuOpen()
    {
        if (!newGameMenu.GetComponent<MenuTransitionOn>() && !newGameMenu.GetComponent<MenuTransitionOff>())
        {
            newGameMenu.AddComponent<MenuTransitionOn>();
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
            loadingBackground.SetActive(false);
        }
    }

    public void NewGameMenuClose()
    {
        if (!newGameMenu.GetComponent<MenuTransitionOn>() && !newGameMenu.GetComponent<MenuTransitionOff>())
        {
            newGameMenu.AddComponent<MenuTransitionOff>();
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
        }
    }

    public void Easy()
    {
        SetDifficulty(Difficulties.easy);
    }

    public void EasyDiagonal()
    {
        SetDifficulty(Difficulties.easyDiag);
    }

    public void Medium()
    {
        SetDifficulty(Difficulties.medium);
    }

    public void MediumDiagonal()
    {
        SetDifficulty(Difficulties.mediumDiag);
    }

    public void Hard()
    {
        SetDifficulty(Difficulties.hard);
    }

    public void HardDiagonal()
    {
        SetDifficulty(Difficulties.hardDiag);
    }

    public void Expert()
    {
        SetDifficulty(Difficulties.expert);
    }

    public void ExpertDiagonal()
    {
        SetDifficulty(Difficulties.expertDiag);
    }

    void SetDifficulty(Difficulties.Difficulty diff)
    {
        if (!newGameMenu.GetComponent<MenuTransitionOn>() && !newGameMenu.GetComponent<MenuTransitionOff>())
        {
            StartCoroutine(Load(diff));
        }
    }

    IEnumerator Load(Difficulties.Difficulty diff)
    {
        loadingBackground.SetActive(true);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.StartActivityIndicator();
        }
        else
        {
            loadingText.SetActive(true);
        }
        yield return new WaitForSeconds(0);
        PlayerPrefs.SetInt(PlayerPrefsManager.boardCompleted, 0);
        PlayerPrefs.SetInt(PlayerPrefsManager.currentHintCount, 0);
        PlayerPrefs.SetInt(PlayerPrefsManager.hintCount, 1);
        GetComponent<Appearance>().DestroyAllCircles();
        GetComponent<Appearance>().hint.gameObject.GetComponent<UnityAds>().SetHint(1);
        CheckIfRestartCurrentWinStreak();
        GetComponent<BoardCreator>().ClearBoard();
        GetComponent<BoardCreator>().NewBoard(diff.boardCount, diff.percentageEmpty, diff.name, diff.width, diff.height, diff.diagonals);
        GetComponent<NumberScroller>().ClearNumberScroller();
        GetComponent<NumberScroller>().SetUpNumberScroller();
        GetComponent<NumberScroller>().GoToFirstButton();
        GetComponent<Appearance>().RestartButtonSave();
        NewGameMenuClose();
        if (diff.diagonals)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsManager.plusFirstStartUp) == 0)
            {
                PlusFirstPlayPopUpOpen();
            }
        }
        winMenu.SetActive(false);
        Handheld.StopActivityIndicator();
        loadingText.SetActive(false);
    }

    public void Restart()
    {
        if (!newGameMenu.GetComponent<MenuTransitionOn>() && !newGameMenu.GetComponent<MenuTransitionOff>())
        {
            GetComponent<Appearance>().hint.gameObject.GetComponent<UnityAds>().SetHint(1);
            CheckIfRestartCurrentWinStreak();
            GetComponent<Appearance>().DestroyAllCircles();
            GetComponent<BoardCreator>().Restart();
            NewGameMenuClose();
            winMenu.SetActive(false);
        }
    }

    void CheckIfRestartCurrentWinStreak()
    {
        if (!GetComponent<BoardCreator>().GetWinStatus())
        {
            PlayerPrefs.SetInt(PlayerPrefs.GetString(PlayerPrefsManager.difficulty) + PlayerPrefsManager.currentWinStreak, 0);
        }
    }

    /*
     * More Menu
     */

    public void MoreMenuOpen()
    {
        if (!moreMenu.GetComponent<MenuTransitionOn>() && !moreMenu.GetComponent<MenuTransitionOff>())
        {
            moreMenu.AddComponent<MenuTransitionOn>();
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
    }

    public void MoreMenuClose()
    {
        if (!moreMenu.GetComponent<MenuTransitionOn>() && !moreMenu.GetComponent<MenuTransitionOff>())
        {
            moreMenu.AddComponent<MenuTransitionOff>();
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
        }
    }

    /*
     * Settings
     */

    public void SettingsOpen()
    {
        if (!settingsMenu.GetComponent<MenuTransitionOn>() && !settingsMenu.GetComponent<MenuTransitionOff>())
        {
            settingsMenu.AddComponent<MenuTransitionOn>();
            settingsMenu.SetActive(true);
        }
    }

    public void SettingsClose()
    {
        if (!settingsMenu.GetComponent<MenuTransitionOn>() && !settingsMenu.GetComponent<MenuTransitionOff>())
        {
            settingsMenu.AddComponent<MenuTransitionOff>();
            GetComponent<Appearance>().ChangeLineThickness();
            PlayerPrefs.Save();
        }
    }

    public void SoundEffects()
    {
        if (!soundEffectsSlider.GetComponent<SliderMovement>())
        {
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
        }
    }

    public void ShowTime()
    {
        if (!timeSlider.GetComponent<SliderMovement>())
        {
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
        }
    }

    public void ShowLines()
    {
        if (!lineSlider.GetComponent<SliderMovement>())
        {
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
        }
    }

    public void ShowHighlights()
    {
        if (!highlightSlider.GetComponent<SliderMovement>())
        {
            highlightSlider.AddComponent<SliderMovement>();
            if (PlayerPrefs.GetInt(PlayerPrefsManager.showNodeHighlights) == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showNodeHighlights, 1);
                highlightSlider.GetComponent<SliderMovement>().SetGoTowards(1);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefsManager.showNodeHighlights, 0);
                highlightSlider.GetComponent<SliderMovement>().SetGoTowards(0);
            }
        }
    }

    public void MovingThicknessSlider()
    {
        PlayerPrefs.SetInt(PlayerPrefsManager.lineThickness, (int)thicknessSlider.value);
        GetComponent<Appearance>().ChangeSettingLinesThickness((int)thicknessSlider.value);
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

    /*
     * Stats
     */

    public void StatsOpen()
    {
        if (!statsMenu.GetComponent<MenuTransitionOn>() && !statsMenu.GetComponent<MenuTransitionOff>())
        {
            statsMenu.AddComponent<MenuTransitionOn>();
            statsMenu.SetActive(true);
            StatsEasy();
        }
    }

    public void StatsClose()
    {
        if (!statsMenu.GetComponent<MenuTransitionOn>() && !statsMenu.GetComponent<MenuTransitionOff>())
        {
            statsMenu.AddComponent<MenuTransitionOff>();
            CloseConfirmation();
        }
    }

    public void StatsEasy()
    {
        SetStats(0, Difficulties.easy.name);
    }

    public void StatsMedium()
    {
        SetStats(1, Difficulties.medium.name);
    }

    public void StatsHard()
    {
        SetStats(2, Difficulties.hard.name);
    }

    public void StatsExpert()
    {
        SetStats(3, Difficulties.expert.name);
    }

    public void StatsEasyPlus()
    {
        SetStats(4, Difficulties.easyDiag.name);
    }

    public void StatsMediumPlus()
    {
        SetStats(5, Difficulties.mediumDiag.name);
    }

    public void StatsHardPlus()
    {
        SetStats(6, Difficulties.hardDiag.name);
    }

    public void StatsExpertPlus()
    {
        SetStats(7, Difficulties.expertDiag.name);
    }

    void SetStats(int index, string diffName)
    {
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        currentDifficultyIndex = index;
        difficultyButtons[currentDifficultyIndex].GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        FillStatsText(diffName);
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

    /*
     * How To Play
     */

    public void HowToPlayOpen()
    {
        if (!howToPlayMenu.GetComponent<MenuTransitionOn>() && !howToPlayMenu.GetComponent<MenuTransitionOff>())
        {
            howToPlayMenu.AddComponent<MenuTransitionOn>();
            howToPlayMenu.SetActive(true);
        }
    }

    public void HowToPlayClose()
    {
        if (!howToPlayMenu.GetComponent<MenuTransitionOn>() && !howToPlayMenu.GetComponent<MenuTransitionOff>())
        {
            howToPlayMenu.AddComponent<MenuTransitionOff>();
        }
    }

    public void FirstPlayPopUpOpen()
    {
        firstPlayPopUp.SetActive(true);
        firstPlayPopUp.AddComponent<MenuTransitionOn>();
        GetComponent<Timer>().PauseTimer();
    }

    public void FirstPlayPopUpClose()
    {
        if (!firstPlayPopUp.GetComponent<MenuTransitionOn>() && !firstPlayPopUp.GetComponent<MenuTransitionOff>())
        {
            GetComponent<Timer>().UnPauseTimer();
            firstPlayPopUp.AddComponent<MenuTransitionOff>();
            PlayerPrefs.SetInt(PlayerPrefsManager.firstStartUp, 1);
        }
    }

    public void PlusFirstPlayPopUpOpen()
    {
        plusFirstPlayPopUp.SetActive(true);
        plusFirstPlayPopUp.AddComponent<MenuTransitionOn>();
        GetComponent<Timer>().PauseTimer();
    }

    public void PlusFirstPlayPopUpClose()
    {
        if (!plusFirstPlayPopUp.GetComponent<MenuTransitionOn>() && !plusFirstPlayPopUp.GetComponent<MenuTransitionOff>())
        {
            GetComponent<Timer>().UnPauseTimer();
            plusFirstPlayPopUp.AddComponent<MenuTransitionOff>();
            PlayerPrefs.SetInt(PlayerPrefsManager.plusFirstStartUp, 1);
        }
    }

    /*
     * Winning Screen
     */

    public void WinMenuOpen()
    {
        winMenu.SetActive(true);
    }

    public void WinMenuClose()
    {
        NewGameMenuOpen();
    }
}
