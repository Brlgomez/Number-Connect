﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public Canvas canvas;
    public GameObject box, linePrefab;
    public GameObject boxHolders, lineHolder, textHolder, holdDownHolder;
    public Text difficulty;
    public GameObject undoButton;
    static int nodeSize = 50;
    int amount;
    float percentRemoved;
    bool won;
    bool diagonal;
    List<GameObject> gameBoardAnswer = new List<GameObject>();
    Dictionary<Vector2, GameObject> gameBoardPositions = new Dictionary<Vector2, GameObject>();
    Dictionary<int, int> xAmounts = new Dictionary<int, int>();
    Dictionary<int, int> yAmounts = new Dictionary<int, int>();
    Dictionary<int, GameObject> userPlacedNodes = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> lockedNodes = new Dictionary<int, GameObject>();
    List<GameObject> notPlacedNumbers = new List<GameObject>();
    Dictionary<int, GameObject> hintedNumbers = new Dictionary<int, GameObject>();
    List<TempNode.TempNodeValues> tempGameBoardAnswer = new List<TempNode.TempNodeValues>();
    Dictionary<Vector2, TempNode.TempNodeValues> tempGameBoardPositions = new Dictionary<Vector2, TempNode.TempNodeValues>();

    void Start()
    {
        undoButton.GetComponent<Button>().interactable = false;
        if (PlayerPrefs.GetInt(PlayerPrefsManager.boardSize) == 0)
        {
            NewBoard(Difficulties.tutorial.boardCount,
                     Difficulties.tutorial.percentageEmpty,
                     Difficulties.tutorial.name,
                     Difficulties.tutorial.width,
                     Difficulties.tutorial.height,
                     Difficulties.tutorial.diagonals
                    );
        }
        else
        {
            Load();
        }
        if (PlayerPrefs.GetInt(PlayerPrefsManager.firstStartUp, 0) == 0)
        {
            GetComponent<Menus>().FirstPlayPopUpOpen();
        }
    }

    public void NewBoard(int boardSize, float percentage, string diff, int width, int height, bool diag)
    {
        difficulty.text = diff;
        amount = boardSize;
        percentRemoved = percentage;
        diagonal = diag;
        //int steps = 0;
        int i = 0;
        int increment = 1;
        int skip = 0;
        TempNode.TempNodeValues node = new TempNode.TempNodeValues(Vector2.zero, i + 1);
        tempGameBoardAnswer.Add(node);
        tempGameBoardPositions.Add(node.position, node);
        AddValue(Vector2.zero);
        i++;
        while (i < amount)
        {
            TempNode.TempNodeValues nextNode;
            TempNode.TempNodeValues previous = tempGameBoardAnswer[i - 1];
            List<Vector2> listOfPositions = GetListOfPositions(diagonal);
            Vector2 tempPosition = listOfPositions[Random.Range(0, listOfPositions.Count)];
            Vector2 nextPosition = previous.position + tempPosition;
            while (tempGameBoardPositions.ContainsKey(nextPosition) ||
                   (!xAmounts.ContainsKey((int)nextPosition.x) && xAmounts.Count >= width) ||
                   (!yAmounts.ContainsKey((int)nextPosition.y) && yAmounts.Count >= height))
            {
                listOfPositions.Remove(tempPosition);
                if (listOfPositions.Count == 0)
                {
                    bool restart = false;
                    skip++;
                    if (skip % ((boardSize - i) + 1) == 0)
                    {
                        increment++;
                    }
                    if (increment > tempGameBoardPositions.Count - 1)
                    {
                        restart = true;
                        increment = tempGameBoardPositions.Count - 1;
                    }
                    for (int k = 0; k < increment; k++)
                    {
                        i--;
                        RemoveValue(previous.position);
                        tempGameBoardPositions.Remove(previous.position);
                        tempGameBoardAnswer.Remove(previous);
                        previous = tempGameBoardAnswer[i - 1];
                    }
                    listOfPositions = GetListOfPositions(diagonal);
                    if (restart)
                    {
                        increment = 1;
                    }
                }
                tempPosition = listOfPositions[Random.Range(0, listOfPositions.Count)];
                nextPosition = previous.position + tempPosition;
            }
            nextNode = new TempNode.TempNodeValues(nextPosition, i + 1);
            tempGameBoardAnswer.Add(nextNode);
            tempGameBoardPositions.Add(nextNode.position, nextNode);
            AddValue(nextPosition);
            i++;
            //steps++;
        }
        //Debug.Log("Steps " + steps);
        for (int j = 0; j < tempGameBoardAnswer.Count; j++)
        {
            GameObject nextNode = Instantiate(box, boxHolders.transform);
            NodeComponent(nextNode, tempGameBoardAnswer[j].position, j + 1);
            gameBoardAnswer.Add(nextNode);
            gameBoardPositions.Add(tempGameBoardAnswer[j].position, nextNode);
        }
        tempGameBoardAnswer.Clear();
        tempGameBoardPositions.Clear();
        CheckCompletedBoard();
        ShiftBoard();
        Save();
    }

    List<Vector2> GetListOfPositions(bool isDiagonal)
    {
        List<Vector2> listOfPositions = new List<Vector2>
        {
            new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0)
        };
        if (isDiagonal)
        {
            listOfPositions.Add(new Vector2(1, 1));
            listOfPositions.Add(new Vector2(1, -1));
            listOfPositions.Add(new Vector2(-1, 1));
            listOfPositions.Add(new Vector2(-1, -1));
        }
        return listOfPositions;
    }

    void NodeComponent(GameObject nextNode, Vector2 nextPosition, int i)
    {
        nextNode.transform.localPosition = new Vector3(
            nextPosition.x * nodeSize,
            nextPosition.y * nodeSize,
            0
        );
        nextNode.GetComponent<Node>().position = new Vector2(
            nextPosition.x,
            nextPosition.y
        );
        nextNode.GetComponent<Node>().value = i;
        GameObject line = Instantiate(linePrefab, lineHolder.transform);
        line.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<Menus>().thicknessSlider.value, 60);
        line.transform.position = new Vector3(10000, 10000, 0);
        nextNode.GetComponent<Node>().line = line;
    }

    void AddValue(Vector2 value)
    {
        int x, y;
        if (xAmounts.TryGetValue((int)value.x, out x))
        {
            xAmounts[(int)value.x]++;
        }
        else
        {
            xAmounts.Add((int)value.x, 1);
        }
        if (yAmounts.TryGetValue((int)value.y, out y))
        {
            yAmounts[(int)value.y]++;
        }
        else
        {
            yAmounts.Add((int)value.y, 1);
        }
    }

    void RemoveValue(Vector2 value)
    {
        xAmounts[(int)value.x]--;
        if (xAmounts[(int)value.x] == 0)
        {
            xAmounts.Remove((int)value.x);

        }
        yAmounts[(int)value.y]--;
        if (yAmounts[(int)value.y] == 0)
        {
            yAmounts.Remove((int)value.y);
        }
    }

    void CheckCompletedBoard()
    {
        int previousValue = 0;
        int emptySpots = Mathf.RoundToInt(gameBoardAnswer.Count * percentRemoved);
        List<GameObject> tempBoard = new List<GameObject>();
        Dictionary<int, GameObject> tempNodes = new Dictionary<int, GameObject>();
        for (int i = 0; i < gameBoardAnswer.Count; i++)
        {
            tempBoard.Add(gameBoardAnswer[i]);
        }
        for (int i = 0; i < emptySpots; i++)
        {
            GameObject randomSelection = tempBoard[Random.Range(0, tempBoard.Count)];
            tempBoard.Remove(randomSelection);
            tempNodes.Add(randomSelection.GetComponent<Node>().value, randomSelection);
        }
        for (int j = 0; j < gameBoardAnswer.Count; j++)
        {
            if (tempNodes.ContainsKey(gameBoardAnswer[j].GetComponent<Node>().value))
            {
                GetComponent<Appearance>().SetNodeToEmptyLook(gameBoardAnswer[j]);
                gameBoardAnswer[j].GetComponent<Node>().lockedValue = false;
                notPlacedNumbers.Add(gameBoardAnswer[j]);
            }
            else
            {
                GetComponent<Appearance>().SetNodeToLockedLook(gameBoardAnswer[j]);
                gameBoardAnswer[j].GetComponent<Node>().lockedValue = true;
                lockedNodes.Add(gameBoardAnswer[j].GetComponent<Node>().value, gameBoardAnswer[j]);
                if ((previousValue + 1) == gameBoardAnswer[j].GetComponent<Node>().value && j != 0 && gameBoardAnswer[j - 1].GetComponent<Node>().lockedValue)
                {
                    GetComponent<Appearance>().DrawLine(gameBoardAnswer[j - 1], gameBoardAnswer[j]);
                }
            }
            previousValue = gameBoardAnswer[j].GetComponent<Node>().value;
            if (j == 0 || j == gameBoardAnswer.Count - 1)
            {
                GetComponent<Appearance>().CreateStartEndCircle(gameBoardAnswer[j]);
            }
        }
    }

    void ShiftBoard()
    {
        float middleX = 0;
        int lowestX = 0;
        int highestX = 0;
        float middleY = 0;
        int lowestY = 0;
        int highestY = 0;
        foreach (KeyValuePair<int, int> xAmount in xAmounts)
        {
            if (lowestX > xAmount.Key)
            {
                lowestX = xAmount.Key;
            }
            if (highestX < xAmount.Key)
            {
                highestX = xAmount.Key;
            }
        }
        foreach (KeyValuePair<int, int> yAmount in yAmounts)
        {
            if (lowestY > yAmount.Key)
            {
                lowestY = yAmount.Key;
            }
            if (highestY < yAmount.Key)
            {
                highestY = yAmount.Key;
            }
        }
        middleX = ((float)lowestX + highestX) / 2;
        middleY = ((float)lowestY + highestY) / 2;
        boxHolders.transform.position = new Vector3(
            boxHolders.transform.position.x - ((middleX * nodeSize) * canvas.scaleFactor),
            boxHolders.transform.position.y - ((middleY * nodeSize) * canvas.scaleFactor) + (Screen.height / 20f),
            0
        );
        lineHolder.transform.position = boxHolders.transform.position;
        textHolder.transform.position = boxHolders.transform.position;
        holdDownHolder.transform.position = boxHolders.transform.position;
        GetComponent<ZoomInBoard>().Zoom(xAmounts.Count, yAmounts.Count);
    }

    public void PressedNode(GameObject node)
    {
        if (!won)
        {
            if (!node.GetComponent<Node>().lockedValue)
            {
                int highLightedValue = GetComponent<NumberScroller>().GetHighLightedValue();
                if (highLightedValue > 0)
                {
                    AddNode(node, highLightedValue, true, 0);
                    GetComponent<NumberScroller>().GoToNextNumber();
                    GetComponent<SoundManager>().PressNode();
                    GetComponent<HapticFeedback>().SelectTapticFeedback();
                }
                else
                {
                    if (node.GetComponent<Node>().userValue > 0)
                    {
                        RemoveNode(node, true);
                        GetComponent<SoundManager>().EraseNode();
                        GetComponent<HapticFeedback>().SelectTapticFeedback();
                    }
                }
                RecheckNeighborConnections(node);
            }
            /*
            else if (node.GetComponent<Node>().lockedValue)
            {
                if (GetComponent<NumberScroller>().GetHighLightedValue() != -1)
                {
                    GetComponent<NumberScroller>().GoToNearbyNumberByNodeValue(node.GetComponent<Node>().value);
                    GetComponent<HapticFeedback>().MediumTapticFeedback();
                    GetComponent<SoundManager>().PlayScrollSound();
                }
                else
                {
                    GetComponent<HapticFeedback>().ErrorTapticFeedback();
                }
            }
            */
        }
    }

    void AddNode(GameObject node, int value, bool saveAction, int hinted)
    {
        Vector2 nodePosition = node.GetComponent<Node>().position;
        UserAction.Action action = new UserAction.Action
        {
            added = true,
            position = nodePosition,
            value = value,
            hinted = hinted
        };
        if (!saveAction)
        {
            action = null;
        }
        if (hinted == 1 && !hintedNumbers.ContainsKey(node.GetComponent<Node>().value))
        {
            hintedNumbers.Add(node.GetComponent<Node>().value, node);
        }
        RemoveNodeConnections(node, action);
        node.GetComponent<Node>().hinted = hinted;
        node.GetComponent<Node>().userValue = value;
        SaveByIndex(node.GetComponent<Node>().value - 1);
        GetComponent<Appearance>().SetNodeToUserPlacedLook(node);
        CheckIfNodePlacedSomewhereElse(action, value);
        GetComponent<UserAction>().AddAction(action);
        undoButton.GetComponent<Button>().interactable |= GetComponent<UserAction>().HaveActions();
        userPlacedNodes.Add(value, node);
        AddLineToNeighbors(nodePosition, node);
        if (value == 1 || value == gameBoardAnswer.Count)
        {
            GetComponent<Appearance>().CreateStartEndCircle(node);
        }
        if (CheckIfWon())
        {
            bool newBest;
            won = true;
            GetComponent<Timer>().GameFinished();
            GetComponent<Menus>().WinMenuOpen();
            newBest = GetComponent<PlayerPrefsManager>().SaveStats();
            GetComponent<CongratsScreenManager>().SetUpGameWonScreen(newBest);
            GetComponent<Appearance>().MakeNodeHighlightClear(true);
            GetComponent<HapticFeedback>().SuccessTapticFeedback();
            GetComponent<SoundManager>().PlayWinSound();
        }
    }

    void RemoveNode(GameObject node, bool saveAction)
    {
        UserAction.Action action = new UserAction.Action
        {
            added = false,
            position = node.GetComponent<Node>().position,
            value = node.GetComponent<Node>().userValue,
            hinted = node.GetComponent<Node>().hinted
        };
        if (saveAction)
        {
            GetComponent<UserAction>().AddAction(action);
            undoButton.GetComponent<Button>().interactable |= GetComponent<UserAction>().HaveActions();
        }
        RemoveNodeConnections(node, null);
        node.GetComponent<Node>().userValue = -1;
        SaveByIndex(node.GetComponent<Node>().value - 1);
        GetComponent<Appearance>().SetNodeToEmptyLook(node);
    }

    void AddLineToNeighbors(Vector2 nodePosition, GameObject node)
    {
        List<Vector2> checkPositions = GetNeighboringPositions(nodePosition);
        for (int i = 0; i < checkPositions.Count; i++)
        {
            if (gameBoardPositions.ContainsKey(checkPositions[i]))
            {
                CheckNeighborsNumber(checkPositions[i], node);
            }
        }
    }

    List<Vector2> GetNeighboringPositions(Vector2 nodePosition)
    {
        List<Vector2> positions = new List<Vector2>
        {
            new Vector2(nodePosition.x, nodePosition.y + 1),
            new Vector2(nodePosition.x, nodePosition.y - 1),
            new Vector2(nodePosition.x - 1, nodePosition.y),
            new Vector2(nodePosition.x + 1, nodePosition.y)
        };
        if (diagonal)
        {
            positions.Add(new Vector2(nodePosition.x + 1, nodePosition.y + 1));
            positions.Add(new Vector2(nodePosition.x + 1, nodePosition.y - 1));
            positions.Add(new Vector2(nodePosition.x - 1, nodePosition.y + 1));
            positions.Add(new Vector2(nodePosition.x - 1, nodePosition.y - 1));
        }
        return positions;
    }

    void RemoveNodeConnections(GameObject node, UserAction.Action action)
    {
        if (node.GetComponent<Node>().userValue > 0)
        {
            if (action != null)
            {
                action.removedOther = true;
                action.otherPosition = node.GetComponent<Node>().position;
                action.otherValue = node.GetComponent<Node>().userValue;
                action.otherHinted = node.GetComponent<Node>().hinted;
            }
            if (node.GetComponent<Node>().userValue == 1 || node.GetComponent<Node>().userValue == gameBoardAnswer.Count)
            {
                GetComponent<Appearance>().DestroyCircleByValue(node.GetComponent<Node>().userValue);
            }
            if (node.GetComponent<Node>().nextNode != null)
            {
                node.GetComponent<Node>().line.transform.position = new Vector3(10000, 10000, 0);
                node.GetComponent<Node>().nextNode = null;
            }
            if (node.GetComponent<Node>().previousNode != null)
            {
                node.GetComponent<Node>().previousNode.GetComponent<Node>().line.transform.position = new Vector3(10000, 10000, 0);
                node.GetComponent<Node>().previousNode.GetComponent<Node>().nextNode = null;
            }
            if (node.GetComponent<Node>().hinted == 1 && hintedNumbers.ContainsKey(node.GetComponent<Node>().value))
            {
                hintedNumbers.Remove(node.GetComponent<Node>().value);
            }
            userPlacedNodes.Remove(node.GetComponent<Node>().userValue);
        }
    }

    void CheckIfNodePlacedSomewhereElse(UserAction.Action action, int value)
    {
        if (userPlacedNodes.ContainsKey(value))
        {
            GameObject node = userPlacedNodes[value];
            if (action != null)
            {
                if (!action.removedOther)
                {
                    action.removedOther = true;
                    action.otherPosition = node.GetComponent<Node>().position;
                    action.otherValue = node.GetComponent<Node>().userValue;
                    action.otherHinted = node.GetComponent<Node>().hinted;
                }
                else
                {
                    action.removedOtherOther = true;
                    action.otherOtherPosition = node.GetComponent<Node>().position;
                    action.otherOtherValue = node.GetComponent<Node>().userValue;
                    action.otherOtherHinted = node.GetComponent<Node>().hinted;
                }
            }
            if (node.GetComponent<Node>().userValue == 1 || node.GetComponent<Node>().userValue == gameBoardAnswer.Count)
            {
                GetComponent<Appearance>().DestroyCircleByValue(node.GetComponent<Node>().userValue);
            }
            if (node.GetComponent<Node>().nextNode != null)
            {
                node.GetComponent<Node>().line.transform.position = new Vector3(10000, 10000, 0);
                node.GetComponent<Node>().nextNode = null;
            }
            if (node.GetComponent<Node>().previousNode != null)
            {
                node.GetComponent<Node>().previousNode.GetComponent<Node>().line.transform.position = new Vector3(10000, 10000, 0);
                node.GetComponent<Node>().previousNode.GetComponent<Node>().nextNode = null;
            }
            if (node.GetComponent<Node>().hinted == 1 && hintedNumbers.ContainsKey(node.GetComponent<Node>().value))
            {
                hintedNumbers.Remove(node.GetComponent<Node>().value);
            }
            node.GetComponent<Node>().userValue = -1;
            SaveByIndex(node.GetComponent<Node>().value - 1);
            GetComponent<Appearance>().SetNodeToEmptyLook(node);
            userPlacedNodes.Remove(value);
            RecheckNeighborConnections(node);
        }
    }

    void CheckNeighborsNumber(Vector2 pos, GameObject node)
    {
        if (gameBoardPositions[pos].GetComponent<Node>().lockedValue)
        {
            if (!node.GetComponent<Node>().lockedValue)
            {
                if (gameBoardPositions[pos].GetComponent<Node>().value == (node.GetComponent<Node>().userValue - 1))
                {
                    GetComponent<Appearance>().DrawLine(gameBoardPositions[pos], node);
                }
                else if (gameBoardPositions[pos].GetComponent<Node>().value == (node.GetComponent<Node>().userValue + 1))
                {
                    GetComponent<Appearance>().DrawLine(node, gameBoardPositions[pos]);
                }
            }
            if (node.GetComponent<Node>().lockedValue)
            {
                if (gameBoardPositions[pos].GetComponent<Node>().value == (node.GetComponent<Node>().value - 1))
                {
                    GetComponent<Appearance>().DrawLine(gameBoardPositions[pos], node);
                }
                else if (gameBoardPositions[pos].GetComponent<Node>().value == (node.GetComponent<Node>().value + 1))
                {
                    GetComponent<Appearance>().DrawLine(node, gameBoardPositions[pos]);
                }
            }
        }
        else
        {
            if (!node.GetComponent<Node>().lockedValue)
            {
                if (gameBoardPositions[pos].GetComponent<Node>().userValue == (node.GetComponent<Node>().userValue - 1))
                {
                    GetComponent<Appearance>().DrawLine(gameBoardPositions[pos], node);
                }
                else if (gameBoardPositions[pos].GetComponent<Node>().userValue == (node.GetComponent<Node>().userValue + 1))
                {
                    GetComponent<Appearance>().DrawLine(node, gameBoardPositions[pos]);
                }
            }
            if (node.GetComponent<Node>().lockedValue)
            {
                if (gameBoardPositions[pos].GetComponent<Node>().userValue == (node.GetComponent<Node>().value - 1))
                {
                    GetComponent<Appearance>().DrawLine(gameBoardPositions[pos], node);
                }
                else if (gameBoardPositions[pos].GetComponent<Node>().userValue == (node.GetComponent<Node>().value + 1))
                {
                    GetComponent<Appearance>().DrawLine(node, gameBoardPositions[pos]);
                }
            }
        }
    }

    bool CheckIfWon()
    {
        if (userPlacedNodes.Count - notPlacedNumbers.Count == 0)
        {
            for (int i = 0; i < gameBoardAnswer.Count; i++)
            {
                if (gameBoardAnswer[i].GetComponent<Node>().lockedValue)
                {
                    if (gameBoardAnswer[i].GetComponent<Node>().previousNode == null && gameBoardAnswer[i].GetComponent<Node>().value != 1)
                    {
                        return false;
                    }
                    if (gameBoardAnswer[i].GetComponent<Node>().nextNode == null && gameBoardAnswer[i].GetComponent<Node>().value != amount)
                    {
                        return false;
                    }
                }
                else
                {
                    if (gameBoardAnswer[i].GetComponent<Node>().previousNode == null && gameBoardAnswer[i].GetComponent<Node>().userValue != 1)
                    {
                        return false;
                    }
                    if (gameBoardAnswer[i].GetComponent<Node>().nextNode == null && gameBoardAnswer[i].GetComponent<Node>().userValue != amount)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }

    public void Undo()
    {
        if (!won)
        {
            UserAction.Action action = GetComponent<UserAction>().PopAction();
            if (action != null)
            {
                GameObject node = gameBoardPositions[action.position];
                if (action.added)
                {
                    int userValue = node.GetComponent<Node>().userValue;
                    userPlacedNodes.Remove(node.GetComponent<Node>().userValue);
                    RemoveNodeConnections(node, null);
                    node.GetComponent<Node>().userValue = -1;
                    SaveByIndex(node.GetComponent<Node>().value - 1);
                    GetComponent<Appearance>().SetNodeToEmptyLook(node);
                    if (action.removedOther)
                    {
                        GameObject otherNode = gameBoardPositions[action.otherPosition];
                        AddNode(otherNode, action.otherValue, false, action.otherHinted);
                    }
                    if (action.removedOtherOther)
                    {
                        GameObject otherNode = gameBoardPositions[action.otherOtherPosition];
                        AddNode(otherNode, action.otherOtherValue, false, action.otherOtherHinted);
                    }
                    GetComponent<NumberScroller>().HighlightNumber(userValue);
                }
                else
                {
                    AddNode(node, action.value, false, action.hinted);
                }
                RecheckNeighborConnections(node);
                GetComponent<SoundManager>().Undo();
            }
            undoButton.GetComponent<Button>().interactable &= GetComponent<UserAction>().HaveActions();
            GetComponent<HapticFeedback>().SelectTapticFeedback();
        }
    }

    public void Hint()
    {
        if (!won)
        {
            List<GameObject> wrongAnswers = new List<GameObject>();
            if (userPlacedNodes.Count == notPlacedNumbers.Count)
            {
                for (int i = 0; i < notPlacedNumbers.Count; i++)
                {
                    if (notPlacedNumbers[i].GetComponent<Node>().value != notPlacedNumbers[i].GetComponent<Node>().userValue)
                    {
                        wrongAnswers.Add(notPlacedNumbers[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < notPlacedNumbers.Count; i++)
                {
                    if (notPlacedNumbers[i].GetComponent<Node>().userValue == -1)
                    {
                        wrongAnswers.Add(notPlacedNumbers[i]);
                    }
                }
            }
            if (wrongAnswers.Count > 0)
            {
                GameObject randomNode = wrongAnswers[Random.Range(0, wrongAnswers.Count)];
                AddNode(randomNode, randomNode.GetComponent<Node>().value, true, 1);
                GetComponent<Appearance>().NodeFeedBack(randomNode.transform);
                GetComponent<HapticFeedback>().SuccessTapticFeedback();
                GetComponent<SoundManager>().PlayHintSound();
                RecheckNeighborConnections(randomNode);
                PlayerPrefs.SetInt(PlayerPrefsManager.currentHintCount, PlayerPrefs.GetInt(PlayerPrefsManager.currentHintCount, 0) + 1);
            }
        }
    }

    void RecheckNeighborConnections(GameObject node)
    {
        List<Vector2> neighbors = GetNeighboringPositions(node.GetComponent<Node>().position);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (gameBoardPositions.ContainsKey(neighbors[i]))
            {
                AddLineToNeighbors(neighbors[i], gameBoardPositions[neighbors[i]]);
            }
        }
    }

    /*
     * Restarting & Clearing
     */

    public void ClearBoard()
    {
        boxHolders.GetComponent<RectTransform>().pivot = Vector2.one / 2.0f;
        lineHolder.GetComponent<RectTransform>().pivot = Vector2.one / 2.0f;
        textHolder.GetComponent<RectTransform>().pivot = Vector2.one / 2.0f;
        holdDownHolder.GetComponent<RectTransform>().pivot = Vector2.one / 2.0f;
        GetComponent<Appearance>().highlightHolder.GetComponent<RectTransform>().pivot = Vector2.one / 2.0f;
        GetComponent<Appearance>().RemoveNodeHighlight();
        boxHolders.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        lineHolder.transform.position = boxHolders.transform.position;
        textHolder.transform.position = boxHolders.transform.position;
        holdDownHolder.transform.position = boxHolders.transform.position;
        boxHolders.transform.localScale = Vector3.one;
        lineHolder.transform.localScale = Vector3.one;
        textHolder.transform.localScale = Vector3.one;
        holdDownHolder.transform.localScale = Vector3.one;
        GetComponent<Appearance>().highlightHolder.transform.localScale = Vector3.one;
        for (int i = 0; i < gameBoardAnswer.Count; i++)
        {
            Destroy(gameBoardAnswer[i].GetComponent<Node>().text);
            if (gameBoardAnswer[i].GetComponent<Node>().line != null)
            {
                Destroy(gameBoardAnswer[i].GetComponent<Node>().line);
            }
            Destroy(gameBoardAnswer[i].gameObject);
        }
        gameBoardAnswer.Clear();
        gameBoardPositions.Clear();
        xAmounts.Clear();
        yAmounts.Clear();
        userPlacedNodes.Clear();
        notPlacedNumbers.Clear();
        lockedNodes.Clear();
        hintedNumbers.Clear();
        if (won)
        {
            won = false;
            if (Camera.main.GetComponent<GameWon>())
            {
                Camera.main.GetComponent<GameWon>().TurnOffWinScreen();
            }
            GetComponent<Appearance>().MakeNodeHighlightClear(true);
        }
        GetComponent<UserAction>().ClearActions();
        undoButton.GetComponent<Button>().interactable = false;
        GetComponent<Timer>().Restart();
    }

    public void Restart()
    {
        for (int i = 0; i < gameBoardAnswer.Count; i++)
        {
            if (!gameBoardAnswer[i].GetComponent<Node>().lockedValue && gameBoardAnswer[i].GetComponent<Node>().userValue > 0)
            {
                RemoveNodeConnections(gameBoardAnswer[i], null);
                gameBoardAnswer[i].GetComponent<Node>().userValue = -1;
                GetComponent<Appearance>().SetNodeToEmptyLook(gameBoardAnswer[i]);
            }
            if (i == 0 || i == gameBoardAnswer.Count - 1)
            {
                GetComponent<Appearance>().CreateStartEndCircle(gameBoardAnswer[i]);
            }
        }
        GetComponent<Appearance>().RemoveNodeHighlight();
        GetComponent<NumberScroller>().GoToFirstButton();
        userPlacedNodes.Clear();
        if (won)
        {
            won = false;
            if (Camera.main.GetComponent<GameWon>())
            {
                Camera.main.GetComponent<GameWon>().TurnOffWinScreen();
            }
            GetComponent<Appearance>().MakeNodeHighlightClear(false);
        }
        GetComponent<UserAction>().ClearActions();
        undoButton.GetComponent<Button>().interactable = false;
        Save();
    }

    /*
     * Save & Load
     */

    public void Save()
    {
        PlayerPrefs.SetString(PlayerPrefsManager.difficulty, difficulty.text);
        PlayerPrefs.SetInt(PlayerPrefsManager.boardSize, gameBoardAnswer.Count);
        if (diagonal)
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.diagonal, 1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.diagonal, 0);
        }
        for (int i = 0; i < gameBoardAnswer.Count; i++)
        {
            PlayerPrefs.SetInt(i + PlayerPrefsManager.xPos, (int)gameBoardAnswer[i].GetComponent<Node>().position.x);
            PlayerPrefs.SetInt(i + PlayerPrefsManager.yPos, (int)gameBoardAnswer[i].GetComponent<Node>().position.y);
            PlayerPrefs.SetInt(i + PlayerPrefsManager.value, gameBoardAnswer[i].GetComponent<Node>().value);
            PlayerPrefs.SetInt(i + PlayerPrefsManager.userValue, gameBoardAnswer[i].GetComponent<Node>().userValue);
            if (gameBoardAnswer[i].GetComponent<Node>().lockedValue)
            {
                PlayerPrefs.SetInt(i + PlayerPrefsManager.locked, 1);
            }
            else
            {
                PlayerPrefs.SetInt(i + PlayerPrefsManager.locked, 0);
            }
        }
        GetComponent<Timer>().SaveTime();
        PlayerPrefs.Save();
    }

    public void SaveByIndex(int index)
    {
        PlayerPrefs.SetInt(index + PlayerPrefsManager.hinted, gameBoardAnswer[index].GetComponent<Node>().hinted);
        PlayerPrefs.SetInt(index + PlayerPrefsManager.userValue, gameBoardAnswer[index].GetComponent<Node>().userValue);
    }

    public void Load()
    {
        difficulty.text = PlayerPrefs.GetString(PlayerPrefsManager.difficulty);
        amount = PlayerPrefs.GetInt(PlayerPrefsManager.boardSize);
        if (PlayerPrefs.GetInt(PlayerPrefsManager.diagonal) == 1)
        {
            diagonal = true;
        }
        for (int i = 0; i < amount; i++)
        {
            GameObject node = Instantiate(box, boxHolders.transform);
            Vector2 pos = new Vector2(
                PlayerPrefs.GetInt(i + PlayerPrefsManager.xPos),
                PlayerPrefs.GetInt(i + PlayerPrefsManager.yPos)
            );
            NodeComponent(node, pos, PlayerPrefs.GetInt(i + PlayerPrefsManager.value));
            node.GetComponent<Node>().userValue = PlayerPrefs.GetInt(i + PlayerPrefsManager.userValue);
            if (PlayerPrefs.GetInt(i + PlayerPrefsManager.locked) == 1)
            {
                node.GetComponent<Node>().userValue = -1;
                node.GetComponent<Node>().lockedValue = true;
                GetComponent<Appearance>().SetNodeToLockedLook(node);
                lockedNodes.Add(node.GetComponent<Node>().value, node);
                if (node.GetComponent<Node>().value == 1 || node.GetComponent<Node>().value == amount)
                {
                    GetComponent<Appearance>().CreateStartEndCircle(node);
                }
            }
            else
            {
                if (node.GetComponent<Node>().userValue > 0)
                {
                    userPlacedNodes.Add(PlayerPrefs.GetInt(i + PlayerPrefsManager.userValue), node);
                    node.GetComponent<Node>().userValue = PlayerPrefs.GetInt(i + PlayerPrefsManager.userValue);
                    node.GetComponent<Node>().hinted = PlayerPrefs.GetInt(i + PlayerPrefsManager.hinted);
                    GetComponent<Appearance>().SetNodeToUserPlacedLook(node);
                    if (node.GetComponent<Node>().userValue == 1 || node.GetComponent<Node>().userValue == amount)
                    {
                        GetComponent<Appearance>().CreateStartEndCircle(node);
                    }
                    if (node.GetComponent<Node>().hinted == 1)
                    {
                        hintedNumbers.Add(node.GetComponent<Node>().value, node);
                    }
                }
                else
                {
                    node.GetComponent<Node>().userValue = -1;
                    node.GetComponent<Node>().lockedValue = false;
                    GetComponent<Appearance>().SetNodeToEmptyLook(node);
                }
                notPlacedNumbers.Add(node);
            }
            gameBoardAnswer.Add(node);
            gameBoardPositions.Add(pos, node);
            AddValue(pos);
        }
        for (int i = 0; i < gameBoardAnswer.Count; i++)
        {
            AddLineToNeighbors(gameBoardAnswer[i].GetComponent<Node>().position, gameBoardAnswer[i]);
        }
        ShiftBoard();
        GetComponent<Timer>().LoadTime();
        if (CheckIfWon())
        {
            won = true;
            GetComponent<Timer>().GameFinished();
            GetComponent<Menus>().WinMenuOpen();
            GetComponent<CongratsScreenManager>().SetUpGameWonScreen(false);
            GetComponent<Appearance>().MakeNodeHighlightClear(true);
        }
    }

    /*
     * Getters
     */

    public int GetBoardNumberAmount()
    {
        return amount;
    }

    public Dictionary<int, GameObject> GetUserPlacedNodes()
    {
        return userPlacedNodes;
    }

    public Dictionary<int, GameObject> GetLockedPlacedNodes()
    {
        return lockedNodes;
    }

    public List<GameObject> GetNotPlacedNumbers()
    {
        return notPlacedNumbers;
    }

    public Dictionary<Vector2, GameObject> GetGameBoardPositions()
    {
        return gameBoardPositions;
    }

    public List<GameObject> GetGameBoard()
    {
        return gameBoardAnswer;
    }

    public Dictionary<int, GameObject> GetHintedNumbers()
    {
        return hintedNumbers;
    }

    public bool GetWinStatus()
    {
        return won;
    }

    public bool GetDiagonal()
    {
        return diagonal;
    }
}
