using UnityEngine;
using UnityEngine.UI;

public class CongratsScreenManager : MonoBehaviour
{
    public GameObject shine, particles;
    public Image topPanel, bottomPanel, button;
    public Text congratsText, statsText, statsTextValues, buttonText;

    public void SetUpGameWonScreen(bool newBest)
    {
        string diff = PlayerPrefs.GetString(PlayerPrefsManager.difficulty);
        float time = PlayerPrefs.GetFloat(PlayerPrefsManager.time);
        float bestTime = PlayerPrefs.GetFloat(diff + PlayerPrefsManager.bestTime, Mathf.Infinity);
        string statsString = "";
        string statsValueString = "";
        if (newBest)
        {
            statsString = "Difficulty\nNew Best Time\nHint Count";
            statsValueString = diff + "\n" + GetComponent<Timer>().ConvertTime(bestTime) +
                          "\n" + PlayerPrefs.GetInt(PlayerPrefsManager.currentHintCount);
        }
        else
        {
            statsString = "Difficulty\nTime\nBest Time\nHint Count";
            statsValueString = diff + "\n" + GetComponent<Timer>().ConvertTime(time) +
                          "\n" + GetComponent<Timer>().ConvertTime(bestTime) +
                          "\n" + PlayerPrefs.GetInt(PlayerPrefsManager.currentHintCount);
        }
        statsText.text = statsString;
        statsTextValues.text = statsValueString;
        gameObject.AddComponent<GameWon>().SetUp(shine, particles, topPanel, bottomPanel,
                                                 congratsText, statsText, statsTextValues, button, buttonText);
    }
}
