using UnityEngine;
using UnityEngine.UI;

public class CongratsScreenManager : MonoBehaviour
{
    public GameObject shine, particles;
    public Image topPanel, bottomPanel, button;
    public Text congratsText, statsText, buttonText;

    public void SetUpGameWonScreen(bool newBest)
    {
        string diff = PlayerPrefs.GetString(PlayerPrefsManager.difficulty);
        float time = PlayerPrefs.GetFloat(PlayerPrefsManager.time);
        float bestTime = PlayerPrefs.GetFloat(diff + PlayerPrefsManager.bestTime, Mathf.Infinity);
        string text = "";
        if (newBest)
        {
            text = diff + "\nNew Best Time: " + GetComponent<Timer>().ConvertTime(bestTime) +
                          "\nHint Count: " + PlayerPrefs.GetInt(PlayerPrefsManager.currentHintCount);
        }
        else
        {
            text = diff + "\nTime: " + GetComponent<Timer>().ConvertTime(time) +
                          "\nBest Time: " + GetComponent<Timer>().ConvertTime(bestTime) +
                          "\nHint Count: " + PlayerPrefs.GetInt(PlayerPrefsManager.currentHintCount);
        }
        gameObject.AddComponent<GameWon>().SetUp(shine, particles, topPanel, bottomPanel,
                                                 congratsText, statsText, button, buttonText, text);
    }
}
