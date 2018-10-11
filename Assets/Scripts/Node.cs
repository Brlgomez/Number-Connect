using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool lockedValue;
    public int value;
    public int userValue;
    public int hinted;
    public Vector2 position;
    public GameObject previousNode;
    public GameObject nextNode;
    public GameObject line;
    public GameObject text;

    public void Awake()
    {
        userValue = -1;
    }

    public void PressedNode()
    {
        Camera.main.GetComponent<BoardCreator>().PressedNode(gameObject);
    }
}
