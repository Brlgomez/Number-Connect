using UnityEngine;
using UnityEngine.UI;

public class SliderMovement : MonoBehaviour
{
    static int speed = 5;
    int goTowards;
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (goTowards == 0)
        {
            slider.value -= Time.smoothDeltaTime * speed;
            if (slider.value <= goTowards)
            {
                slider.value = goTowards;
                Camera.main.GetComponent<HapticFeedback>().LightTapticFeedback();
                Destroy(GetComponent<SliderMovement>());
            }
        }
        else
        {
            slider.value += Time.smoothDeltaTime * speed;
            if (slider.value >= goTowards)
            {
                slider.value = goTowards;
                Camera.main.GetComponent<HapticFeedback>().LightTapticFeedback();
                Destroy(GetComponent<SliderMovement>());
            }
        }
    }

    public void SetGoTowards(int t)
    {
        goTowards = t;
    }
}
