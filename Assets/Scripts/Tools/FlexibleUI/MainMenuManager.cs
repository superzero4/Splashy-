using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//[ExecuteAlways()]
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SkinData _skin;
    [SerializeField]
    private Camera _cam;

    public SkinData Skin { get => _skin; set => _skin = value; }

    // Start is called before the first frame update
    void Awake()
    {
        resetSkin();
    }
    [ContextMenu("Reset skin")]
    public void resetSkin()
    {
        Debug.LogWarning("Reskining UI");
        _skin.setSkinData();
        List<FlexibleUI> flexibleElements= new List<FlexibleUI>();
        GetComponentsInChildren<FlexibleUI>(true,flexibleElements);
        FlexibleUI backGroundSetter = _cam.GetComponent<FlexibleUI>();
        flexibleElements.Add(backGroundSetter);
        //Debug.Log(flexibleElements.Contains(backGroundSetter));
        foreach (var c in flexibleElements)
        {
/*            if (c.name.Contains("amera"))
            {
                Debug.Log("reseting camera");
            }*/
            //var cc = c as FlexibleUI;
            c.Skin = _skin;
            //Debug.Log((c.Skin != null)+" skinSetted");
            c.ResetAndReskin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void loadSkin(SkinData skin)
    {
        _skin = skin;
        resetSkin();
    }
}
