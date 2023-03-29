using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIEnums;
using UnityEngine.UI;
using static SkinData;

[RequireComponent(typeof(Selectable))]
public class FlexibleUISelectable : FlexibleUIAbstractSelectable
{
    [SerializeField] private TypeOfButton _typeOfButton;

    public TypeOfButton TypeOfButton { get => _typeOfButton; set => _typeOfButton = value; }

    protected override ButtonData findSprites()
    {
        try
        {
            return Skin.SpritesOfInteractables[_typeOfButton];
        }catch(KeyNotFoundException e)
        {
            Debug.LogWarning("Key "+_typeOfButton+" was not present in skin's dictionnary but was used on "+name+" gameobject, using the first element of the dictionnary instead");
            return Skin.SpritesOfInteractables[new List<TypeOfButton>(Skin.SpritesOfInteractables.Keys)[0]];
        }
    }
}
