using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Themes : MonoBehaviour
{
    public enum ThemeName { light, dark, paper };

    public static Theme lightTheme = new Theme
    {
        nameOfTheme = ThemeName.light,
        backgroundColor = new Color(0.9f, 0.9f, 0.9f),
        panelColor = new Color(1, 1, 1),

        generalButtonColor = new Color(0, 0, 0),
        menuButtonColor = new Color(1, 1, 1),
        highlightColor = new Color(1, 0.5f, 0),

        lockedNodeColor = new Color(1, 1, 1),
        userPlacedNodeColor = new Color(1, 1, 1),
        emptyNodeColor = new Color(0.75f, 0.75f, 0.75f),
        lockedNodeTextColor = new Color(0.1f, 0.25f, 0.75f),
        userPlacedNodeTextColor = new Color(0, 0, 0),
        hintedNodeTextColor = new Color(0.2f, 0.5f, 0.2f)
    };

    public static Theme darkTheme = new Theme
    {
        nameOfTheme = ThemeName.dark,
        backgroundColor = new Color(0.125f, 0.125f, 0.125f),
        panelColor = new Color(0.1f, 0.1f, 0.1f),

        generalButtonColor = new Color(0.9f, 0.9f, 0.9f),
        menuButtonColor = new Color(0.1f, 0.1f, 0.1f),
        highlightColor = new Color(0.9f, 0.55f, 0.5f),

        lockedNodeColor = new Color(0.4f, 0.4f, 0.4f),
        userPlacedNodeColor = new Color(0.4f, 0.4f, 0.4f),
        emptyNodeColor = new Color(0.2f, 0.2f, 0.2f),
        lockedNodeTextColor = new Color(0.2f, 0.75f, 0.9f),
        userPlacedNodeTextColor = new Color(0.9f, 0.9f, 0.9f),
        hintedNodeTextColor = new Color(0.3f, 0.8f, 0.3f)
    };
    public static Theme paperTheme = new Theme
    {
        nameOfTheme = ThemeName.paper,
        backgroundColor = new Color(0.86f, 0.84f, 0.79f),
        panelColor = new Color(0.97f, 0.94f, 0.89f),

        generalButtonColor = new Color(0.3f, 0.2f, 0.12f),
        menuButtonColor = new Color(0.97f, 0.94f, 0.89f),
        highlightColor = new Color(0.47f, 0.67f, 0.76f),

        lockedNodeColor = new Color(0.97f, 0.94f, 0.89f),
        userPlacedNodeColor = new Color(0.97f, 0.94f, 0.89f),
        emptyNodeColor = new Color(0.74f, 0.71f, 0.65f),
        lockedNodeTextColor = new Color(0.9f, 0.25f, 0),
        userPlacedNodeTextColor = new Color(0.3f, 0.2f, 0.12f),
        hintedNodeTextColor = new Color(1, 0.61f, 0)
    };

    public class Theme
    {
        public ThemeName nameOfTheme;
        public Color backgroundColor;
        public Color panelColor;
        public Color highlightColor;
        public Color generalButtonColor;
        public Color menuButtonColor;
        public Color lockedNodeColor;
        public Color userPlacedNodeColor;
        public Color emptyNodeColor;
        public Color lockedNodeTextColor;
        public Color userPlacedNodeTextColor;
        public Color hintedNodeTextColor;
    }
}
