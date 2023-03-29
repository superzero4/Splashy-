using UnityEngine;

public abstract class Dependency<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    private T controller;
    protected T _controller { get => controller; }
    private void OnValidate()
    {
        if (transform.parent != null)
            controller = transform.parent.GetComponentInChildren<T>();
        else
            controller = FindObjectOfType<T>();
    }
}
