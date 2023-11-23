using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpciones2 : MonoBehaviour
{

    public UIBlock Root = null;

    [Header("Temporary")]
    public BoolSettings BoolSettings = new BoolSettings();
    public ItemView ToggleItemView = null;
    public FloatSetting FloatSetting = new FloatSetting();
    public ItemView SliderItemView = null;

    private void Start()
    {
        //Genera el metodo de cambio de color de los botones
        Root.AddGestureHandler<Gesture.OnHover, CambioVisualOpciones>(CambioVisualOpciones.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, CambioVisualOpciones>(CambioVisualOpciones.HandleUnHover);
        Root.AddGestureHandler<Gesture.OnPress, CambioVisualOpciones>(CambioVisualOpciones.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, CambioVisualOpciones>(CambioVisualOpciones.HandleRelease);

        Root.AddGestureHandler<Gesture.OnClick, CambioVisualOpciones>(HandleToggleClicked);
        Root.AddGestureHandler<Gesture.OnDrag, VisualSlider>(HandleSliderDragged);

        BindToggle(BoolSettings, ToggleItemView.Visuals as CambioVisualOpciones);
        BindSlider(FloatSetting, SliderItemView.Visuals as VisualSlider);
    }

    private void HandleSliderDragged(Gesture.OnDrag evt, VisualSlider target)
    {
        Vector3 currentPointerPos = evt.PointerPositions.Current;

        float localXpos = target.SliderBackGround.transform.InverseTransformPoint(currentPointerPos).x;
        float sliderWidth = target.SliderBackGround.CalculatedSize.X.Value;

        float distanceFromLeft = localXpos + .5f * sliderWidth;
        float percenteFromLeft = Mathf.Clamp01(distanceFromLeft / sliderWidth);

        FloatSetting.Value = FloatSetting.min + percenteFromLeft * (FloatSetting.max - FloatSetting.min);

        target.FillBar.Size.X.Percent = percenteFromLeft;
        target.ValueLable.Text = FloatSetting.DisplayValue;
    }

    private void HandleToggleClicked(Gesture.OnClick evt, CambioVisualOpciones target)
    {
        BoolSettings.State = !BoolSettings.State;
        target.IsChecked = BoolSettings.State;
    }

    private void BindSlider(FloatSetting floatSetting, VisualSlider slider)
    {
        slider.ValueLable.Text = floatSetting.DisplayValue;
        slider.FillBar.Size.X.Percent = (floatSetting.Value - floatSetting.min) / (floatSetting.max - floatSetting.min);
    }

    private void BindToggle(BoolSettings boolSettings, CambioVisualOpciones Visualess)
    {
        Visualess.IsChecked = boolSettings.State;
    }
}
