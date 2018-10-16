using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchAndZoom : MonoBehaviour
{
    static float speed = 0.005f;
    static int minSize = 1;
    static float maxSize = 2f;
    public GameObject boxHolder, lineHolder, textHolder, highlightHolder;
    public bool zooming;
    bool gotPivot;
    Vector2 currentPosition, lastPosition;
    float deltaPosition;
    Vector2 positionDelta;

    void Update()
    {
        /*
        if (Input.touchCount == 1)
        {
            Touch touchZero = Input.GetTouch(0);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            float delta = touchZero.deltaTime;
            if (delta > 0.01f)
            {
                Vector2 direction = touchZero.position - touchZeroPrevPos;
                Vector3 newPos = new Vector3(
                    boxHolder.transform.localPosition.x + direction.x,
                    boxHolder.transform.localPosition.y + direction.y,
                    0
                );

                
                positionDelta = new Vector3(positionDelta.x + direction.x, positionDelta.y + direction.y);
                if (positionDelta.x > ((boxHolder.transform.localScale.x - 1) * 500) || positionDelta.x < -((boxHolder.transform.localScale.x - 1) * 500))
                {
                    newPos = new Vector3(newPos.x - direction.x, newPos.y, 0);
                    positionDelta = new Vector2(positionDelta.x - direction.x, positionDelta.y);
                }

                if (positionDelta.y > ((boxHolder.transform.localScale.x - 1) * 500) || positionDelta.y < -((boxHolder.transform.localScale.x - 1) * 500))
                {
                    newPos = new Vector3(newPos.x, newPos.y - direction.y, 0);
                    positionDelta = new Vector2(positionDelta.x, positionDelta.y - direction.y);
                }


                boxHolder.transform.localPosition = newPos;
                lineHolder.transform.localPosition = newPos;
                textHolder.transform.localPosition = newPos;
                highlightHolder.transform.localPosition = newPos;
            }
        }
        */
        if (Input.touchCount == 2)
        {
            zooming = true;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Vector3 sizeDelta = Vector3.one * deltaMagnitudeDiff * speed;

            boxHolder.transform.localScale -= sizeDelta;
            lineHolder.transform.localScale -= sizeDelta;
            textHolder.transform.localScale -= sizeDelta;
            highlightHolder.transform.localScale -= sizeDelta;

            if (!gotPivot)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(touchZero.position) +
                                    ((Camera.main.ScreenToViewportPoint(touchZero.position) - (Vector3.one / 2)) * 1.25f);
                Vector3 pos2 = Camera.main.ScreenToViewportPoint(touchOne.position) +
                                     ((Camera.main.ScreenToViewportPoint(touchOne.position) - (Vector3.one / 2)) * 1.25f);
                Vector2 middle = (pos + pos2) / 2;

                boxHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                lineHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                textHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                highlightHolder.transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

                float xShift = (boxHolder.transform.position.x - (Screen.width / 2)) / (Screen.width);
                float yShift = (boxHolder.transform.position.y - (Screen.height / 2)) / (Screen.height);
                Vector2 newPivot = new Vector2((middle.x - xShift), (middle.y - yShift));

                boxHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
                lineHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
                textHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
                highlightHolder.transform.GetComponent<RectTransform>().pivot = newPivot;
            }

            if (boxHolder.transform.localScale.x > maxSize)
            {
                boxHolder.transform.localScale = Vector3.one * maxSize;
                lineHolder.transform.localScale = Vector3.one * maxSize;
                textHolder.transform.localScale = Vector3.one * maxSize;
                highlightHolder.transform.localScale = Vector3.one * maxSize;
            }
            if (boxHolder.transform.localScale.x < minSize)
            {
                boxHolder.transform.localScale = Vector3.one * minSize;
                lineHolder.transform.localScale = Vector3.one * minSize;
                textHolder.transform.localScale = Vector3.one * minSize;
                highlightHolder.transform.localScale = Vector3.one * minSize;
            }
        }
        else
        {
            if (zooming)
            {
                zooming = false;
                gotPivot = false;
            }
        }
    }
}
