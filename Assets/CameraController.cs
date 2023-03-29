using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Eraudf
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement _player;
        private Transform _transform;
        void Awake()
        {
            _transform = transform;
            
        }
        private void Start()
        {
            var v = _transform.position;
            v.y = _player.TopHeight * .9f;
            _transform.position = v;
        }
        private void Update()
        {
            _transform.position += _player.NormalizedConstantMovement;
        }
    }

}