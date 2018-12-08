using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UnityAds : MonoBehaviour
{
    static int timeForNextAd = 5;
    float time;
    int hintCount = 1;
    Button m_Button;
#if UNITY_IOS
    string gameId = "2834216";
#elif UNITY_ANDROID
    string gameId = "2834217";
#endif
    public string placementId = "rewardedVideo";

    void Start()
    {
        hintCount = PlayerPrefs.GetInt(PlayerPrefsManager.hintCount, 1);
        SetHint(hintCount);
        m_Button = GetComponent<Button>();
        if (m_Button) m_Button.onClick.AddListener(ShowAd);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.removeAds, 0) == 0)
        {
            if (Advertisement.isSupported)
            {
                Advertisement.Initialize(gameId, true);
            }
        }
        SetHint(hintCount);
    }

    void Update()
    {
        if (m_Button)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsManager.removeAds, 0) == 0)
            {
                bool isReady = Advertisement.IsReady(placementId);
                if (isReady || hintCount > 0)
                {
                    m_Button.interactable = true;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    m_Button.interactable = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                if (time < timeForNextAd)
                {
                    time += Time.deltaTime;
                    m_Button.interactable = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    m_Button.interactable = true;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }

    void ShowAd()
    {
        if (!Camera.main.GetComponent<BoardCreator>().GetWinStatus())
        {
            hintCount = PlayerPrefs.GetInt(PlayerPrefsManager.hintCount, 1);
            time = 0;
            if (hintCount > 0)
            {
                if (PlayerPrefs.GetInt(PlayerPrefsManager.removeAds, 0) == 0)
                {
                    hintCount--;
                    SetHint(hintCount);
                }
                Camera.main.GetComponent<BoardCreator>().Hint();
                PlayerPrefs.SetInt(PlayerPrefsManager.hintCount, hintCount);
            }
            else
            {
                ShowOptions options = new ShowOptions();
                options.resultCallback = HandleShowResult;
                Advertisement.Show(placementId, options);
            }
        }
    }

    void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed - Offer a reward to the player");
                StartCoroutine("ShowHint");
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped - Do NOT reward the player");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show");
                break;
        }
    }

    IEnumerator ShowHint()
    {
        yield return new WaitForSeconds(0.5f);
        Camera.main.GetComponent<BoardCreator>().Hint();
    }

    public void SetHint(int count)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.removeAds, 0) == 1)
        {
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Inf";
        }
        else if (count > 0)
        {
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = count.ToString();
            hintCount = 1;
        }
        else
        {
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Ad";
            hintCount = 0;
        }
    }

    public void BoughtRemoveAds()
    {
        PlayerPrefs.SetInt(PlayerPrefsManager.removeAds, 1);
        PlayerPrefs.Save();
        SetHint(hintCount);
    }
}
