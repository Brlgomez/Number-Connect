using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    static float rotationSpeed = -25;
    static float scaleSpeed = 0.25f;
    static float maxScaleSize = 1.20f;
    static float minScaleSize = 0.80f;
    static float transitionSpeed = 1f;
    static float timeAfterTurningOff = 1f;
    bool turningOn, turningOff;
    Color shineColor, highlightColor, backgroundColor, textColor, panelColor;
    GameObject shine, particles;
    Image topPanel, bottomPanel, buttonImage;
    Text congratsText, statsText, newGameText;
    ParticleSystem ps;
    float transition, timeForDestroying;

    public void SetUp(GameObject shineImage, GameObject particlesObject, Image top, Image bottom,
                      Text congrats, Text stats, Image button, Text buttonText, string statsString)
    {
        backgroundColor = Camera.main.GetComponent<Appearance>().CurrentTheme().backgroundColor;
        highlightColor = Camera.main.GetComponent<Appearance>().CurrentTheme().highlightColor;
        panelColor = Camera.main.GetComponent<Appearance>().CurrentTheme().panelColor;
        shineColor = Camera.main.GetComponent<Appearance>().CurrentTheme().lockedNodeColor;
        textColor = Camera.main.GetComponent<Appearance>().CurrentTheme().menuButtonTextColor;

        topPanel = top;
        bottomPanel = bottom;
        buttonImage = button;
        congratsText = congrats;
        statsText = stats;
        newGameText = buttonText;
        shine = shineImage;
        particles = particlesObject;

        ps = particles.GetComponent<ParticleSystem>();
        ps.Play();
        var main = ps.main;
        main.startColor = shineColor;
        shine.SetActive(true);

        shine.GetComponent<Image>().color = GetClearOfColor(shine.GetComponent<Image>().color);
        topPanel.color = GetClearOfColor(topPanel.color);
        bottomPanel.color = GetClearOfColor(bottomPanel.color);
        buttonImage.color = GetClearOfColor(buttonImage.color);
        congratsText.color = GetClearOfColor(congratsText.color);
        statsText.color = GetClearOfColor(statsText.color);
        newGameText.color = GetClearOfColor(newGameText.color);
        statsText.text = statsString;
        turningOn = true;
    }

    void Update()
    {
        transition += Time.deltaTime * transitionSpeed;
        shine.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * rotationSpeed);
        shine.transform.localScale = Vector3.one * Mathf.SmoothStep(minScaleSize, maxScaleSize, Mathf.PingPong(Time.time * scaleSpeed, 1));
        if (turningOn)
        {
            Camera.main.backgroundColor = Color.Lerp(backgroundColor, highlightColor, transition);
            shine.GetComponent<Image>().color = Color.Lerp(GetClearOfColor(shine.GetComponent<Image>().color), shineColor, transition);
            topPanel.color = Color.Lerp(GetClearOfColor(topPanel.color), panelColor, transition);
            bottomPanel.color = Color.Lerp(GetClearOfColor(bottomPanel.color), panelColor, transition);
            buttonImage.color = Color.Lerp(GetClearOfColor(buttonImage.color), panelColor, transition);
            congratsText.color = Color.Lerp(GetClearOfColor(congratsText.color), textColor, transition);
            statsText.color = Color.Lerp(GetClearOfColor(statsText.color), textColor, transition);
            newGameText.color = Color.Lerp(GetClearOfColor(newGameText.color), textColor, transition);
        }
        if (turningOff)
        {
            timeForDestroying += Time.deltaTime;
            Camera.main.backgroundColor = Color.Lerp(highlightColor, backgroundColor, transition);
            shine.GetComponent<Image>().color = Color.Lerp(shineColor, GetClearOfColor(shine.GetComponent<Image>().color), transition);
            if (timeForDestroying > timeAfterTurningOff)
            {
                shine.SetActive(false);
                particles.GetComponent<ParticleSystem>().Stop();
                Camera.main.backgroundColor = backgroundColor;
                Destroy(gameObject.GetComponent<GameWon>());
            }
        }
    }

    public void TurnOffWinScreen()
    {
        turningOn = false;
        turningOff = true;
        transition = 0;
    }

    public void ChangedTheme()
    {
        backgroundColor = Camera.main.GetComponent<Appearance>().CurrentTheme().backgroundColor;
        highlightColor = Camera.main.GetComponent<Appearance>().CurrentTheme().highlightColor;
        panelColor = Camera.main.GetComponent<Appearance>().CurrentTheme().panelColor;
        shineColor = Camera.main.GetComponent<Appearance>().CurrentTheme().lockedNodeColor;
        textColor = Camera.main.GetComponent<Appearance>().CurrentTheme().menuButtonTextColor;
        Camera.main.backgroundColor = highlightColor;
        shine.GetComponent<Image>().color = shineColor;
        var main = ps.main;
        main.startColor = shineColor;
    }

    Color GetClearOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 0);
    }
}
