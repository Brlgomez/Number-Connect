using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appearance : MonoBehaviour
{
    public GameObject textPrefab, linePrefab, nodeHighlightPrefab, nodeFeedbackPrefab;
    public GameObject textHolder, highlightHolder;
    public Image undo, erase, hint, direction;
    public List<Image> backgrounds;
    public List<Image> panels;
    public List<Text> texts;
    public List<GameObject> buttonsFromMenus;
    public List<Image> sliderFill;
    public Image normalConnection, diagonalConnection, normalBoard, diagonalBoard;
    public Slider thicknessSlider;
    public List<Sprite> normalConnectionSprites, diagonalConnectionSprites, normalBoardSprites, diagonalBoardSprites;
    public Text restartText;
    public GameObject settingNodes, settingText, settingLines;

    public GameObject startCircle, endCircle;
    GameObject previousHighlight, nextHighlight;

    Themes.Theme currentTheme;

    void Awake()
    {
        Application.targetFrameRate = 60;
        string theme = PlayerPrefs.GetString(PlayerPrefsManager.currentTheme);
        if (theme == "" || theme == Themes.lightTheme.nameOfTheme.ToString())
        {
            ChangeColors(Themes.lightTheme);
        }
        else if (theme == Themes.darkTheme.nameOfTheme.ToString())
        {
            ChangeColors(Themes.darkTheme);
        }
        else if (theme == Themes.paperTheme.nameOfTheme.ToString())
        {
            ChangeColors(Themes.paperTheme);
        }
        if (PlayerPrefs.GetInt(PlayerPrefsManager.boardCompleted, 0) == 1)
        {
            GetComponent<Appearance>().RestartButtonNoSave();
        }
        else
        {
            GetComponent<Appearance>().RestartButtonSave();
        }
        thicknessSlider.value = PlayerPrefs.GetInt(PlayerPrefsManager.lineThickness, 10);
        ChangeSettingLinesThickness();
    }

    public void SetNodeToEmptyLook(GameObject node)
    {
        SetNode(node, currentTheme.emptyNodeColor, currentTheme.userPlacedNodeTextColor, "");
    }

    public void SetNodeToLockedLook(GameObject node)
    {
        SetNode(node, currentTheme.lockedNodeColor, currentTheme.lockedNodeTextColor, node.GetComponent<Node>().value.ToString());
    }

    public void SetNodeToUserPlacedLook(GameObject node)
    {
        if (node.GetComponent<Node>().hinted == 1)
        {
            SetNode(node, currentTheme.userPlacedNodeColor,
                    currentTheme.hintedNodeTextColor, node.GetComponent<Node>().userValue.ToString());
        }
        else
        {
            SetNode(node, currentTheme.userPlacedNodeColor,
                    currentTheme.userPlacedNodeTextColor, node.GetComponent<Node>().userValue.ToString());
        }
    }

    void SetNode(GameObject node, Color nodeColor, Color nodeTextColor, string nodeText)
    {
        if (node.GetComponent<Node>().text == null)
        {
            node.GetComponent<Node>().text = Instantiate(textPrefab, node.transform);
            node.GetComponent<Node>().text.transform.SetParent(textHolder.transform);
            node.GetComponent<Node>().text.GetComponent<Outline>().effectColor = currentTheme.lockedNodeColor;
        }
        if (node.GetComponent<Node>().line != null)
        {
            node.GetComponent<Node>().line.GetComponent<Image>().color = currentTheme.highlightColor;
        }
        node.GetComponent<Image>().color = nodeColor;
        node.GetComponent<Node>().text.GetComponentInChildren<Text>().color = nodeTextColor;
        node.GetComponent<Node>().text.GetComponentInChildren<Text>().text = nodeText;
    }

    public void DrawLine(GameObject previousNode, GameObject currentNode)
    {
        GameObject newLine = Instantiate(linePrefab, GetComponent<BoardCreator>().boxHolders.transform, false);
        float lineX = (currentNode.transform.position.x + previousNode.transform.position.x) / 2;
        float lineY = (currentNode.transform.position.y + previousNode.transform.position.y) / 2;
        Vector2 currentNodePos = currentNode.GetComponent<Node>().position;
        Vector2 previousNodePos = previousNode.GetComponent<Node>().position;
        newLine.GetComponent<Image>().color = currentTheme.highlightColor;
        newLine.transform.position = new Vector3(lineX, lineY, 0);
        newLine.transform.SetParent(GetComponent<BoardCreator>().lineHolder.transform);
        if ((int)previousNodePos.y == (int)currentNode.GetComponent<Node>().position.y)
        {
            newLine.transform.Rotate(0, 0, 90);
        }
        else if ((int)previousNodePos.x != (int)currentNodePos.x && GetComponent<BoardCreator>().GetDiagonal())
        {
            if (((int)previousNodePos.y - 1) == (int)currentNodePos.y &&
                ((int)previousNodePos.x - 1) != (int)currentNodePos.x)
            {
                newLine.transform.Rotate(0, 0, 45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
            }
            else if (((int)previousNodePos.y + 1) == (int)currentNodePos.y &&
                     ((int)previousNodePos.x - 1) != (int)currentNodePos.x)
            {
                newLine.transform.Rotate(0, 0, -45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
            }
            else if (((int)previousNodePos.y - 1) == (int)currentNodePos.y &&
                     ((int)previousNodePos.x + 1) != (int)currentNodePos.x)
            {
                newLine.transform.Rotate(0, 0, -45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
            }
            else if (((int)previousNodePos.y + 1) == (int)currentNodePos.y &&
                     ((int)previousNodePos.x + 1) != (int)currentNodePos.x)
            {
                newLine.transform.Rotate(0, 0, 45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 80);
            }
        }
        SetLineThickness(newLine);
        if (previousNode.GetComponent<Node>().line != null)
        {
            Destroy(previousNode.GetComponent<Node>().line);
        }
        previousNode.GetComponent<Node>().line = newLine;
        previousNode.GetComponent<Node>().nextNode = currentNode;
        currentNode.GetComponent<Node>().previousNode = previousNode;
    }

    public void ChangeThemes(Themes.ThemeName name)
    {
        if (currentTheme.nameOfTheme != name)
        {
            PlayerPrefs.SetString(PlayerPrefsManager.currentTheme, name.ToString());
            PlayerPrefs.Save();
            switch (name)
            {
                case Themes.ThemeName.light:
                    ChangeColors(Themes.lightTheme);
                    break;
                case Themes.ThemeName.dark:
                    ChangeColors(Themes.darkTheme);
                    break;
                case Themes.ThemeName.paper:
                    ChangeColors(Themes.paperTheme);
                    break;
            }
            if (GetComponent<GameWon>())
            {
                GetComponent<GameWon>().ChangedTheme();
            }
        }
    }

    void ChangeColors(Themes.Theme newTheme)
    {
        currentTheme = newTheme;
        Camera.main.backgroundColor = currentTheme.backgroundColor;
        undo.color = currentTheme.generalButtonColor;
        erase.color = currentTheme.generalButtonColor;
        direction.color = currentTheme.generalButtonColor;
        hint.color = currentTheme.generalButtonColor;
        hint.gameObject.transform.GetChild(0).GetComponent<Image>().color = currentTheme.highlightColor;
        hint.gameObject.GetComponentInChildren<Text>().color = currentTheme.generalButtonColor;
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
            content.transform.GetChild(i).GetComponentInChildren<Text>().color = currentTheme.generalButtonColor;
        }
        for (int i = 0; i < buttonsFromMenus.Count; i++)
        {
            buttonsFromMenus[i].GetComponent<Image>().color = currentTheme.menuButtonColor;
            for (int j = 0; j < buttonsFromMenus[i].transform.childCount; j++)
            {
                if (buttonsFromMenus[i].transform.GetChild(j).GetComponentInChildren<Text>() != null)
                {
                    buttonsFromMenus[i].transform.GetChild(j).GetComponentInChildren<Text>().color = currentTheme.generalButtonColor;
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
        startCircle.GetComponent<Image>().color = currentTheme.highlightColor;
        endCircle.GetComponent<Image>().color = currentTheme.highlightColor;
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
            case Themes.ThemeName.light:
                imageIndex = 0;
                break;
            case Themes.ThemeName.dark:
                imageIndex = 1;
                break;
            case Themes.ThemeName.paper:
                imageIndex = 2;
                break;
        }
        normalConnection.sprite = normalConnectionSprites[imageIndex];
        diagonalConnection.sprite = diagonalConnectionSprites[imageIndex];
        normalBoard.sprite = normalBoardSprites[imageIndex];
        diagonalBoard.sprite = diagonalBoardSprites[imageIndex];
        ChangeSettingNodesColors();
    }

    public void NodeFeedBack(Transform feedBackParent)
    {
        GameObject feedback = Instantiate(nodeFeedbackPrefab, feedBackParent);
        feedback.transform.SetParent(textHolder.transform);
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
        if (start)
        {
            startCircle.transform.position = node.transform.position;
        }
        else
        {
            endCircle.transform.position = node.transform.position;
        }
    }

    public void DestroyCircleByValue(int value)
    {
        if (value == 1)
        {
            startCircle.transform.position = new Vector3(1000, 1000, 0);
        }
        else
        {
            endCircle.transform.position = new Vector3(1000, 1000, 0);
        }
    }

    public void DestroyAllCircles()
    {
        startCircle.transform.position = new Vector3(1000, 1000, 0);
        endCircle.transform.position = new Vector3(1000, 1000, 0);
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

    public void MovingThicknessSlider()
    {
        ChangeSettingLinesThickness();
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

    public void RestartButtonNoSave()
    {
        restartText.text = "Restart";
    }

    public void RestartButtonSave()
    {
        restartText.text = "Restart";
    }

    Color GetClearOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 0);
    }

    Color GetOpaqueOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 1);
    }

    public void ChangeSettingNodesColors()
    {
        for (int i = 0; i < settingNodes.transform.childCount; i++)
        {
            settingNodes.transform.GetChild(i).GetComponent<Image>().color = currentTheme.lockedNodeColor;
        }
        for (int i = 0; i < settingText.transform.childCount; i++)
        {
            if (i % 4 == 0)
            {
                settingText.transform.GetChild(i).GetComponent<Text>().color = currentTheme.lockedNodeTextColor;
            }
            else
            {
                settingText.transform.GetChild(i).GetComponent<Text>().color = currentTheme.userPlacedNodeTextColor;
            }
            settingText.transform.GetChild(i).GetComponent<Outline>().effectColor = currentTheme.lockedNodeColor;
        }
        for (int i = 0; i < settingLines.transform.childCount; i++)
        {
            settingLines.transform.GetChild(i).GetComponent<Image>().color = currentTheme.highlightColor;
        }
    }

    public void ChangeSettingLinesThickness()
    {
        for (int i = 0; i < settingLines.transform.childCount; i++)
        {
            if (settingLines.name != "StartEndCircle(Clone)")
            {
                SetLineThickness(settingLines.transform.GetChild(i).gameObject);
            }
        }
    }

    public Themes.Theme CurrentTheme()
    {
        return currentTheme;
    }
}
