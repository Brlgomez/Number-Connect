using UnityEngine;
using UnityEngine.UI;

public class MenuTransitionOff : MonoBehaviour
{
    static float maxTime = 0.5f;
    static float speed = 1.25f;
    static float backgroundSpeed = 2.56f;
    GameObject holder;
    Vector3 originalPosition;
    Vector3 newPosition;
    float time;
    Color originalColor;
    Color newColor;
    float alpha;

    void Start()
    {
        holder = gameObject.transform.GetChild(0).gameObject;
        originalPosition = gameObject.transform.position;
        newPosition = new Vector3(holder.transform.position.x, -Screen.height, holder.transform.position.z);
        originalColor = GetComponent<Image>().color;
        newColor = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0);
        alpha = originalColor.a;
    }

    void Update()
    {
        time += Time.deltaTime * speed;
        alpha -= Time.deltaTime * backgroundSpeed;
        holder.transform.position = Vector3.Lerp(holder.transform.position, newPosition, time);
        if (alpha > newColor.a)
        {
            GetComponent<Image>().color = new Color(newColor.r, newColor.g, newColor.b, alpha);
        }
        if (time > maxTime)
        {
            holder.transform.position = originalPosition;
            GetComponent<Image>().color = originalColor;
            gameObject.SetActive(false);
            Destroy(GetComponent<MenuTransitionOff>());
        }
    }
}
