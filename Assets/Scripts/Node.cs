using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    static float holdTime = 0.5f;
    bool held, exited;
    public UnityEvent onLongPress = new UnityEvent();

    public void Awake()
    {
        userValue = -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        held = false;
        exited = false;
        Invoke("OnLongPress", holdTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
        if (!exited && !held)
        {
            Camera.main.GetComponent<BoardCreator>().PressedNode(gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exited = true;
        CancelInvoke("OnLongPress");
    }

    void OnLongPress()
    {
        held = true;
        onLongPress.Invoke();
        if (lockedValue)
        {
            Camera.main.GetComponent<HapticFeedback>().MediumTapticFeedback();
            Camera.main.GetComponent<NumberScroller>().GoToNearbyNumberByNodeValue(value);
            Camera.main.GetComponent<Appearance>().NodeFeedBack(transform);
            Camera.main.GetComponent<SoundManager>().PlayScrollSound();
        }
        else if (!lockedValue && userValue > 0)
        {
            Camera.main.GetComponent<HapticFeedback>().MediumTapticFeedback();
            Camera.main.GetComponent<NumberScroller>().GoToNearbyNumberByNodeValue(userValue);
            Camera.main.GetComponent<Appearance>().NodeFeedBack(transform);
            Camera.main.GetComponent<SoundManager>().PlayScrollSound();
        }
        else
        {
            Camera.main.GetComponent<HapticFeedback>().ErrorTapticFeedback();
        }
    }
}
