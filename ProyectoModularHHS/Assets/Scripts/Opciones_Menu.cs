using Nova;
using NovaSamples.UIControls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opciones_Menu : MonoBehaviour
{
    public UIBlock Root = null;

    [Header("Temporal")]
    public BoolSetting BoolSetting = new BoolSetting();
    public ItemView ToggleItemView = null;


    private void Start()
    {
        //Visual
        Root.AddGestureHandler<Gesture.OnHover, ControlesDeCambioVisual>(ControlesDeCambioVisual.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, ControlesDeCambioVisual>(ControlesDeCambioVisual.HandleUnHovered);
        Root.AddGestureHandler<Gesture.OnPress, ControlesDeCambioVisual>(ControlesDeCambioVisual.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, ControlesDeCambioVisual>(ControlesDeCambioVisual.HandleRelease);

        //Cmabio de estado del bool
        Root.AddGestureHandler<Gesture.OnClick, ControlesDeCambioVisual>(HandleToggleClicked);

        //Temporal
        BindToggle(BoolSetting, ToggleItemView.Visuals as ControlesDeCambioVisual);
    }

    private void HandleToggleClicked(Gesture.OnClick evt, ControlesDeCambioVisual target)
    {
        BoolSetting.State = !BoolSetting.State;
        target.IsChecked = BoolSetting.State;
    }

    private void BindToggle(BoolSetting boolSetting, ControlesDeCambioVisual visuals)
    {
        visuals.Label.Text = boolSetting.Name;
        visuals.IsChecked = boolSetting.State;
    }
}
