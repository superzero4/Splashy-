using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ModelSelector : GameObjectSelector
{
    [SerializeField] Animator _animator;
    public override int IndexOfModelToUse
    {
        get => base.IndexOfModelToUse; set
        {
            _indexOfGameobjectToUse = Mathf.Clamp(value, 0, transform.childCount-2);
            PickObject();
        }
    }
    public override void PickObject()
    {
        for (int i = 0; i < transform.childCount-1; i++)
        {
            bool isPickedObject = i == _indexOfGameobjectToUse;
            transform.GetChild(i).gameObject.SetActive(isPickedObject);
            OtherOperationOnGameObjects(isPickedObject);
        }
    }
    public override void PickRandom(int maxOffset = 0)
    {
        base.PickRandom(maxOffset+1);
    }
    protected override void OtherOperationOnGameObjects(bool b)
    {
            
            /*if (b)         
                transform.GetChild(i).GetComponent<Animator>().gameObject.SetActive(false);
            else
            {
                var an = transform.GetChild(i).GetComponent<Animator>();
                var av = an.avatar;
                an.gameObject.SetActive(true);
                _animator.avatar = av;
            }*/
    }
    private void OnValidate()
    {
        IndexOfModelToUse = _indexOfGameobjectToUse;
    }
}
