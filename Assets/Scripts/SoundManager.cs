using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip pressNode;
    public AudioClip erase;
    public AudioClip undo;
    public AudioClip direction;
    public GameObject scrollObj, hintObj, winObj;

    public void PlayScrollSound()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 1)
        {
            scrollObj.GetComponent<AudioSource>().Play();
        }
    }

    public void PressNode()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 1)
        {
            Camera.main.GetComponent<AudioSource>().clip = pressNode;
            Camera.main.GetComponent<AudioSource>().volume = 0.6f;
            Camera.main.GetComponent<AudioSource>().pitch = Random.Range(1.5f, 1.6f);
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }

    public void EraseNode()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 1)
        {
            Camera.main.GetComponent<AudioSource>().clip = erase;
            Camera.main.GetComponent<AudioSource>().volume = 1;
            Camera.main.GetComponent<AudioSource>().pitch = Random.Range(0.975f, 1.025f);
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }

    public void Undo()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 1)
        {
            Camera.main.GetComponent<AudioSource>().clip = undo;
            Camera.main.GetComponent<AudioSource>().volume = 0.5f;
            Camera.main.GetComponent<AudioSource>().pitch = Random.Range(0.95f, 1.05f);
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }

    public void Direction()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 1)
        {
            Camera.main.GetComponent<AudioSource>().clip = direction;
            Camera.main.GetComponent<AudioSource>().volume = 0.2f;
            Camera.main.GetComponent<AudioSource>().pitch = 0.65f;
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }

    public void PlayHintSound()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 1)
        {
            hintObj.GetComponent<AudioSource>().Play();
        }
    }

    public void PlayWinSound()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.soundEffects, 1) == 1)
        {
            winObj.GetComponent<AudioSource>().Play();
        }
    }
}
