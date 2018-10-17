using UnityEngine;

public class HapticFeedback : MonoBehaviour
{
    public void SelectTapticFeedback()
    {
        if (TapticPlugin.TapticManager.IsSupport())
        {
            TapticPlugin.TapticManager.Selection();
        }
    }

    public void LightTapticFeedback()
    {
        if (TapticPlugin.TapticManager.IsSupport())
        {
            TapticPlugin.TapticManager.Impact(TapticPlugin.ImpactFeedback.Light);
        }
    }

    public void MediumTapticFeedback()
    {
        if (TapticPlugin.TapticManager.IsSupport())
        {
            TapticPlugin.TapticManager.Impact(TapticPlugin.ImpactFeedback.Midium);
        }
    }

    public void HeavyTapticFeedback()
    {
        if (TapticPlugin.TapticManager.IsSupport())
        {
            TapticPlugin.TapticManager.Impact(TapticPlugin.ImpactFeedback.Heavy);
        }
    }

    public void SuccessTapticFeedback()
    {
        if (TapticPlugin.TapticManager.IsSupport())
        {
            TapticPlugin.TapticManager.Notification(TapticPlugin.NotificationFeedback.Success);
        }
    }

    public void WarningTapticFeedback()
    {
        if (TapticPlugin.TapticManager.IsSupport())
        {
            TapticPlugin.TapticManager.Notification(TapticPlugin.NotificationFeedback.Warning);
        }
    }

    public void ErrorTapticFeedback()
    {
        if (TapticPlugin.TapticManager.IsSupport())
        {
            TapticPlugin.TapticManager.Notification(TapticPlugin.NotificationFeedback.Error);
        }
    }
}
