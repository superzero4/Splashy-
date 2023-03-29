using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIEnums
{
    [Flags]
    public enum UIContains
    {
        Text=1,
        Background=2,
        Icon=4
    }
    public enum TypeOfButton
    {
        Confirm,
        Cancel,
        MenuItem,
        Toggle,
        ToggleInGroup
    }
}
