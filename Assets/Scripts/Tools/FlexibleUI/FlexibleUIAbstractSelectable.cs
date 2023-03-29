using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SkinData;

[RequireComponent(typeof(Selectable))]
public abstract class FlexibleUIAbstractSelectable : FlexibleUI
{
    private Selectable _selectable;
    protected override void Reset()
    {
        base.Reset();
        SkinData.ButtonData buttonData = findSprites();
        IconDataToUse = buttonData.icon;
        BackGroundDataToUse = buttonData.backgrond;
        _selectable = GetComponent<Selectable>();
        _selectable.transition = buttonData.transitionType;
        _selectable.spriteState = buttonData.transitions.transitions;
        _selectable.colors = buttonData.colors.colors;
    }
    protected abstract ButtonData findSprites();

}
