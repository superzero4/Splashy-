using System;
using UnityEngine;
public class GameObjectSelector : MonoBehaviour
{
    [SerializeField, RangeBasedOnChildCount] protected int _indexOfGameobjectToUse;
    public virtual int IndexOfModelToUse
    {
        get => _indexOfGameobjectToUse; set
        {
            _indexOfGameobjectToUse = Mathf.Clamp(value, 0, transform.childCount);
            PickObject();
        }
    }
    public GameObject Selected { get => transform.GetChild(_indexOfGameobjectToUse).gameObject; }
    public virtual void PickObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool isPickedObject = i == _indexOfGameobjectToUse;
            transform.GetChild(i).gameObject.SetActive(isPickedObject);
            OtherOperationOnGameObjects(isPickedObject);
        }
    }
    public void PickObject(int index)
    {
        IndexOfModelToUse = index;
        PickObject();
    }
    public virtual void PickRandom(int maxOffset=0)
    {
        _indexOfGameobjectToUse = UnityEngine.Random.Range(0, transform.childCount-maxOffset);
        PickObject();
    }
    protected virtual void OtherOperationOnGameObjects(bool isPickedObject)
    {

    }
    private void OnValidate()
    {
        PickObject();
    }
}