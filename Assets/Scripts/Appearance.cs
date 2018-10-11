using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appearance : MonoBehaviour
{
    public GameObject text, line;
    public GameObject textHolder, highlightHolder;
    Theme currentTheme;
    Theme lightTheme, darkTheme, paperTheme;
    public Image undo, erase, hint, direction;
    public List<Image> backgrounds;
    public List<Image> panels;
    public List<Text> texts;
    public List<GameObject> buttonsFromMenus;
    public List<Image> sliderFill;
    public GameObject startEndCirclePrefab, nodeHighlightPrefab;
    public GameObject startCircle, endCircle;
    public GameObject previousHighlight, nextHighlight;
    public Image normalConnection, diagonalConnection, normalBoard, diagonalBoard;
    public Slider thicknessSlider;
    public List<Sprite> normalConnectionSprites, diagonalConnectionSprites, normalBoardSprites, diagonalBoardSprites;

    public enum ThemeName { light, dark, paper };

    void Awake()
    {
        Application.targetFrameRate = 60;
        string theme = PlayerPrefs.GetString(PlayerPrefsManager.currentTheme);
        InitializeThemes();
        if (theme == "" || theme == lightTheme.nameOfTheme.ToString())
        {
            ChangeColors(lightTheme);
        }
        else if (theme == darkTheme.nameOfTheme.ToString())
        {
            ChangeColors(darkTheme);
        }
        else if (theme == paperTheme.nameOfTheme.ToString())
        {
            ChangeColors(paperTheme);
        }
        thicknessSlider.value = PlayerPrefs.GetInt(PlayerPrefsManager.lineThickness, 10);
    }

    public void SetNodeToLockedLook(GameObject node)
    {
        node.GetComponent<Image>().color = currentTheme.lockedNodeColor;
        if (node.GetComponent<Node>().text == null)
        {
            node.GetComponent<Node>().text = Instantiate(text, node.transform);
            node.GetComponent<Node>().text.transform.SetParent(textHolder.transform);
            node.GetComponent<Node>().text.GetComponent<Outline>().effectColor = currentTheme.lockedNodeColor;
        }
        if (node.GetComponent<Node>().line != null)
        {
            node.GetComponent<Node>().line.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        node.GetComponent<Node>().text.GetComponentInChildren<Text>().text = node.GetComponent<Node>().value.ToString();
        node.GetComponent<Node>().text.GetComponentInChildren<Text>().color = currentTheme.lockedNodeTextColor;
    }

    public void SetNodeToUserPlacedLook(GameObject node)
    {
        if (node.GetComponent<Node>().text == null)
        {
            node.GetComponent<Node>().text = Instantiate(text, node.transform);
            node.GetComponent<Node>().text.transform.SetParent(textHolder.transform);
            node.GetComponent<Node>().text.GetComponent<Outline>().effectColor = currentTheme.lockedNodeColor;
        }
        if (node.GetComponent<Node>().line != null)
        {
            node.GetComponent<Node>().line.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        node.GetComponent<Image>().color = currentTheme.userPlacedNodeColor;
        node.GetComponent<Node>().text.GetComponentInChildren<Text>().text = node.GetComponent<Node>().userValue.ToString();
        if (node.GetComponent<Node>().hinted == 1)
        {
            node.GetComponent<Node>().text.GetComponentInChildren<Text>().color = currentTheme.hintedNodeTextColor;
        }
        else
        {
            node.GetComponent<Node>().text.GetComponentInChildren<Text>().color = currentTheme.userPlacedNodeTextColor;
        }
    }

    public void SetNodeToEmptyLook(GameObject node)
    {
        if (node.GetComponent<Node>().text == null)
        {
            node.GetComponent<Node>().text = Instantiate(text, node.transform);
            node.GetComponent<Node>().text.transform.SetParent(textHolder.transform);
            node.GetComponent<Node>().text.GetComponent<Outline>().effectColor = currentTheme.lockedNodeColor;
        }
        if (node.GetComponent<Node>().line != null)
        {
            node.GetComponent<Node>().line.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        node.GetComponent<Image>().color = currentTheme.emptyNodeColor;
        node.GetComponent<Node>().text.GetComponentInChildren<Text>().text = "";
    }

    public void DrawLine(GameObject previousNode, GameObject currentNode)
    {
        GameObject newLine = Instantiate(line, GetComponent<BoardCreator>().boxHolders.transform, false);
        float lineX = (currentNode.transform.position.x + previousNode.transform.position.x) / 2;
        float lineY = (currentNode.transform.position.y + previousNode.transform.position.y) / 2;
        newLine.GetComponent<Image>().color = currentTheme.highlightColor;
        newLine.transform.position = new Vector3(lineX, lineY, 0);
        newLine.transform.SetParent(GetComponent<BoardCreator>().lineHolder.transform);
        SetLineThickness(newLine);
        if ((int)previousNode.GetComponent<Node>().position.y == (int)currentNode.GetComponent<Node>().position.y)
        {
            newLine.transform.Rotate(0, 0, 90);
        }
        else if ((int)previousNode.GetComponent<Node>().position.x != (int)currentNode.GetComponent<Node>().position.x)
        {
            if (GetComponent<BoardCreator>().GetDiagonal())
            {
                if (((int)previousNode.GetComponent<Node>().position.y - 1) == (int)currentNode.GetComponent<Node>().position.y &&
                    ((int)previousNode.GetComponent<Node>().position.x - 1) != (int)currentNode.GetComponent<Node>().position.x)
                {
                    newLine.transform.Rotate(0, 0, 45);
                    newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
                }
                else if (((int)previousNode.GetComponent<Node>().position.y + 1) == (int)currentNode.GetComponent<Node>().position.y &&
                         ((int)previousNode.GetComponent<Node>().position.x - 1) != (int)currentNode.GetComponent<Node>().position.x)
                {
                    newLine.transform.Rotate(0, 0, -45);
                    newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
                }
                else if (((int)previousNode.GetComponent<Node>().position.y - 1) == (int)currentNode.GetComponent<Node>().position.y &&
                         ((int)previousNode.GetComponent<Node>().position.x + 1) != (int)currentNode.GetComponent<Node>().position.x)
                {
                    newLine.transform.Rotate(0, 0, -45);
                    newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
                }
                else if (((int)previousNode.GetComponent<Node>().position.y + 1) == (int)currentNode.GetComponent<Node>().position.y &&
                         ((int)previousNode.GetComponent<Node>().position.x + 1) != (int)currentNode.GetComponent<Node>().position.x)
                {
                    newLine.transform.Rotate(0, 0, 45);
                    newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
                }
            }
        }
        if (previousNode.GetComponent<Node>().line != null)
        {
            Destroy(previousNode.GetComponent<Node>().line);
        }
        previousNode.GetComponent<Node>().line = newLine;
        previousNode.GetComponent<Node>().nextNode = currentNode;
        currentNode.GetComponent<Node>().previousNode = previousNode;
    }

    public void ChangeThemes(ThemeName name)
    {
        if (currentTheme.nameOfTheme != name)
        {
            PlayerPrefs.SetString(PlayerPrefsManager.currentTheme, name.ToString());
            PlayerPrefs.Save();
            switch (name)
            {
                case ThemeName.light:
                    ChangeColors(lightTheme);
                    break;
                case ThemeName.dark:
                    ChangeColors(darkTheme);
                    break;
                case ThemeName.paper:
                    ChangeColors(paperTheme);
                    break;
            }
            if (GetComponent<GameWon>())
            {
                GetComponent<GameWon>().ChangedTheme();
            }
        }
    }

    void ChangeColors(Theme newTheme)
    {
        currentTheme = newTheme;
        Camera.main.backgroundColor = currentTheme.backgroundColor;
        undo.color = currentTheme.generalButtonColor;
        erase.color = currentTheme.generalButtonColor;
        direction.color = currentTheme.generalButtonColor;
        hint.color = currentTheme.generalButtonColor;
        hint.gameObject.transform.GetChild(0).GetComponent<Image>().color = currentTheme.highlightColor;
        hint.gameObject.GetComponentInChildren<Text>().color = currentTheme.menuButtonTextColor;
        List<GameObject> gameBoard = GetComponent<BoardCreator>().GetGameBoard();
        GameObject content = GetComponent<NumberScroller>().GetContent();
        for (int i = 0; i < gameBoard.Count; i++)
        {
            if (gameBoard[i].GetComponent<Node>().text != null)
            {
                gameBoard[i].GetComponent<Node>().text.GetComponent<Outline>().effectColor = currentTheme.lockedNodeColor;
            }
            if (gameBoard[i].GetComponent<Node>().lockedValue)
            {
                SetNodeToLockedLook(gameBoard[i]);
            }
            else
            {
                if (gameBoard[i].GetComponent<Node>().userValue > 0)
                {
                    SetNodeToUserPlacedLook(gameBoard[i]);
                }
                else
                {
                    SetNodeToEmptyLook(gameBoard[i]);
                }
            }
        }
        for (int i = 0; i < content.transform.childCount; i++)
        {
            content.transform.GetChild(i).GetComponent<Image>().color = currentTheme.menuButtonColor;
            content.transform.GetChild(i).GetComponentInChildren<Text>().color = currentTheme.menuButtonTextColor;
        }
        for (int i = 0; i < buttonsFromMenus.Count; i++)
        {
            buttonsFromMenus[i].GetComponent<Image>().color = currentTheme.menuButtonColor;
            for (int j = 0; j < buttonsFromMenus[i].transform.childCount; j++)
            {
                if (buttonsFromMenus[i].transform.GetChild(j).GetComponentInChildren<Text>() != null)
                {
                    buttonsFromMenus[i].transform.GetChild(j).GetComponentInChildren<Text>().color = currentTheme.menuButtonTextColor;
                }
            }
        }
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].color = currentTheme.panelColor;
        }
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].color = currentTheme.generalButtonColor;
        }
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].color = currentTheme.backgroundColor;
        }
        for (int i = 0; i < sliderFill.Count; i++)
        {
            sliderFill[i].color = currentTheme.highlightColor;
        }
        if (startCircle != null)
        {
            startCircle.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        if (endCircle != null)
        {
            endCircle.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        if (nextHighlight != null)
        {
            nextHighlight.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        if (previousHighlight != null)
        {
            previousHighlight.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        GetComponent<NumberScroller>().ChangeHighlightColor();
        int imageIndex = 0;
        switch (currentTheme.nameOfTheme)
        {
            case ThemeName.light:
                imageIndex = 0;
                break;
            case ThemeName.dark:
                imageIndex = 1;
                break;
            case ThemeName.paper:
                imageIndex = 2;
                break;
        }
        normalConnection.sprite = normalConnectionSprites[imageIndex];
        diagonalConnection.sprite = diagonalConnectionSprites[imageIndex];
        normalBoard.sprite = normalBoardSprites[imageIndex];
        diagonalBoard.sprite = diagonalBoardSprites[imageIndex];
    }

    public void NodeFeedBack(Transform node, GameObject box)
    {
        GameObject feedback = Instantiate(box, node);
        feedback.transform.SetParent(textHolder.transform);
        Destroy(feedback.GetComponent<Node>());
        Destroy(feedback.GetComponent<Button>());
        feedback.AddComponent<NodeFeedback>();
    }

    public void CreateStartEndCircle(GameObject node)
    {
        if (node.GetComponent<Node>().lockedValue)
        {
            if (node.GetComponent<Node>().value == GetComponent<BoardCreator>().GetBoardNumberAmount())
            {
                CreateCircleSprite(false, node);
            }
            else if (node.GetComponent<Node>().value == 1)
            {
                CreateCircleSprite(true, node);
            }
        }
        else
        {
            if (node.GetComponent<Node>().userValue == GetComponent<BoardCreator>().GetBoardNumberAmount())
            {
                CreateCircleSprite(false, node);
            }
            else if (node.GetComponent<Node>().userValue == 1)
            {
                CreateCircleSprite(true, node);
            }
        }
    }

    void CreateCircleSprite(bool start, GameObject node)
    {
        GameObject circle = Instantiate(startEndCirclePrefab, node.transform);
        circle.GetComponent<Image>().color = currentTheme.highlightColor;
        circle.transform.SetParent(GetComponent<BoardCreator>().lineHolder.transform);
        if (start)
        {
            if (startCircle != null)
            {
                Destroy(startCircle);
            }
            startCircle = circle;
        }
        else
        {
            if (endCircle != null)
            {
                Destroy(endCircle);
            }
            endCircle = circle;
        }
    }

    public void DestroyCircleByValue(int value)
    {
        if (value == 1)
        {
            if (startCircle != null)
            {
                Destroy(startCircle);
            }
        }
        else
        {
            if (endCircle != null)
            {
                Destroy(endCircle);
            }
        }
    }

    public void DestroyAllCircles()
    {
        if (startCircle != null)
        {
            Destroy(startCircle);
        }
        if (endCircle != null)
        {
            Destroy(endCircle);
        }
    }

    public void FindNextNumber(int value)
    {
        bool foundNext = false;
        bool foundPrevious = false;
        for (int i = value + 1; i < GetComponent<BoardCreator>().GetBoardNumberAmount() + 1; i++)
        {
            if (GetComponent<BoardCreator>().GetUserPlacedNodes().ContainsKey(i))
            {
                SetNodeToHighlight(GetComponent<BoardCreator>().GetUserPlacedNodes()[i], true);
                foundNext = true;
                break;
            }
            if (GetComponent<BoardCreator>().GetLockedPlacedNodes().ContainsKey(i))
            {
                SetNodeToHighlight(GetComponent<BoardCreator>().GetLockedPlacedNodes()[i], true);
                foundNext = true;
                break;
            }
        }
        for (int i = value - 1; i > 0; i--)
        {
            if (GetComponent<BoardCreator>().GetUserPlacedNodes().ContainsKey(i))
            {
                SetNodeToHighlight(GetComponent<BoardCreator>().GetUserPlacedNodes()[i], false);
                foundPrevious = true;
                break;
            }
            if (GetComponent<BoardCreator>().GetLockedPlacedNodes().ContainsKey(i))
            {
                SetNodeToHighlight(GetComponent<BoardCreator>().GetLockedPlacedNodes()[i], false);
                foundPrevious = true;
                break;
            }
        }
        if (nextHighlight != null && !foundNext)
        {
            Destroy(nextHighlight);
        }
        if (previousHighlight != null && !foundPrevious)
        {
            Destroy(previousHighlight);
        }
    }

    void SetNodeToHighlight(GameObject node, bool next)
    {
        if (next)
        {
            if (nextHighlight == null)
            {
                GameObject nodeHighlight = Instantiate(nodeHighlightPrefab, node.transform);
                nodeHighlight.transform.SetParent(highlightHolder.transform);
                nodeHighlight.GetComponent<Image>().color = currentTheme.highlightColor;
                nextHighlight = nodeHighlight;
            }
            else
            {
                nextHighlight.transform.SetParent(node.transform);
                nextHighlight.transform.localPosition = Vector3.zero;
                nextHighlight.transform.SetParent(highlightHolder.transform);
            }
        }
        else
        {
            if (previousHighlight == null)
            {
                GameObject nodeHighlight = Instantiate(nodeHighlightPrefab, node.transform);
                nodeHighlight.transform.SetParent(highlightHolder.transform);
                nodeHighlight.GetComponent<Image>().color = currentTheme.highlightColor;
                previousHighlight = nodeHighlight;
            }
            else
            {
                previousHighlight.transform.SetParent(node.transform);
                previousHighlight.transform.localPosition = Vector3.zero;
                previousHighlight.transform.SetParent(highlightHolder.transform);
            }
        }
    }

    public void RemoveNodeHighlight()
    {
        if (nextHighlight != null)
        {
            Destroy(nextHighlight);
        }
        nextHighlight = null;
        if (previousHighlight != null)
        {
            Destroy(previousHighlight);
        }
        previousHighlight = null;
    }

    public void MakeNodeHighlightClear(bool clear)
    {
        if (clear)
        {
            if (nextHighlight != null)
            {
                nextHighlight.GetComponent<Image>().color = GetClearOfColor(nextHighlight.GetComponent<Image>().color);
            }
            if (previousHighlight != null)
            {
                previousHighlight.GetComponent<Image>().color = GetClearOfColor(previousHighlight.GetComponent<Image>().color);
            }
        }
        else
        {
            if (nextHighlight != null)
            {
                nextHighlight.GetComponent<Image>().color = GetOpaqueOfColor(nextHighlight.GetComponent<Image>().color);
            }
            if (previousHighlight != null)
            {
                previousHighlight.GetComponent<Image>().color = GetOpaqueOfColor(previousHighlight.GetComponent<Image>().color);
            }
        }
    }

    public void SaveThickness()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.lineThickness, 10) != (int)thicknessSlider.value)
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.lineThickness, (int)thicknessSlider.value);
            ChangeLineThickness();
        }
    }

    void ChangeLineThickness()
    {
        GameObject lineHolder = GetComponent<BoardCreator>().lineHolder;
        for (int i = 0; i < lineHolder.transform.childCount; i++)
        {
            if (lineHolder.transform.GetChild(i).name == "Line(Clone)")
            {
                SetLineThickness(lineHolder.transform.GetChild(i).gameObject);
            }
        }
    }

    void SetLineThickness(GameObject lineObj)
    {
        Vector2 oldSize = lineObj.GetComponent<RectTransform>().sizeDelta;
        lineObj.GetComponent<RectTransform>().sizeDelta = new Vector2(thicknessSlider.value, oldSize.y);
    }

    Color GetClearOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 0);
    }

    Color GetOpaqueOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 1);
    }

    void InitializeThemes()
    {
        lightTheme = new Theme
        {
            nameOfTheme = ThemeName.light,
            backgroundColor = new Color(0.9f, 0.9f, 0.9f),
            panelColor = new Color(1, 1, 1),

            highlightColor = new Color(1, 0.5f, 0),
            generalButtonColor = new Color(0, 0, 0),
            menuButtonColor = new Color(1, 1, 1),
            menuButtonTextColor = new Color(0, 0, 0),

            lockedNodeColor = new Color(1, 1, 1),
            userPlacedNodeColor = new Color(1, 1, 1),
            emptyNodeColor = new Color(0.75f, 0.75f, 0.75f),
            lockedNodeTextColor = new Color(0, 0.25f, 0.75f),
            userPlacedNodeTextColor = new Color(0, 0, 0),
            hintedNodeTextColor = new Color(0, 0.5f, 0)
        };
        darkTheme = new Theme
        {
            nameOfTheme = ThemeName.dark,
            backgroundColor = new Color(0.125f, 0.125f, 0.125f),
            panelColor = new Color(0.1f, 0.1f, 0.1f),

            highlightColor = new Color(1, 0.5f, 0.5f),
            generalButtonColor = new Color(0.9f, 0.9f, 0.9f),
            menuButtonColor = new Color(0.1f, 0.1f, 0.1f),
            menuButtonTextColor = new Color(0.9f, 0.9f, 0.9f),

            lockedNodeColor = new Color(0.4f, 0.4f, 0.4f),
            userPlacedNodeColor = new Color(0.4f, 0.4f, 0.4f),
            emptyNodeColor = new Color(0.2f, 0.2f, 0.2f),
            lockedNodeTextColor = new Color(1, 1, 0),
            userPlacedNodeTextColor = new Color(0.9f, 0.9f, 0.9f),
            hintedNodeTextColor = new Color(0.1f, 0.8f, 1)
        };
        paperTheme = new Theme
        {
            nameOfTheme = ThemeName.paper,
            backgroundColor = new Color(0.86f, 0.84f, 0.79f),
            panelColor = new Color(0.97f, 0.94f, 0.89f),

            highlightColor = new Color(0.47f, 0.67f, 0.76f),
            generalButtonColor = new Color(0.3f, 0.2f, 0.12f),
            menuButtonColor = new Color(0.97f, 0.94f, 0.89f),
            menuButtonTextColor = new Color(0.3f, 0.2f, 0.12f),

            lockedNodeColor = new Color(0.97f, 0.94f, 0.89f),
            userPlacedNodeColor = new Color(0.97f, 0.94f, 0.89f),
            emptyNodeColor = new Color(0.74f, 0.71f, 0.65f),
            lockedNodeTextColor = new Color(0.9f, 0.25f, 0),
            userPlacedNodeTextColor = new Color(0.3f, 0.2f, 0.12f),
            hintedNodeTextColor = new Color(1, 0.73f, 0)
        };
    }

    public Theme CurrentTheme()
    {
        return currentTheme;
    }

    public class Theme
    {
        public ThemeName nameOfTheme;
        public Color backgroundColor;
        public Color panelColor;
        public Color highlightColor;
        public Color generalButtonColor;
        public Color menuButtonColor;
        public Color menuButtonTextColor;
        public Color lockedNodeColor;
        public Color userPlacedNodeColor;
        public Color emptyNodeColor;
        public Color lockedNodeTextColor;
        public Color userPlacedNodeTextColor;
        public Color hintedNodeTextColor;
    }
}
