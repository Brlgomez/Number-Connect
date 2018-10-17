using UnityEngine;
using UnityEngine.UI;

public class NodeFeedback : MonoBehaviour
{
    static float speed = 1.5f;
    float alpha = 1;
    Color initialColor;

    void Start()
    {
        initialColor = Camera.main.GetComponent<Appearance>().CurrentTheme().highlightColor;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        initialColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
        GetComponent<Image>().color = initialColor;
        transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one * 3, deltaTime);
        alpha -= deltaTime * speed;
        if (alpha < 0)
        {
            Destroy(gameObject);
        }
    }
}
