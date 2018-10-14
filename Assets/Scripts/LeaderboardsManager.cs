using UnityEngine;
#if (NO_GPGS)
using UnityEngine.SocialPlatforms.GameCenter;
#endif
#if (!NO_GPGS)
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class LeaderboardsManager : MonoBehaviour
{
    void Start()
    {
#if (!NO_GPGS)
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
        LogIn();
    }

    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) => { });
    }

    public void GetLeaderboards()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                PushTempScores();
                Social.ShowLeaderboardUI();
            }
        });
    }

    public void PushBestTime(int time, string diff)
    {
        if (diff == GetComponent<Menus>().easy.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeEasy, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == GetComponent<Menus>().medium.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeMedium, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == GetComponent<Menus>().hard.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeHard, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == GetComponent<Menus>().expert.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeExpert, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == GetComponent<Menus>().easyDiag.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeEasyPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == GetComponent<Menus>().mediumDiag.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeMediumPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == GetComponent<Menus>().hardDiag.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeHardPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == GetComponent<Menus>().expertDiag.difficulty)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeExpertPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
    }

    public void PushWinCount(int wins, string diff)
    {
        if (diff == GetComponent<Menus>().easy.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsEasy, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == GetComponent<Menus>().medium.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsMedium, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == GetComponent<Menus>().hard.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsHard, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == GetComponent<Menus>().expert.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsExpert, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == GetComponent<Menus>().easyDiag.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsEasyPlus, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == GetComponent<Menus>().mediumDiag.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsMediumPlus, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == GetComponent<Menus>().hardDiag.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsHardPlus, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == GetComponent<Menus>().expertDiag.difficulty)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsExpertPlus, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
    }

    void SetTempBestTime(bool success, string difficulty, int time)
    {
        if (success)
        {
            PlayerPrefs.SetInt(difficulty + PlayerPrefsManager.tempBestTime, -1);
        }
        else
        {
            PlayerPrefs.SetInt(difficulty + PlayerPrefsManager.tempBestTime, time);
        }
    }

    void SetWinCount(bool success, string difficulty, int wins)
    {
        if (success)
        {
            PlayerPrefs.SetInt(difficulty + PlayerPrefsManager.tempWinsCount, -1);
        }
        else
        {
            PlayerPrefs.SetInt(difficulty + PlayerPrefsManager.tempWinsCount, wins);
        }
    }

    void PushTempScores()
    {
        CheckTime(GetComponent<Menus>().easy.difficulty);
        CheckTime(GetComponent<Menus>().medium.difficulty);
        CheckTime(GetComponent<Menus>().hard.difficulty);
        CheckTime(GetComponent<Menus>().expert.difficulty);
        CheckTime(GetComponent<Menus>().easyDiag.difficulty);
        CheckTime(GetComponent<Menus>().mediumDiag.difficulty);
        CheckTime(GetComponent<Menus>().hardDiag.difficulty);
        CheckTime(GetComponent<Menus>().expertDiag.difficulty);
        CheckWins(GetComponent<Menus>().easy.difficulty);
        CheckWins(GetComponent<Menus>().medium.difficulty);
        CheckWins(GetComponent<Menus>().hard.difficulty);
        CheckWins(GetComponent<Menus>().expert.difficulty);
        CheckWins(GetComponent<Menus>().easyDiag.difficulty);
        CheckWins(GetComponent<Menus>().mediumDiag.difficulty);
        CheckWins(GetComponent<Menus>().hardDiag.difficulty);
        CheckWins(GetComponent<Menus>().expertDiag.difficulty);
    }

    void CheckTime(string diff)
    {
        if (PlayerPrefs.GetInt(diff + PlayerPrefsManager.tempBestTime, -1) != -1)
        {
            PushBestTime(PlayerPrefs.GetInt(diff + PlayerPrefsManager.tempBestTime), diff);
        }
    }

    void CheckWins(string diff)
    {
        if (PlayerPrefs.GetInt(diff + PlayerPrefsManager.tempWinsCount, -1) != -1)
        {
            PushBestTime(PlayerPrefs.GetInt(diff + PlayerPrefsManager.tempWinsCount), diff);
        }
    }
}
