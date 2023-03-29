using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*using EraudF;

namespace EraudF{*/
public class DestroyOnAwake : MonoBehaviour
{
    [SerializeField]
    private bool _keepForEditorTime=false;
    void Awake()
    {
#if UNITY_EDITOR
        if (!_keepForEditorTime)
            DestroyImmediate(gameObject);
#else
        DestroyImmediate(gameObject);
#endif
    }
}
//}