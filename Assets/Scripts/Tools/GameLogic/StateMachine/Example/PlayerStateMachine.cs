using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AbstractStateMachine;
using UnityEngine.UI;
using TMPro;

namespace StateMachinesExample
{
    public abstract class PlayerStateMachine<E> : StateMachine<E, SamplePlayerController> where E : Enum
    {
#if UNITY_EDITOR
        [SerializeField] private TMP_Text _text;
#endif
        public virtual TypeOfState AddState<TypeOfState>(string paramName = "", float exitTime = -1) where TypeOfState : PlayerState<E>
        {
            object[] obj;
            if (exitTime > 0)
            {
                obj = new object[2];
                obj[1] = exitTime;
            }
            else
            {
                obj = new object[1];
            }
            obj[0] = paramName;
            TypeOfState state = base.AddState<TypeOfState>(obj);
#if UNITY_EDITOR
            state._text = _text;
#endif
            return state;
        }
    }
    public abstract class PlayerTimedState<E> : PlayerState<E> where E : Enum
    {
        protected float _exitTime;
        protected float _duration;
        public PlayerTimedState(SamplePlayerController p, string paramName, float duration, E member) : base(p, paramName, member)
        {
            _duration = duration;
            //_exitTime = exitTime;
            if (duration > 0)
            {
                /*foreach (var t in _transitions)
                {
                    //Equivalent??
                    AddTimerToCondition(t);
                }*/
            }
            else
            {
                Debug.LogWarning("There's no point in using timed states with negative/no time, use regular states instead");
            }
        }
        public void AddTimerToCondition(IState<E,SamplePlayerController>.Transition t)
        {
            t.AddCondition(() =>
            {
            //Debug.Log("Comparing time : " + Time.time + " with" + _exitTime);
            return Time.time > _exitTime;
            });
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            _exitTime = Time.time + _duration;
        }
        public override Transition AddTransition(E dest, Predicate<SamplePlayerController> condition = null, int priority = -1)
        {
            var t = base.AddTransition(dest, condition, priority);
            AddTimerToCondition(t);
            return t;
        }
    }
    public abstract class PlayerState<E> : State<E,SamplePlayerController> where E : Enum
    {
        string _paramName;
        protected Animator _animator;
#if UNITY_EDITOR
        public TMP_Text _text;
#endif

        public PlayerState(SamplePlayerController p, string paramName, E member) : base(p, member)
        {
            _animator = p.Animator;
            _paramName = paramName;
        }
        public override void OnEnterState()
        {
#if UNITY_EDITOR
            if (_text != null)
                _text.text = this._paramName;
            //Debug.Log("Entering : " + _paramName);
#endif
            if (_paramName != "")
            {
                _animator.SetBool(_paramName, true);
            }
        }
        public override void OnExitState()
        {
            if (_paramName != "")
            {
                _animator.SetBool(_paramName, false);
            }
        }
        public override void OnUpdateState()
        {
        }
    }
}
