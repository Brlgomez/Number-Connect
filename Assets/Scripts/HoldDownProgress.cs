using UnityEngine;

public class HoldDownProgress : MonoBehaviour
{
    static float maxTime = 0.3f;
    Vector2 maxSize = new Vector2(100, 10);
    Vector2 originalSize = new Vector2(10, 10);
    float time;
    float speed;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(10, 10);
        speed = 1.25f;
    }

    void Update()
    {
        time += Time.smoothDeltaTime * speed;
        rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, maxSize, time);
        if (time > maxTime)
        {
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, originalSize, time * 3);
        }
        if (time > (maxTime + 0.1f))
        {
            Destroy(GetComponent<HoldDownProgress>());
        }
    }
}
