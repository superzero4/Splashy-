using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectAdditiveSelector : GameObjectSelector
{
    public override void PickObject()
    {
        //base.PickObject();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform transform1 = transform.GetChild(i);
            transform1.gameObject.SetActive(i >= _indexOfGameobjectToUse);
            transform1.GetComponentInChildren<GameObjectSelector>().PickObject(i + 1);
        }
    }
}
