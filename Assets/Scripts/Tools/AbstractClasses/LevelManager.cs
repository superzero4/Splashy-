using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The idead of this class is to hold a list of levels which is different from the list of scenes in build settings
/// So you can cycle through all your level in the order you set them in the array in the inspector and also have scenes for other thing in build settings
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField,
     Tooltip("If left empty, all the scene in build settings will be used as levels, you can also do this manually in a context menu option of this element"),
     ContextMenuItem("Add all scenes from build settings", "AddAllScenesFromBuild")]
    private List<int> _sceneIndexesOfLevelsToLoad;
    [SerializeField, Tooltip("If not checked, the first level will be reloaded after the last one")] private bool _quitAfterLastLevel;

    private int _sceneIndexOfCurrentlyLoadedLevel;
    static private LevelManager instance;
    private int _sceneCount;

    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();               
            }
            return  instance;
        }
    }
    public int SceneCount { get => _sceneIndexesOfLevelsToLoad.Count; set => _sceneCount = value; }

    //public List<int> SceneIndexesOfLevelsToLoad { get => _sceneIndexesOfLevelsToLoad; set => _sceneIndexesOfLevelsToLoad = value; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            Debug.LogWarning("Two managers where present at the same time, the last one was destroyed");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (_sceneIndexesOfLevelsToLoad == null || _sceneIndexesOfLevelsToLoad.Count == 0)
        {
            AddAllScenesFromBuild();
        }
        _sceneIndexOfCurrentlyLoadedLevel = _sceneIndexesOfLevelsToLoad.IndexOf(SceneManager.GetActiveScene().buildIndex);
        _sceneCount = _sceneIndexesOfLevelsToLoad.Count;
    }
    [ContextMenu("Add all scenes from build settings")]
    private void AddAllScenesFromBuild()
    {
        _sceneIndexesOfLevelsToLoad = new List<int>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            _sceneIndexesOfLevelsToLoad.Add(i);
        }
    }
    public void NextLevel()
    {
        Debug.Log(_sceneIndexOfCurrentlyLoadedLevel + ";" + _sceneCount);
        int currentSceneIndex = _sceneIndexOfCurrentlyLoadedLevel;
        if (_sceneIndexOfCurrentlyLoadedLevel == _sceneCount - 1)
        {
            if (_quitAfterLastLevel)
            {
                Application.Quit();
                Debug.LogWarning("Application quitted");
                return;
            }
            else
            {
                _sceneIndexOfCurrentlyLoadedLevel = -1;
            }
        }
        LoadSpecificSceneUnchecked(_sceneIndexOfCurrentlyLoadedLevel + 1);
    }

    private void LoadSpecificSceneUnchecked(int sceneIndexOfLevelToLoad)
    {
        SceneManager.UnloadSceneAsync(_sceneIndexesOfLevelsToLoad[_sceneIndexOfCurrentlyLoadedLevel]);
        int newSceneBuildIndex = _sceneIndexesOfLevelsToLoad[sceneIndexOfLevelToLoad];
        SceneManager.LoadScene(newSceneBuildIndex, LoadSceneMode.Additive);
        _sceneIndexOfCurrentlyLoadedLevel = sceneIndexOfLevelToLoad;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneIndex">The index of the scene in the list of this component, not to mistake with build index of scene</param>
    public void LoadSpecificSceneFromSceneIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < _sceneCount)
        {
            LoadSpecificSceneUnchecked(sceneIndexOfLevelToLoad: sceneIndex);
        }
        else
        {
            Debug.LogWarning($"Requested scene {sceneIndex} couldn't be loaded, be sure that you are not mistaking build" +
                "indexes with the index in the array specified in the LevelManager class");
        }
    }
    public void ReloadLevel()
    {
        LoadSpecificSceneUnchecked(_sceneIndexOfCurrentlyLoadedLevel);
    }
}
