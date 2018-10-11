using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    float time;
    int tempTime;
    bool gameFinished;
    bool pause;

    void Update()
    {
        if (!gameFinished && !pause)
        {
            time += Time.deltaTime;
            if (tempTime != Mathf.FloorToInt(time))
            {
                timerText.text = ConvertTimeNoMilliseconds(time);
                SaveTime();
            }
            tempTime = Mathf.FloorToInt(time);
        }
    }

    public string ConvertTimeNoMilliseconds(float time)
    {
        string text = "";
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt(time / 60) % 60;
        int seconds = Mathf.FloorToInt(time % 60);
        if (hours == 0)
        {
            text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            text = hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        return text;
    }

    public string ConvertTime(float time)
    {
        string text = "";
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt(time / 60) % 60;
        int seconds = Mathf.FloorToInt(time % 60);
        int miliseconds = Mathf.FloorToInt(Mathf.Repeat(time, 1.0f) * 100);
        if (hours == 0)
        {
            text = minutes.ToString("00") + ":" + seconds.ToString("00") + "." + miliseconds.ToString("00");
        }
        else
        {
            text = hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + "." + miliseconds.ToString("00");
        }
        return text;
    }

    public float GetTime()
    {
        return time;
    }

    public void GameFinished()
    {
        gameFinished = true;
        SaveTime();
    }

    public void PauseTimer()
    {
        pause = true;
    }

    public void UnPauseTimer()
    {
        pause = false;
        timerText.text = ConvertTimeNoMilliseconds(time);
    }

    public void Restart()
    {
        gameFinished = false;
        time = 0;
        PlayerPrefs.SetFloat(PlayerPrefsManager.time, time);
        timerText.text = ConvertTimeNoMilliseconds(time);
    }

    public void SaveTime()
    {
        PlayerPrefs.SetFloat(PlayerPrefsManager.time, time);
    }

    public void LoadTime()
    {
        pause = false;
        time = PlayerPrefs.GetFloat(PlayerPrefsManager.time);
        timerText.text = ConvertTimeNoMilliseconds(time);
    }
}
