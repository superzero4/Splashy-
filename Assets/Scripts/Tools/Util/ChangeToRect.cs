using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
/*using EraudF;

namespace EraudF{*/
public class ChangeToRect : MonoBehaviour
{
    [SerializeField]
    private bool _sync;
    [SerializeField]
    private GameObject _original;
    void OnValidate()
    {
        /*Undo.RecordObject(this,"PrechangedTransform");
        foreach(var t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.AddComponent(typeof(RectTransform));
        }*/
        if (_sync)
        {
            var l1 = GetComponentsInChildren<MeshFilter>(true);
            var l2 = _original.GetComponentsInChildren<MeshFilter>(true);
            for (int i = 0; i < l2.Length; i++)
            {
                l1[i].sharedMesh = l2[i].sharedMesh;
                l1[i].gameObject.name = l2[i].gameObject.name;
                l1[i].GetComponent<Renderer>().sharedMaterial = l2[i].GetComponent<Renderer>().sharedMaterial;
                //l1[i].transform.localScale = l2[i].transform.localScale;
            }
        }
    }
}
//}