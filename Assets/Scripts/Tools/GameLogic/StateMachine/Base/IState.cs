using System;
using System.Collections.Generic;
using UnityEngine;
namespace AbstractStateMachine
{
    public interface IState<T, V> where T : Enum
    {
        public void OnFixedUpdateState();
        public bool checkForTransition(out Transition t);
        public class Transition
        {
            private T dest;
            private List<Predicate<V>> condition;

            public T Dest { get => dest; }

            public void AddCondition(Predicate<V> conditionToAdd)
            {
                condition.Add(conditionToAdd);
            }
            public void AddCondition(Func<bool> conditionToAdd)
            {
                AddCondition(Helpers.AsPredicate<V>(conditionToAdd));
            }
            public bool Test(V target)
            {
                foreach(var p in condition)
                {
                    if (!p.Invoke(target))
                    {
                        return false;
                    }
                }
                return true;
            }
            public Transition(T dest, Predicate<V> condition)
            {
                this.dest = dest;
                this.condition = new List<Predicate<V>>();
                AddCondition(condition);
            }
        }

        public void OnExitState();
        public void OnEnterState();
    }
    public abstract class State<T, V> : IState<T, V> where T : Enum where V : class
    {
        public class Transition : IState<T, V>.Transition
        {
            public Transition(T dest, Predicate<V> condition) : base(dest, condition)
            {               
            }
        }
        private T _key;
        protected V _objectToInteract;
        protected List<Transition> _transitions;
        public List<Transition> Transitions { get => _transitions; }
        private static readonly Predicate<V> TruePredicate = (p) => true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="condition">Use a null/true predicate to transition automatically </param>
        public virtual Transition AddTransition(T dest, Predicate<V> condition = null,int priority=-1)
        {
            Transition transition = new Transition(dest, condition ?? TruePredicate);
            if (priority == -1 || _transitions.Count==0)
                _transitions.Add(transition);
            else
            {
                var count = _transitions.Count;
                int index = Mathf.Clamp(count - 1 - priority, 0, count - 1);
                _transitions.Insert(index, transition);
            }
            return transition;
        }
        public void AddTransition(T dest, Func<bool> condition)
        {
            AddTransition(dest, Helpers.AsPredicate<V>(condition));
        }
        public T Key { get => _key; set => _key = value; }

        /*private static State<T, V> instance;

    public static State<T, V> Instance { get { return Instance==null ? new State<T,V>() : instance ): } set => instance = value; }
    */
        public State(V objectToCheck, T key)
        {
            _transitions = new List<Transition>();
            initTransitions();
            _key = key;
            _objectToInteract = objectToCheck;
        }
        public State()
        {
            _transitions = new List<Transition>();
            initTransitions();
        }
        /// <summary>
        /// Use AddTransition protected method to add the necessary transition to the list, this function is called in Ctor
        /// </summary>
        protected abstract void initTransitions();
        public bool checkForTransition(out IState<T, V>.Transition result)
        {
            foreach (var transition in _transitions)
            {
                if (transition.Test(_objectToInteract))
                {
                    result = transition;
                    return true;
                }
            }
            result = null;
            return false;
        }
        /*public virtual void Awake()
        {
            _componentToCheck = GetComponent<V>();
        }*/
        public abstract void OnEnterState();
        public abstract void OnExitState();
        public abstract void OnFixedUpdateState();
        public abstract void OnUpdateState();
    }
}