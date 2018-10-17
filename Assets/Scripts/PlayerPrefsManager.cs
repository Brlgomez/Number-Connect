using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    /* gameplay */
    public static string time = "time";
    public static string difficulty = "difficulty";
    public static string boardSize = "board size";
    public static string diagonal = "diagonal";
    public static string hintCount = "hintCount";
    public static string removeAds = "remove ads";

    /* slider */
    public static string highlightPosition = "highlight position";

    /* nodes */
    public static string locked = "locked";
    public static string value = "value";
    public static string userValue = "user value";
    public static string xPos = "xPos";
    public static string yPos = "yPos";
    public static string hinted = "hinted";

    /* settings */
    public static string currentTheme = "theme";
    public static string soundEffects = "sound";
    public static string showTime = "show time";
    public static string showLines = "show lines";
    public static string showNodeHighlights = "show node highlights";
    public static string lineThickness = "line thickness";

    /* stats */
    public static string winCount = "win count";
    public static string bestTime = "best time";
    public static string averageTime = "average time";
    public static string currentWinStreak = "current win streak";
    public static string longestWinStreak = "longest win streak";
    public static string currentHintCount = "current hint count";
    public static string totalHintCount = "total hint count";
    public static string averageHintCount = "average hint count";

    /* Other */
    public static string firstStartUp = "first start up";
    public static string plusFirstStartUp = "plus first start up";
    public static string boardCompleted = "board completed";
    public static string tempBestTime = "temp best time";
    public static string tempWinsCount = "temp wins count";

    public bool SaveStats()
    {
        PlayerPrefs.SetInt(boardCompleted, (PlayerPrefs.GetInt(boardCompleted, 0) + 1));
        bool newBest = false;
        if (PlayerPrefs.GetInt(boardCompleted, 0) == 1)
        {
            GetComponent<Appearance>().RestartButtonNoSave();

            string diff = PlayerPrefs.GetString(difficulty);
            int winAmount = PlayerPrefs.GetInt(diff + winCount, 0);

            /* time */
            float averageTimeAmount = winAmount * PlayerPrefs.GetFloat(diff + averageTime, 0);
            averageTimeAmount += GetComponent<Timer>().GetTime();
            averageTimeAmount /= (winAmount + 1);
            PlayerPrefs.SetFloat(diff + averageTime, averageTimeAmount);
            PlayerPrefs.SetInt(diff + winCount, winAmount + 1);
            float bestTimeAmount = PlayerPrefs.GetFloat(diff + bestTime, Mathf.Infinity);
            if (bestTimeAmount > GetComponent<Timer>().GetTime())
            {
                PlayerPrefs.SetFloat(diff + bestTime, GetComponent<Timer>().GetTime());
                newBest = true;
            }
            int milliseconds;
            if (Application.platform == RuntimePlatform.Android)
            {
                milliseconds = Mathf.FloorToInt(PlayerPrefs.GetFloat(diff + bestTime) * 1000);
            }
            else
            {
                milliseconds = Mathf.FloorToInt(PlayerPrefs.GetFloat(diff + bestTime) * 100);
            }
            GetComponent<LeaderboardsManager>().PushBestTime(milliseconds, diff);

            /* win streaks */
            int currentWinStreakAmount = PlayerPrefs.GetInt(diff + currentWinStreak, 0);
            currentWinStreakAmount++;
            PlayerPrefs.SetInt(diff + currentWinStreak, currentWinStreakAmount);
            int longestWinStreakAmount = PlayerPrefs.GetInt(diff + longestWinStreak, 0);
            if (currentWinStreakAmount > longestWinStreakAmount)
            {
                PlayerPrefs.SetInt(diff + longestWinStreak, currentWinStreakAmount);
            }
            GetComponent<LeaderboardsManager>().PushWinCount(winAmount + 1, diff);

            /* hint count */
            int currentHintAmount = PlayerPrefs.GetInt(currentHintCount, 0);
            int total = PlayerPrefs.GetInt(diff + totalHintCount, 0) + currentHintAmount;
            float averageHintAmount = winAmount * PlayerPrefs.GetFloat(diff + averageHintCount, 0);
            averageHintAmount += currentHintAmount;
            averageHintAmount /= winAmount + 1;
            PlayerPrefs.SetInt(diff + totalHintCount, total);
            PlayerPrefs.SetFloat(diff + averageHintCount, averageHintAmount);

            PlayerPrefs.Save();
        }
        return newBest;
    }
}
