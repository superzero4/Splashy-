using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SkinData;
[Obsolete("Do not use this class, rather use FlexibleUISelectable class and select Toggle in type",true)]
public class FlexibleUIToggle : FlexibleUIAbstractSelectable
{
    protected override ButtonData findSprites()
    {
        return null;
        //return Skin.SpritesForToggle;
    }
}
