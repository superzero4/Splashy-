using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T _instance;
    public static T Instance
    {
        get
        {
            return _instance ?? FindObjectOfType<T>();
        }
    }
    /// <summary>
    /// Checking for singleton unicity and/or setting ref
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            Debug.LogWarning("Two managers where present at the same time, the last one was destroyed");
        }
        else
        {
            _instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
    }
}