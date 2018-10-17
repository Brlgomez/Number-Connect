using UnityEngine;
using UnityEngine.UI;

public class MenuTransitionOn : MonoBehaviour
{
    static float maxTime = 0.5f;
    static float speed = 1.25f;
    static float backgroundSpeed = 2.56f;
    GameObject holder;
    Vector3 originalPosition;
    float time;
    Color originalColor;
    float alpha;

    void Start()
    {
        gameObject.SetActive(true);
        holder = gameObject.transform.GetChild(0).gameObject;
        originalPosition = holder.transform.position;
        holder.transform.position = new Vector3(originalPosition.x, -Screen.height, originalPosition.z);
        originalColor = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    void Update()
    {
        time += Time.smoothDeltaTime * speed;
        alpha += Time.smoothDeltaTime * backgroundSpeed;
        holder.transform.position = Vector3.Lerp(holder.transform.position, originalPosition, time);
        if (alpha < originalColor.a)
        {
            GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
        if (time > maxTime)
        {
            holder.transform.position = originalPosition;
            GetComponent<Image>().color = originalColor;
            Destroy(GetComponent<MenuTransitionOn>());
        }
    }
}
