using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SkinData;

public class FlexibleUISlider : FlexibleUI
{
    [SerializeField, Header("Index in skin data's slider list")]
    protected int _sliderIndex;
    private SliderData _data;
    private Image _fill;
    protected Slider _slider;
    // Start is called before the first frame update
    protected override void Reset()
    {
        base.Reset();
        _slider = GetComponent<Slider>();
        _fill = _slider.fillRect.GetComponent<Image>();
        SliderData[] sliders = Skin.sliders;
        _data = sliders[Mathf.Min(_sliderIndex, sliders.Length - 1)];
        _slider.colors = _data.handleColors.colors;
        BackGroundDataToUse = _data.background;
        IconDataToUse = _data.handle;
    }
    protected override void SkinBackground()
    {
        base.SkinBackground();
        SpriteData fillArea = _data.fillArea;
        Debug.Log((fillArea == null) + ";" + (_fill == null));
        _fill.sprite = fillArea.sprite;
        _fill.color = fillArea.color;
    }

}
