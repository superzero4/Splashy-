using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Eraudf
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector2 _normalizeFingerPos = Vector2.zero;

        public Vector2 NormalizedFingerPos
        {
            get
            {
                if (Input.touchCount > 0)
                {
                    _normalizeFingerPos = Input.GetTouch(0).position*Screen.width;
                }
                return _normalizeFingerPos;
            }
        }
        public float FingerX { get => NormalizedFingerPos.x; }

#if !UNITY_ANDROID || UNITY_EDITOR  
        private void Update()
        {
            _normalizeFingerPos.x = Mathf.Clamp(_normalizeFingerPos.x + Input.GetAxis("Horizontal"),-1,1);
            //_fingerPos.y += Input.GetAxis("Vertical");
        } 
#endif
    }

}