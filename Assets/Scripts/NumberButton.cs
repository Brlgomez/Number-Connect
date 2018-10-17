using UnityEngine;

public class NumberButton : MonoBehaviour
{
    public int value;
    public int index;

    public void PressedNumber()
    {
        Camera.main.GetComponent<NumberScroller>().ChangeHighlightedNumber(transform.gameObject, true);
    }
}
