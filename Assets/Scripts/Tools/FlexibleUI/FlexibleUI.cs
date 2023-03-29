using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UIEnums;
using static SkinData;

//[ExecuteAlways()]
public /*abstract*/ class FlexibleUI : MonoBehaviour
{
    [HideInInspector]
    public SkinData Skin { get; set; }
    protected TMP_Text _text;
    protected Image _backGround;
    protected Image _icon;
    private SpriteData _iconDataToUse;
    private FontAndColorData _fontDataToUse;
    private SpriteData _backGroundDataToUse;

    protected SpriteData IconDataToUse { get => _iconDataToUse; set => _iconDataToUse = value.overrideMainColor ? value : Skin.DefaultIcon; }
    protected SpriteData BackGroundDataToUse { get => _backGroundDataToUse; set => _backGroundDataToUse = value.overrideMainColor ? value : Skin.DefaultBackground; }
    protected FontAndColorData FontDataToUse { get => _fontDataToUse; set => _fontDataToUse = value.overrideFontData ? value : Skin.DefaultFontData; }

    protected UIContains _type;
    // Start is called before the first frame update
    void Start()
    {
        SkinUI();
    }

    protected virtual void Reset()
    {
        if (Skin == null)
        {
            Skin = FindObjectOfType<MainMenuManager>().Skin;
        }
        IconDataToUse = Skin.DefaultIcon;
        BackGroundDataToUse = Skin.DefaultBackground;
        FontDataToUse = Skin.DefaultFontData;
        //_fontToUse = Skin.fontForDefaultText;
        _text = GetComponentInChildren<TMP_Text>();
        if (_text != null)
        {
            _type |= UIContains.Text;
        }
        var a = GetComponentsInChildren<Image>();
        int length = a.Length;
        if (length > 0)
        {
            _backGround = a[0];
            _type |= UIContains.Background;

            if (length > 1)
            {
                _icon = a[length - 1];
                _type |= UIContains.Icon;
            }
        }
    }
    [ContextMenu("Update skin")]
    public void ResetAndReskin()
    {
        Reset();
        SkinUI();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Application.isEditor)
        {
            if (Skin == null)
            {
                Debug.Log("No skin attached");               
            }
            else
            {

            }
            SkinUI();
            //Debug.Log(skin.dictionnary.e.ToString());
        }*/
    }
    protected virtual void SkinUI()
    {
        if (checkType(UIContains.Text))
        {
            SkinText();
        }
        if (checkType(UIContains.Background))
        {
            SkinBackground();
        }
        if (checkType(UIContains.Icon))
        {
            SkinIcon();
        }
    }

    protected virtual void SkinText()
    {
        //Debug.Log(gameObject.name+_fontDataToUse.colorOfText);
        FontAndColorData font = FontDataToUse.overrideFontData ? FontDataToUse : Skin.DefaultFontData;//_fontToUse.font;
        _text.font = FontDataToUse.font;
        _text.color = FontDataToUse.colorOfText;
    }
    protected virtual void SkinBackground()
    {
        changeImage(_backGround, BackGroundDataToUse);
        /*_backGround.color = __backGroundDataToUse.color;//Skin.PanelBackground;
        _backGround.sprite = __backGroundDataToUse.sprite*/
        ;
    }
    private void changeImage(Image image, SpriteData data)
    {
        image.color = data.color;
        image.sprite = data.sprite;
    }
    protected virtual void SkinIcon()
    {
        changeImage(_icon, IconDataToUse);
        /*        _icon.sprite = __iconDataToUse.sprite;// icon??Skin.DefaultIcon;
                _icon.color = __iconDataToUse.color;// color??Skin.MainColor;*/
    }

    private bool checkType(UIContains type)
    {
        return (_type & type) != 0;
    }
    /*   protected virtual void SkinText()
       {
           TryGetComponent<TMP_Text>().col
   }*/
}
