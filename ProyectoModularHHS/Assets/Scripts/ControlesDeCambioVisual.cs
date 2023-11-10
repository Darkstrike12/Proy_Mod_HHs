using Nova;
using NovaSamples.UIControls;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ControlesDeCambioVisual : ItemVisuals
{
    public TextBlock Label = null;
    public UIBlock2D Checkbox = null;
    public UIBlock2D CheckMark = null;

    public Color DefaultColor;
    public Color HoveredColor;
    public Color UnHoveredColor;
    public Color PressedColor;

    public bool IsChecked
    {
        get => CheckMark.gameObject.activeSelf;
        set => CheckMark.gameObject.SetActive(value);
    }

    internal static void HandleHover(Gesture.OnHover evt, ControlesDeCambioVisual target)
    {
        target.Checkbox.Color = target.HoveredColor;
    }

    internal static void HandlePress(Gesture.OnPress evt, ControlesDeCambioVisual target)
    {
        target.Checkbox.Color = target.PressedColor;

    }
    internal static void HandleRelease(Gesture.OnRelease evt, ControlesDeCambioVisual target)
    {
        target.Checkbox.Color = target.DefaultColor;
    }

    internal static void HandleUnHovered(Gesture.OnUnhover evt, ControlesDeCambioVisual target)
    {
        target.Checkbox.Color = target.UnHoveredColor;
    }
}
