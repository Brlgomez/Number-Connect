using UnityEngine;

public class ZoomInBoard : MonoBehaviour
{
    public GameObject boxHolder, lineHolder, textHolder, highlightHolder;

    public void Zoom(int width, int height)
    {
        boxHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        lineHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        textHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        highlightHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

        float xShift = (boxHolder.transform.position.x - (Screen.width / 2)) / (Screen.width);
        float yShift = (boxHolder.transform.position.y - (Screen.height / 2)) / (Screen.height);

        Vector2 newPivot = new Vector2(0.5f - xShift, 0.55f - yShift);

        boxHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
        lineHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
        textHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
        highlightHolder.transform.GetComponent<RectTransform>().pivot = newPivot;

        float boardHeight = (Screen.height * 0.62f) / Screen.width;
        float maxHeight = boardHeight * 16;

        if (((float)height / width) >= boardHeight)
        {
            boxHolder.transform.localScale = Vector3.one * maxHeight / height;
            lineHolder.transform.localScale = Vector3.one * maxHeight / height;
            textHolder.transform.localScale = Vector3.one * maxHeight / height;
            highlightHolder.transform.localScale = Vector3.one * maxHeight / height;
        }
        else
        {
            boxHolder.transform.localScale = Vector3.one * 15 / width;
            lineHolder.transform.localScale = Vector3.one * 15 / width;
            textHolder.transform.localScale = Vector3.one * 15 / width;
            highlightHolder.transform.localScale = Vector3.one * 15 / width;
        }
    }
}
