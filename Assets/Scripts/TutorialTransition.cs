using UnityEngine;
using UnityEngine.UI;

public class TutorialTransition : MonoBehaviour
{
    static float transitionLength = 1;

    GameObject background;
    GameObject infoHolder;
    bool turningOn;
    bool turningOff;
    Color initialColor;
    Color clear;
    Vector3 initialPosition;
    Vector3 backPosition;
    float alpha;
    float time;

    void Update()
    {
        if (turningOn)
        {
            time += Time.deltaTime;
            background.GetComponent<Image>().color = Color.Lerp(clear, initialColor, time);
            infoHolder.transform.position = Vector3.Slerp(backPosition, initialPosition, time);
            if (time > transitionLength)
            {
                turningOn = false;
                background.GetComponent<Image>().color = initialColor;
                infoHolder.transform.position = initialPosition;
            }
        }
        else if (turningOff)
        {
            time += Time.deltaTime;
            background.GetComponent<Image>().color = Color.Lerp(initialColor, clear, time);
            infoHolder.transform.position = Vector3.Slerp(initialPosition, backPosition, time);
            if (time > transitionLength)
            {
                background.SetActive(false);
                background.GetComponent<Image>().color = initialColor;
                infoHolder.transform.position = initialPosition;
                Destroy(GetComponent<TutorialTransition>());
            }
        }
    }

    public void TurnOn(GameObject obj)
    {
        turningOn = true;
        background = obj;
        background.SetActive(true);
        infoHolder = obj.transform.GetChild(0).gameObject;
        initialColor = background.GetComponent<Image>().color;
        clear = GetClearOfColor(initialColor);
        background.GetComponent<Image>().color = clear;
        initialPosition = infoHolder.transform.position;
        backPosition = new Vector3(initialPosition.x - Screen.width, initialPosition.y, initialPosition.z);
        infoHolder.transform.position = backPosition;

    }

    public void TurnOff()
    {
        turningOff = true;
        time = 0;
    }

    Color GetClearOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 0);
    }
}
