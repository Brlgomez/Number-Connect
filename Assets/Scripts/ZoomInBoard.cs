using UnityEngine;

public class ZoomInBoard : MonoBehaviour
{
    public GameObject boxHolder, lineHolder, textHolder, highlightHolder;

    public void Zoom(int width, int height)
    {
        Vector3 pos = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width / 2, Screen.height / 2));

        boxHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        lineHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        textHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        highlightHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

        float xShift = (boxHolder.transform.position.x - (Screen.width / 2)) / (Screen.width);
        float yShift = (boxHolder.transform.position.y - (Screen.height / 2)) / (Screen.height);

        Vector2 newPivot = new Vector2(0.5f - xShift, 0.5f - yShift);

        boxHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
        lineHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
        textHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
        highlightHolder.transform.GetComponent<RectTransform>().pivot = newPivot;

        float boardHeight = ((float)Screen.height / Screen.width) - (1.0f / 3.0f);
        if (((float)height / width) >= boardHeight)
        {
            boxHolder.transform.localScale = Vector3.one * 13 / height;
            lineHolder.transform.localScale = Vector3.one * 13 / height;
            textHolder.transform.localScale = Vector3.one * 13 / height;
            highlightHolder.transform.localScale = Vector3.one * 13 / height;
        }
        else
        {
            boxHolder.transform.localScale = Vector3.one * 14 / width;
            lineHolder.transform.localScale = Vector3.one * 14 / width;
            textHolder.transform.localScale = Vector3.one * 14 / width;
            highlightHolder.transform.localScale = Vector3.one * 14 / width;
        }
    }
}
