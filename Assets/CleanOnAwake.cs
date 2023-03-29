using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Eraudf
{
    public class CleanOnAwake : MonoBehaviour
    {
        [SerializeField]
        private List<Component> _toDestroy;
        [SerializeField]
        private bool _keepForEditor;
        void Awake()
        {
#if UNITY_EDITOR
            if (!_keepForEditor)
            {
                foreach (var c in _toDestroy)
                {
                    Destroy(c);
                }
            }
#else
foreach (var c in _toDestroy)
            {
                Destroy(c);
            } 
#endif
        }
    }

}