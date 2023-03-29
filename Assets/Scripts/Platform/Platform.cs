using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Eraudf
{
    
    public enum PlatformType { None, Normal, Final,Bonus }
    public class Platform : MonoBehaviour
    {
        [SerializeField]
        private GameObject _marker;
        #region _serialized
        [SerializeField]
        private Material _touchedMaterial;
        #endregion
        #region _private
        private PlatformType _type;
        private Vector2 _pos;
        private float _radius;
        #endregion
        #region Properties
        public Vector2 Pos { get => _pos; }
        public float Radius { get => _radius; set => _radius = value; }
        public PlatformType Type { get => _type; set => _type = value; } 
        #endregion

        void Awake()
        {
            Transform transform1 = transform;
            var v3 = transform1.position;
            _pos = new Vector2(v3.x, v3.z);
            _radius = 1.3f*(transform1.localScale.x/2);
        }

        internal void PlatformHitted()
        {
            foreach(var rend in GetComponentsInChildren<Renderer>())
                rend.material = _touchedMaterial;
            //_marker.GetComponentInChildren<Renderer>().material = _touchedMaterial;
        }

        internal void SetAsBonus()
        {
            _marker.SetActive(true);
        }
    }

}