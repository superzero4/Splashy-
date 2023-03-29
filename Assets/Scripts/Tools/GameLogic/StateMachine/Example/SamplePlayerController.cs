using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachinesExample;
using System;
using UnityEditor;
namespace StateMachinesExample
{
    public class SamplePlayerController : MonoBehaviour
    {
        #region Settings
        [Header("Movement settings")]
        [SerializeField, Range(0.01f, 5), Tooltip("M/s")] private float _speed;
        [SerializeField, Range(1, 4f)] private float _sprintMultiplier;
        [SerializeField, Range(.05f, 1)] private float _crouchMultiplier;
        [SerializeField, Range(.01f, 4)] private float _timeToPerformHalfTurn;
        [SerializeField, Range(.01f, 10)] private float _jumpForce;
        [SerializeField, Range(.01f, 2)] private float _standingHeight;
        [SerializeField, Range(.01f, 1)] private float _crouchHeight;
        [SerializeField, Range(.1f, 1)] private float _distanceToCover;
        private Vector3 _jumpVectorForce;
        #endregion
        #region Components
        [Header("Components link")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerAirStateMachine _air;
        [SerializeField] private PlayerPositionsStateMachine _position;
        [SerializeField] private PlayerMovementStateMachine _move;
        [SerializeField] private Transform _cam;
        [SerializeField] private CapsuleCollider _playerCollider;

        #endregion
        #region Private fields
        private Transform _rbsTransform;
        //private Ray? _currentCover;
        //private Vector3 _lookDirection;
        //private int _dominantStateMachine;
        #endregion

        public Rigidbody Rb { get => _rb; set => _rb = value; }


        public bool Test1 { get => true; }
        public bool Test2 { get => false; }
        public bool Test3 { get => true; }
        public Animator Animator { get => _animator; set => _animator = value; }
        public PlayerMovementStateMachine Move { get => _move; set => _move = value; }
        public PlayerPositionsStateMachine Position { get => _position; set => _position = value; }
        public PlayerAirStateMachine Air { get => _air; set => _air = value; }
        public float CrouchHeight { get => _crouchHeight; set => _crouchHeight = value; }
        public float StandingHeight { get => _standingHeight; set => _standingHeight = value; }
        public float DistanceToCover { get => _distanceToCover; set => _distanceToCover = value; }
        public enum SoundEmmited { SILENT, FOOTSTEPS, NOISY }
        public SoundEmmited MakingNoise
        {
            get
            {
                if (_position == Positions.COVER || _move == Moves.IDLE || (_position == Positions.CROUCH && _move == Moves.WALK))
                    return SoundEmmited.SILENT;
                if (_move == Moves.WALK || _position == Positions.CROUCH)
                    return SoundEmmited.FOOTSTEPS;
                else
                    return SoundEmmited.NOISY;
            }
        }
#if UNITY_EDITOR
        public float SprintMultiplier { get => _sprintMultiplier; set => _sprintMultiplier = value; }
#endif
        //public Ray? CurrentCover { get => _currentCover; set => _currentCover = value; }
        //public Vector3 LookDirection { get => _lookDirection; set => _lookDirection = value; }

        private void Awake()
        {
            _rbsTransform = _rb.transform;
        }
        /*private void Update()
        {
            //Debug.Log("noise = " + MakingNoise);
            if (_input.Reload.Down)
                _checkpoints.ReloadCheckPoint(_rb);
        }*/
        public void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.gameObject.name + "<=collided  entered from " + gameObject.name);
        }
        public void AdjustColliderSize(float height)
        {
            _playerCollider.center = new Vector3(0, height / 2, 0);
            _playerCollider.height = height;
            //StartCoroutine(temp(height));       
        }
        private IEnumerator temp(float height)
        {
            yield return new WaitForFixedUpdate();
        }
        private void AdjustRot()
        {
            //Debug.Log("rots :" + _cam.rotation.eulerAngles.y + "," + _rbsTransform.rotation.eulerAngles.y);
            float diff = _cam.rotation.eulerAngles.y - _rbsTransform.rotation.eulerAngles.y;
            float signum = Mathf.Sign(diff);
            diff = (signum < 0 ? -diff : diff);
            diff = Mathf.Min(360 - diff, diff);
            //Quaternion.Angle(_cam.rotation.)
            float step = (180f / _timeToPerformHalfTurn) * Time.deltaTime;
            if (diff > 2 * step)
            {
                _rb.MoveRotation(Quaternion.Euler(_rb.rotation.eulerAngles.x, _rb.rotation.eulerAngles.y + signum * step, _rb.rotation.eulerAngles.z));
            }
        }

        public void MoveAtSpeed(float runMultiplier)
        {
            throw new NotImplementedException();
        }
        internal void MoveAlongCover()
        {
            throw new NotImplementedException();
        }
        internal void SnapToCover(Vector3 point, Vector3 normal)
        {
            throw new NotImplementedException();
        }
        private void MoveCharacter(float speed, Vector3 clampedDirection)
        {
            //Debug.Log(speed * clampedDirection + " vect :" + clampedDirection);
            _rb.MovePosition(_rb.position + speed * clampedDirection);

        }
        internal void Walk()
        {
            MoveAtSpeed(1);
        }
        internal void Sprint()
        {
            MoveAtSpeed(_sprintMultiplier);
        }
        internal void Jump()
        {
            _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.VelocityChange);
        }

        /// <summary>
        /// Only x axis will be updated
        /// </summary>
        /// <param name="x"></param>
        private void UpdateAnimatorWithInputs(float x)
        {
            _animator.SetFloat("xDirection", x);
        }
        private void UpdateAnimatorWithInputs(float x, float y)
        {
            UpdateAnimatorWithInputs(x);
            _animator.SetFloat("yDirection", y);
        }
#if UNITY_EDITOR
        public static void DisplayCrouchSprintSpeed(SerializedProperty _sprintMultiplier, SerializedProperty _crouchMultiplier)
        {
            //EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Relative speed when sprinting and crouching, should be lower than 1");
            EditorGUILayout.LabelField((_sprintMultiplier.floatValue * _crouchMultiplier.floatValue).ToString());
            GUILayout.FlexibleSpace();
            //EditorGUILayout.EndHorizontal();
        }
#endif
    }
}
