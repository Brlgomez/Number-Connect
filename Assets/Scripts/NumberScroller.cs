using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberScroller : MonoBehaviour
{
    public ScrollRect scrollView;
    public GameObject numberButtonPrefab;
    public GameObject content;
    public GameObject eraserButton;
    public GameObject directionButton;
    static int updateInterval = 2;
    static float numberButtonSize = 100;
    GameObject highLightedNumber;
    int highLightedValue;
    List<GameObject> numberButtonList = new List<GameObject>();
    List<GameObject> notPlacedNumbers;
    Dictionary<int, int> indexList = new Dictionary<int, int>();
    Vector2 originalSizeDelta;
    Vector3 originalPosition;
    int direction = 1;
    float previousSliderPos;

    void Start()
    {
        float height = content.transform.parent.transform.parent.GetComponent<RectTransform>().rect.height;
        numberButtonSize = height;
        numberButtonPrefab.GetComponent<RectTransform>().sizeDelta = Vector2.one * height;
        originalPosition = content.GetComponent<RectTransform>().transform.localPosition;
        originalSizeDelta = content.GetComponent<RectTransform>().sizeDelta;
        SetUpNumberScroller();
        if (PlayerPrefs.GetInt(PlayerPrefsManager.highlightPosition) >= 0)
        {
            ChangeHighlightedNumber(numberButtonList[PlayerPrefs.GetInt(PlayerPrefsManager.highlightPosition)], true);
        }
        else
        {
            PressEraser();
        }
    }

    public void SliderScrolling()
    {
        if (Time.frameCount % updateInterval == 0)
        {
            if (Mathf.Abs(previousSliderPos - content.GetComponent<RectTransform>().transform.localPosition.x) > numberButtonSize)
            {
                previousSliderPos = content.GetComponent<RectTransform>().transform.localPosition.x;
                GetComponent<HapticFeedback>().LightTapticFeedback();
                GetComponent<SoundManager>().PlayScrollSound();
            }
        }
    }

    public void SetUpNumberScroller()
    {
        notPlacedNumbers = GetComponent<BoardCreator>().GetNotPlacedNumbers();
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(
            notPlacedNumbers.Count * (numberButtonSize) - content.GetComponent<RectTransform>().rect.width,
            content.GetComponent<RectTransform>().sizeDelta.y
        );
        for (int i = 0; i < notPlacedNumbers.Count; i++)
        {
            GameObject newButton = Instantiate(numberButtonPrefab, content.transform, false);
            float x = 0;
            if (notPlacedNumbers.Count % 2 == 0)
            {
                x = ((i - notPlacedNumbers.Count / 2) * (numberButtonSize)) + (numberButtonSize / 2) + (content.GetComponent<RectTransform>().rect.width / 2);
            }
            else
            {
                x = ((i - notPlacedNumbers.Count / 2) * (numberButtonSize)) + (content.GetComponent<RectTransform>().rect.width / 2);
            }
            newButton.transform.localPosition = new Vector3(x, 0, 0);
            newButton.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
            newButton.GetComponentInChildren<Text>().color = GetComponent<Appearance>().CurrentTheme().generalButtonColor;
            newButton.GetComponentInChildren<Text>().text = notPlacedNumbers[i].GetComponent<Node>().value.ToString();
            newButton.GetComponent<NumberButton>().value = notPlacedNumbers[i].GetComponent<Node>().value;
            newButton.GetComponent<NumberButton>().index = i;
            numberButtonList.Add(newButton);
            indexList.Add(notPlacedNumbers[i].GetComponent<Node>().value, i);
            if (i == 0)
            {
                ChangeHighlightedNumber(newButton, false);
            }
        }
    }

    public void ChangeHighlightedNumber(GameObject button, bool changeContentPos)
    {
        if (highLightedNumber != null)
        {
            highLightedNumber.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        }
        else
        {
            GetComponent<Appearance>().MakeNodeHighlightClear(false);
        }
        highLightedNumber = button;
        highLightedValue = button.GetComponent<NumberButton>().value;
        highLightedNumber.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        eraserButton.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().generalButtonColor;
        if (changeContentPos)
        {
            if (Mathf.Abs(highLightedNumber.GetComponent<NumberButton>().transform.position.x - Screen.width / 2) > Screen.width / 2)
            {
                float shift = scrollView.GetComponent<RectTransform>().rect.width / numberButtonSize;
                content.GetComponent<RectTransform>().transform.localPosition = new Vector3(
                    -highLightedNumber.GetComponent<NumberButton>().index * numberButtonSize + (shift * 0.5f * numberButtonSize) - numberButtonSize / 2,
                    content.GetComponent<RectTransform>().transform.localPosition.y,
                    content.GetComponent<RectTransform>().transform.localPosition.z
                );
                previousSliderPos = content.GetComponent<RectTransform>().transform.localPosition.x;
            }
            PlayerPrefs.SetInt(PlayerPrefsManager.highlightPosition, highLightedNumber.GetComponent<NumberButton>().index);
        }
        GetComponent<Appearance>().FindNextNumberForNodeHighlight(highLightedValue);
    }

    public void HighlightNumber(int num)
    {
        if (indexList.ContainsKey(num))
        {
            int index = indexList[num];
            ChangeHighlightedNumber(numberButtonList[index], true);
        }
    }

    public void GoToNextNumber()
    {
        int currentNumber = highLightedNumber.GetComponent<NumberButton>().index;
        if (direction == 1)
        {
            if (highLightedNumber.GetComponent<NumberButton>().index < numberButtonList.Count - 1)
            {
                int i = 1;
                while (GetComponent<BoardCreator>().GetHintedNumbers().ContainsKey(numberButtonList[currentNumber + i].GetComponent<NumberButton>().value))
                {
                    i++;
                }
                if (currentNumber + i < numberButtonList.Count)
                {
                    ChangeHighlightedNumber(numberButtonList[currentNumber + i], true);
                }
            }
        }
        else
        {
            if (highLightedNumber.GetComponent<NumberButton>().index > 0)
            {
                int i = -1;
                while (GetComponent<BoardCreator>().GetHintedNumbers().ContainsKey(numberButtonList[currentNumber + i].GetComponent<NumberButton>().value))
                {
                    i--;
                }
                if (currentNumber + i >= 0)
                {
                    ChangeHighlightedNumber(numberButtonList[currentNumber + i], true);
                }
            }
        }
    }

    public void GoToNearbyNumberByNodeValue(int value)
    {
        if (direction == 1)
        {
            if (value != GetComponent<BoardCreator>().GetBoardNumberAmount())
            {
                NextNumberForward(value);
            }
            else
            {
                NextNumberBackwards(value);
            }
        }
        else
        {
            if (value != 1)
            {
                NextNumberBackwards(value);
            }
            else
            {
                NextNumberForward(value);
            }
        }
    }

    void NextNumberForward(int value)
    {
        int num = value + 1;
        while (!indexList.ContainsKey(num))
        {
            num++;
        }
        HighlightNumber(num);
    }

    void NextNumberBackwards(int value)
    {
        int num = value - 1;
        while (!indexList.ContainsKey(num))
        {
            num--;
        }
        HighlightNumber(num);
    }

    public void ChangeScrollerDirection()
    {
        if (direction == 1)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        GetComponent<Appearance>().direction.transform.Rotate(new Vector3(0, 180, 0));
        GetComponent<Appearance>().direction.transform.GetChild(0).transform.Rotate(new Vector3(0, 180, 0));
        GetComponent<SoundManager>().Direction();
    }

    public void PressEraser()
    {
        if (highLightedNumber != null)
        {
            highLightedNumber.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().menuButtonColor;
        }
        highLightedNumber = null;
        highLightedValue = -1;
        eraserButton.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        GetComponent<Appearance>().MakeNodeHighlightClear(true);
        PlayerPrefs.SetInt(PlayerPrefsManager.highlightPosition, -1);
    }

    public void GoToFirstButton()
    {
        ChangeHighlightedNumber(numberButtonList[0], true);
    }

    public void ClearNumberScroller()
    {
        content.GetComponent<RectTransform>().transform.localPosition = new Vector3(
                    0,
                    content.GetComponent<RectTransform>().transform.localPosition.y,
                    content.GetComponent<RectTransform>().transform.localPosition.z
        );
        for (int i = 0; i < numberButtonList.Count; i++)
        {
            Destroy(numberButtonList[i]);
        }
        numberButtonList.Clear();
        indexList.Clear();
        highLightedNumber = null;
        content.GetComponent<RectTransform>().sizeDelta = originalSizeDelta;
        content.GetComponent<RectTransform>().transform.localPosition = originalPosition;
        PlayerPrefs.SetInt(PlayerPrefsManager.highlightPosition, 0);
    }

    public GameObject GetContent()
    {
        return content;
    }

    public int GetHighLightedValue()
    {
        return highLightedValue;
    }

    public void ChangeHighlightColor()
    {
        if (highLightedValue == -1)
        {
            eraserButton.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
        }
        else
        {
            if (highLightedNumber != null)
            {
                highLightedNumber.GetComponent<Image>().color = GetComponent<Appearance>().CurrentTheme().highlightColor;
            }
        }
    }
}
