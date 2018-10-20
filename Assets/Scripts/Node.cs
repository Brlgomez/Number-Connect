using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
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

    static float holdTime = 0.4f;
    static float objectHoldTime = 0.1f;
    bool held, exited, movedObj;
    public UnityEvent onLongPress = new UnityEvent();
    public UnityEvent showHoldObject = new UnityEvent();

    public void Awake()
    {
        userValue = -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        held = false;
        exited = false;
        movedObj = false;
        Invoke("OnLongPress", holdTime);
        Invoke("ShowHoldObject", objectHoldTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
        CancelInvoke("ShowHoldObject");
        if (!exited && !held)
        {
            Camera.main.GetComponent<BoardCreator>().PressedNode(gameObject);
        }
        if (movedObj)
        {
            Camera.main.GetComponent<Appearance>().MoveHoldObjectOutOfScene();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exited = true;
        CancelInvoke("OnLongPress");
        CancelInvoke("ShowHoldObject");
        if (movedObj)
        {
            Camera.main.GetComponent<Appearance>().MoveHoldObjectOutOfScene();
        }
    }

    void OnLongPress()
    {
        held = true;
        onLongPress.Invoke();
        if (lockedValue)
        {
            Camera.main.GetComponent<HapticFeedback>().MediumTapticFeedback();
            Camera.main.GetComponent<NumberScroller>().GoToNearbyNumberByNodeValue(value);
            //Camera.main.GetComponent<Appearance>().NodeFeedBack(transform);
            Camera.main.GetComponent<SoundManager>().PlayScrollSound();
        }
        else if (!lockedValue && userValue > 0)
        {
            Camera.main.GetComponent<HapticFeedback>().MediumTapticFeedback();
            Camera.main.GetComponent<NumberScroller>().GoToNearbyNumberByNodeValue(userValue);
            //Camera.main.GetComponent<Appearance>().NodeFeedBack(transform);
            Camera.main.GetComponent<SoundManager>().PlayScrollSound();
        }
        else
        {
            Camera.main.GetComponent<HapticFeedback>().ErrorTapticFeedback();
        }
    }

    void ShowHoldObject()
    {
        movedObj = true;
        showHoldObject.Invoke();
        if (lockedValue)
        {
            Camera.main.GetComponent<Appearance>().MoveHoldObjectToCell(gameObject, value, text.GetComponent<Text>().color);
        }
        else if (!lockedValue && userValue > 0)
        {
            Camera.main.GetComponent<Appearance>().MoveHoldObjectToCell(gameObject, userValue, text.GetComponent<Text>().color);
        }
    }
}
