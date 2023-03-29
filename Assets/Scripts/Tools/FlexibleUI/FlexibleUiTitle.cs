using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkinData;

public class FlexibleUiTitle : FlexibleUI
{
    [SerializeField,Tooltip("Lowest has highest priority, if the level doesn't exist, the closesst level will be chosed")]
    private int _titleLevel;
    private TitleData _data;
    // Start is called before the first frame update
    protected override void Reset()
    {
        base.Reset();
        TitleData[] levelOfTitles = Skin.LevelOfTitles;
        _data = levelOfTitles[Mathf.Min(_titleLevel,levelOfTitles.Length-1)];
        BackGroundDataToUse = _data.underLine;
        FontDataToUse = _data.font;
    }
    protected override void SkinBackground()
    {
        base.SkinBackground();
    }
    protected override void SkinText()
    {
        base.SkinText();
        //Debug.Log(_fontDataToUse.colorOfText);
        if (_data.sizeOfFont > 0)
        {
            _text.fontSize = _data.sizeOfFont;
        }
    }

}
