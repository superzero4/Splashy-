using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UIEnums;
using System;
using UnityEditor;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "UISkin", fileName = "NewSkin")]
public class SkinData : ScriptableObject
{
    [Tooltip("IF the override default font checkbox isn't checked and not all fonts overrides it, it will cause errors")]
    public DefaultFont DefaultFontData;
    public Color SceneBackground;
    public Color MainColor;
    /*public Sprite PanelImage;
    public Color PanelBackground;*/
    public List<Color> SideColors;
    [Tooltip("This refers to the FIRST Image component in the hierarchy where FlexibleUI script is placed" +
        "\n IF the override default font checkbox isn't checked and not all fonts overrides it, it will cause errors")]
    public DefaultSpriteData DefaultBackground;
    [Tooltip("This refers to the LAST Image component in the hierarchy where FlexibleUI (which are nut FlexibleButton) script is placed"+
        "\n IF the override default font checkbox isn't checked and not all fonts overrides it, it will cause errors")]
    public DefaultSpriteData DefaultIcon;
    //public SerializableDictionnary<TypeOfIcons, Sprite> dictionnary = new SerializableDictionnary<TypeOfIcons, Sprite>();
    [SerializeField, Header("Only the first occurence of each icon type will be retained")]
    private Pair<TypeOfButton, ButtonData>[] _buttonTypeToSprite;
    /*public ButtonData SpritesForToggle;
    public ButtonData SpritesForToggleInToggleGroup;*/
    public Dictionary<TypeOfButton, ButtonData> SpritesOfInteractables;
    [Header("Lowest index in list is highest level of title")]
    public TitleData[] LevelOfTitles;
    public SliderData[] sliders;

    private void OnValidate()
    {
        FindObjectOfType<MainMenuManager>()?.resetSkin();
    }

    internal void setSkinData()
    {
        SpritesOfInteractables = new Dictionary<TypeOfButton, ButtonData>();
        foreach (var kv in _buttonTypeToSprite)
        {
            if (!SpritesOfInteractables.ContainsKey(kv.key))
            {
                SpritesOfInteractables.Add(kv.key, kv.value);
            }
        }
    }
    [Serializable]
    public class Pair<K, V>
    {
        public K key;
        public V value;
    }
    [Serializable]
    public class ButtonData
    {
        public SpriteData icon;
        [Header("Will be set to default icon background if kept to none")]
        public SpriteData backgrond;
        public Transition transitionType;
        [Header("Only one of the two following blocks must be filled, accordingly to the transition type chose")]
        public SpriteState transitions;
        public ColorBlock colors;
    }
    [Serializable]
    public class SpriteState
    {
        public UnityEngine.UI.SpriteState transitions;
    }
    [Serializable]
    public class ColorBlock
    {
        public UnityEngine.UI.ColorBlock colors/*=new UnityEngine.UI.ColorBlock()*/;
    }
    [Serializable]
    public class SpriteData
    {
        public Sprite sprite;
        public bool overrideMainColor;
        public Color color/*=new Color(1,1,1,1)*/;
    }
    [Serializable]
    public class DefaultSpriteData : SpriteData
    {
        //[HideInInspector]public new bool overrideMainColor = false;
    }
    [Serializable]
    public class FontAndColorData
    {
        public bool overrideFontData;
        public TMP_FontAsset font;
        public Color colorOfText;
    }
    [Serializable]
    public class DefaultFont : FontAndColorData
    {
        //[HideInInspector]public new bool overrideFontData=false;
    }
    [Serializable]
    public class TitleData
    {
        public FontAndColorData font;
        [Header("Negative value will keep original font size")]
        public int sizeOfFont = 0;
        public SpriteData underLine;
    }
    [Serializable]
    public class SliderData
    {
        public SpriteData background;
        public SpriteData fillArea;
        public SpriteData handle;
        public ColorBlock handleColors/*=ColorBlock.defaultColorBlock*/;
    }
}
