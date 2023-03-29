using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSetter : FlexibleUI
{
    private Camera _cam;
    private new void Reset()
    {
        if (_cam == null)
        {
            _cam = GetComponent<Camera>();
        }
    }
    protected override void SkinUI()
    {
        //Debug.Log(Skin == null);
        if (_cam == null)
        {
            _cam = GetComponent<Camera>();
        }
        try
        {
            _cam.backgroundColor = Skin.SceneBackground;
        }
        catch (Exception e)
        {
            Debug.LogWarning("An error appearing here means there's another error in a flexible UI elements that occured before this; "+e.ToString());
        }
    }
}
