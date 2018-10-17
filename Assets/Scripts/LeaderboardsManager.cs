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
        if (diff == Difficulties.easy.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeEasy, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == Difficulties.medium.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeMedium, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == Difficulties.hard.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeHard, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == Difficulties.expert.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeExpert, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == Difficulties.easyDiag.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeEasyPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == Difficulties.mediumDiag.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeMediumPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == Difficulties.hardDiag.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeHardPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
        else if (diff == Difficulties.expertDiag.name)
        {
            Social.ReportScore(time, LeaderboardIDs.bestTimeExpertPlus, (bool success) =>
            {
                SetTempBestTime(success, diff, time);
            });
        }
    }

    public void PushWinCount(int wins, string diff)
    {
        if (diff == Difficulties.easy.name)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsEasy, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == Difficulties.medium.name)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsMedium, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == Difficulties.hard.name)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsHard, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == Difficulties.expert.name)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsExpert, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == Difficulties.easyDiag.name)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsEasyPlus, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == Difficulties.mediumDiag.name)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsMediumPlus, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == Difficulties.hardDiag.name)
        {
            Social.ReportScore(wins, LeaderboardIDs.winsHardPlus, (bool success) =>
            {
                SetWinCount(success, diff, wins);
            });
        }
        else if (diff == Difficulties.expertDiag.name)
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
        CheckTime(Difficulties.easy.name);
        CheckTime(Difficulties.medium.name);
        CheckTime(Difficulties.hard.name);
        CheckTime(Difficulties.expert.name);
        CheckTime(Difficulties.easyDiag.name);
        CheckTime(Difficulties.mediumDiag.name);
        CheckTime(Difficulties.hardDiag.name);
        CheckTime(Difficulties.expertDiag.name);
        CheckWins(Difficulties.easy.name);
        CheckWins(Difficulties.medium.name);
        CheckWins(Difficulties.hard.name);
        CheckWins(Difficulties.expert.name);
        CheckWins(Difficulties.easyDiag.name);
        CheckWins(Difficulties.mediumDiag.name);
        CheckWins(Difficulties.hardDiag.name);
        CheckWins(Difficulties.expertDiag.name);
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
