﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appearance : MonoBehaviour
{
    public GameObject textPrefab, nodeFeedbackPrefab;
    public GameObject textHolder, highlightHolder;
    public Image undo, erase, hint, direction;
    public List<Image> backgrounds;
    public List<Image> panels;
    public List<Text> texts;
    public List<GameObject> buttonsFromMenus;
    public List<Image> sliderFill;
    public Image normalConnection, diagonalConnection, normalBoard, diagonalBoard;
    public List<Sprite> normalConnectionSprites, diagonalConnectionSprites, normalBoardSprites, diagonalBoardSprites;
    public Text restartText;
    public GameObject settingNodes, settingText, settingLines;

    public GameObject startCircle, endCircle;
    public GameObject previousHighlight, nextHighlight;
    public GameObject holdObject;

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
        GameObject newLine = previousNode.GetComponent<Node>().line;
        Vector3 linePos = (currentNode.transform.position + previousNode.transform.position) / 2;
        Vector2 currentNodePos = currentNode.GetComponent<Node>().position;
        Vector2 previousNodePos = previousNode.GetComponent<Node>().position;
        float thickness = newLine.GetComponent<RectTransform>().sizeDelta.x;
        newLine.transform.position = linePos;
        newLine.transform.eulerAngles = new Vector3(0, 0, 0);
        newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(thickness, 60);
        if ((int)previousNodePos.y == (int)currentNode.GetComponent<Node>().position.y)
        {
            newLine.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if ((int)previousNodePos.x != (int)currentNodePos.x && GetComponent<BoardCreator>().GetDiagonal())
        {
            if (((int)previousNodePos.y - 1) == (int)currentNodePos.y &&
                ((int)previousNodePos.x - 1) != (int)currentNodePos.x)
            {
                newLine.transform.eulerAngles = new Vector3(0, 0, 45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(thickness, 80);
            }
            else if (((int)previousNodePos.y + 1) == (int)currentNodePos.y &&
                     ((int)previousNodePos.x - 1) != (int)currentNodePos.x)
            {
                newLine.transform.eulerAngles = new Vector3(0, 0, -45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(thickness, 80);
            }
            else if (((int)previousNodePos.y - 1) == (int)currentNodePos.y &&
                     ((int)previousNodePos.x + 1) != (int)currentNodePos.x)
            {
                newLine.transform.eulerAngles = new Vector3(0, 0, -45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(thickness, 80);
            }
            else if (((int)previousNodePos.y + 1) == (int)currentNodePos.y &&
                     ((int)previousNodePos.x + 1) != (int)currentNodePos.x)
            {
                newLine.transform.eulerAngles = new Vector3(0, 0, 45);
                newLine.GetComponent<RectTransform>().sizeDelta = new Vector2(thickness, 80);
            }
        }
        previousNode.GetComponent<Node>().nextNode = currentNode;
        currentNode.GetComponent<Node>().previousNode = previousNode;
    }

    public void ChangeThemes(Themes.ThemeName name)
    {
        if (currentTheme.nameOfTheme != name)
        {
            PlayerPrefs.SetString(PlayerPrefsManager.currentTheme, name.ToString());
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
        holdObject.GetComponent<Image>().color = currentTheme.lockedNodeColor;
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
        nextHighlight.GetComponent<Image>().color = currentTheme.highlightColor;
        previousHighlight.GetComponent<Image>().color = currentTheme.highlightColor;
        GetComponent<NumberScroller>().ChangeHighlightColor();
        ChangeHelpSprites();
        ChangeSettingNodesColors();
    }

    void ChangeHelpSprites()
    {
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
    }

    public void ChangeSettingNodesColors()
    {
        int i = 0;
        while (i < settingNodes.transform.childCount)
        {
            settingNodes.transform.GetChild(i).GetComponent<Image>().color = currentTheme.lockedNodeColor;
            settingLines.transform.GetChild(i).GetComponent<Image>().color = currentTheme.highlightColor;
            settingText.transform.GetChild(i).GetComponent<Outline>().effectColor = currentTheme.lockedNodeColor;
            if (i % 4 == 0)
            {
                settingText.transform.GetChild(i).GetComponent<Text>().color = currentTheme.lockedNodeTextColor;
            }
            else
            {
                settingText.transform.GetChild(i).GetComponent<Text>().color = currentTheme.userPlacedNodeTextColor;
            }
            i++;
        }
        settingLines.transform.GetChild(i - 1).GetComponent<Image>().color = currentTheme.highlightColor;
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
            startCircle.transform.position = new Vector3(10000, 10000, 0);
        }
        else
        {
            endCircle.transform.position = new Vector3(10000, 10000, 0);
        }
    }

    public void DestroyAllCircles()
    {
        startCircle.transform.position = new Vector3(10000, 10000, 0);
        endCircle.transform.position = new Vector3(10000, 10000, 0);
    }

    public void FindNextNumberForNodeHighlight(int value)
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
        if (!foundNext)
        {
            nextHighlight.transform.position = new Vector3(10000, 10000, 0);
        }
        if (!foundPrevious)
        {
            previousHighlight.transform.position = new Vector3(10000, 10000, 0);
        }
    }

    void SetNodeToHighlight(GameObject node, bool next)
    {
        if (next)
        {
            nextHighlight.transform.SetParent(node.transform);
            nextHighlight.transform.localPosition = Vector3.zero;
            nextHighlight.transform.SetParent(highlightHolder.transform);
        }
        else
        {
            previousHighlight.transform.SetParent(node.transform);
            previousHighlight.transform.localPosition = Vector3.zero;
            previousHighlight.transform.SetParent(highlightHolder.transform);
        }
    }

    public void RemoveNodeHighlight()
    {
        previousHighlight.transform.position = new Vector3(10000, 10000, 0);
        nextHighlight.transform.position = new Vector3(10000, 10000, 0);
    }

    public void MakeNodeHighlightClear(bool clear)
    {
        if (clear)
        {
            nextHighlight.GetComponent<Image>().color = GetClearOfColor(nextHighlight.GetComponent<Image>().color);
            previousHighlight.GetComponent<Image>().color = GetClearOfColor(previousHighlight.GetComponent<Image>().color);
        }
        else
        {
            nextHighlight.GetComponent<Image>().color = GetOpaqueOfColor(nextHighlight.GetComponent<Image>().color);
            previousHighlight.GetComponent<Image>().color = GetOpaqueOfColor(previousHighlight.GetComponent<Image>().color);
        }
    }

    public void ChangeLineThickness()
    {
        GameObject lineHolder = GetComponent<BoardCreator>().lineHolder;
        float thickness = GetComponent<Menus>().thicknessSlider.value;
        for (int i = 0; i < lineHolder.transform.childCount; i++)
        {
            if (lineHolder.transform.GetChild(i).name == "Line(Clone)")
            {
                SetLineThickness(lineHolder.transform.GetChild(i).gameObject, thickness);
            }
        }
    }

    public void ChangeSettingLinesThickness(int thickness)
    {
        for (int i = 0; i < settingLines.transform.childCount; i++)
        {
            if (settingLines.name != "StartEndCircle(Clone)")
            {
                SetLineThickness(settingLines.transform.GetChild(i).gameObject, thickness);
            }
        }
    }

    void SetLineThickness(GameObject lineObj, float thickness)
    {
        Vector2 oldSize = lineObj.GetComponent<RectTransform>().sizeDelta;
        lineObj.GetComponent<RectTransform>().sizeDelta = new Vector2(thickness, oldSize.y);
    }

    public void MoveHoldObjectToCell(GameObject cell, int value, Color textColor)
    {
        if (holdObject.transform.GetChild(1).gameObject.GetComponent<HoldDownProgress>())
        {
            Destroy(holdObject.transform.GetChild(1).gameObject.GetComponent<HoldDownProgress>());
        }
        holdObject.transform.GetChild(1).gameObject.AddComponent<HoldDownProgress>();
        holdObject.transform.GetChild(0).GetComponent<Text>().text = value.ToString();
        holdObject.transform.GetChild(0).GetComponent<Text>().color = textColor;
        holdObject.transform.localPosition = new Vector3(
            cell.transform.localPosition.x,
            cell.transform.localPosition.y + 70,
            cell.transform.localPosition.z);
    }

    public void MoveHoldObjectOutOfScene()
    {
        holdObject.transform.position = new Vector3(10000, 10000);
    }

    public void RestartButtonNoSave()
    {
        restartText.text = "Restart";
    }

    public void RestartButtonSave()
    {
        restartText.text = "Restart";
    }

    public Color GetClearOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 0);
    }

    public Color GetOpaqueOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 1);
    }

    public Themes.Theme CurrentTheme()
    {
        return currentTheme;
    }
}
