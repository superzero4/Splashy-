using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Eraudf
{
    public class PlayerMovement : MonoBehaviour
    {
        #region _serialized
        [SerializeField, Tooltip("Unit/Sec")]
        private Vector3 _constantMovement;
        [SerializeField, Range(.5f, 3)]
        private float _constantMovementMultiplier = 1;
        [SerializeField,Range(.5f,2)]
        private float _timeToDoCompleteYMove;
        [SerializeField]
        private PlayerInput _input;
        [SerializeField]
        private Transform _topMarker;
        [SerializeField]
        private PlatformSpawner _platforms;
        #endregion
        #region Properties
        public Vector3 NormalizedConstantMovement { get => _constantMovement * (Time.deltaTime * _constantMovementMultiplier); }
        public float TopHeight { get => _topHeight; set => _topHeight = value; }
        #endregion        
        #region _privates
        private Transform _transform;
        private float _topHeight;
        Sequence sequence;
        #endregion
        void Awake()
        {
            _transform = transform.parent;
            _topHeight = _topMarker.position.y;
        }
        private void Start()
        {
            DOTween.Init();
            _constantMovement.z = _platforms.ZSpacing / (_timeToDoCompleteYMove * 2);
            TweenCallback platformCheck = ()=> CheckForPlatform();
            sequence = DOTween.Sequence();
            sequence.Append(TweenUp()).Append(TweenDown()).AppendCallback(platformCheck).SetLoops(-1,LoopType.Restart);          
        }
        private void Update()
        {
            var pos = _transform.position;
            pos += NormalizedConstantMovement;
            pos.x = _input.FingerX*_platforms.XWidht;
            _transform.position = pos;
            /*if (pos.y > _topHeight)
            {
                TweenDown();
            }
            else if (pos.y < 0)
            {
                CheckForPlatform();
            }*/
        }

        private void CheckForPlatform()
        {
            var pos = _transform.position;
            switch (_platforms.IsOnAPlatform(new Vector2(pos.x, pos.z)))
            {
                case PlatformType.None: Lost();break;
                case PlatformType.Normal: break;
                case PlatformType.Bonus: StartCoroutine(EnableBonusFor(3));break;
                case PlatformType.Final: _constantMovement = Vector3.zero;
                    StartCoroutine(LoadNext(2f));
                    break;
            }
        }

        private IEnumerator EnableBonusFor(int _nbOfJumps)
        {
            var t = _constantMovementMultiplier;
            _constantMovementMultiplier = 2f;
            yield return new WaitForSeconds(_nbOfJumps * _timeToDoCompleteYMove*2);
            _constantMovementMultiplier = t;
        }

        private IEnumerator LoadNext(float v)
        {
            yield return new WaitForSeconds(v);
            sequence.Kill();
            DOTween.Clear();
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1)%SceneManager.sceneCountInBuildSettings);
        }

        private void Lost()
        {
            sequence.Kill();
            DOTween.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //throw new NotImplementedException("Game Lost");
        }

        private Tween TweenUp()
        {
            //_currentTween.Kill();
            return _transform.DOMoveY(_topHeight, _timeToDoCompleteYMove*_constantMovementMultiplier);
        }

        private Tween TweenDown()
        {
            /*_currentTween.Kill();
            _currentTween = */
            Debug.Log("Tweening down");
            return _transform.DOMoveY(0, _timeToDoCompleteYMove*_constantMovementMultiplier);
        }
    }

}