using System;
using UnityEngine;
namespace AbstractStateMachine
{
    public abstract class TimedState<E,V> : State<E, V> where E : Enum where V : class
    {
        protected float _exitTime;
        protected float _duration;

        public float ExitTime { get => _exitTime; set => _exitTime = value; }
        public float Duration { get => _duration; set => _duration = value; }

        public TimedState(V p, E member, float duration) : base(p, member)
        {
            _duration = duration;
            //_exitTime = exitTime;
            if (duration <= 0)
            {
                Debug.LogWarning("There's no point in using timed states with negative/no time, use regular states instead");
            }
            else
            {
                /*foreach (var t in _transitions)
                {
                    //Equivalent??
                    AddTimerToCondition(t);
                }*/
            }
        }
        public void AddTimerToCondition(IState<E, V>.Transition t)
        {
            t.AddCondition(() =>
            {
                //Debug.Log("Comparing time : " + Time.time + " with" + _exitTime);
                return Time.time > _exitTime;
            });
        }
        public void ResetTime(float newDuration)
        {
            _exitTime = Time.time + newDuration;
        }
        public void ResetTime()
        {
            ResetTime(_duration);
        }
        public override void OnEnterState()
        {
            ResetTime();
        }
        public override Transition AddTransition(E dest, Predicate<V> condition = null, int priority = -1)
        {
            var t = base.AddTransition(dest, condition, priority);
            AddTimerToCondition(t);
            return t;
        }
    }
}