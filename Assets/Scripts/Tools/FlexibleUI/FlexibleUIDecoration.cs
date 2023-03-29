using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleUIDecoration : FlexibleUIPanel
{
    protected override void SkinBackground()
    {
        _backGround.color = BackGroundDataToUse.color;
    }
}
