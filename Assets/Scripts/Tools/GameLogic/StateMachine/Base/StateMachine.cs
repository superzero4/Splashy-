using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
namespace AbstractStateMachine
{
#pragma warning disable CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
#pragma warning disable CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
    public abstract class StateMachine<T, V> : MonoBehaviour where T : Enum where V : UnityEngine.Object
#pragma warning restore CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
#pragma warning restore CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
    {
        public static bool operator ==(StateMachine<T, V> machine, T valueOfCurrentStateToTest)
        {
            try
            {
                return machine.CurrentState.Key.Equals(valueOfCurrentStateToTest);
            }
#pragma warning disable IDE0059 // Assignation inutile d'une valeur
#pragma warning disable CS0168 // La variable est déclarée mais jamais utilisée
            catch (NullReferenceException E)
#pragma warning restore CS0168 // La variable est déclarée mais jamais utilisée
#pragma warning restore IDE0059 // Assignation inutile d'une valeur
            {
                return false;
            }
        }
        public static implicit operator T(StateMachine<T, V> machine) => machine._currentState.Key;
        public static bool operator !=(StateMachine<T, V> machine, T valueOfCurrentStateToTest)
        {
            return !(machine == valueOfCurrentStateToTest);
        }
        private V _objectToInteract;
        private State<T, V> _currentState;
        private Dictionary<T, State<T, V>> _states;
        private Dictionary<T, Predicate<V>> _mandatories = new Dictionary<T, Predicate<V>>();
        public void AddAnyStateTransition(T dest, Predicate<V> condition, bool canTransitionToSelf, int priority = 0)
        {
            foreach (T s in _states.Keys)
            {
                //Debug.Log(dest + "," + s + " are evaluated != " + !dest.Equals(s) + " but ");
                if (canTransitionToSelf || !dest.Equals(s))
                {
                    State<T, V> state = _states[s];
                    int count = state.Transitions.Count;
                    var t = state.AddTransition(dest, condition, count - 1 - priority);
                    AddEventualMandatories(t);
                }
            }
        }
        public void AddAnyStateTransition(T dest, Func<bool> condition, bool canTransitionToSelf)
        {
            AddAnyStateTransition(dest, Helpers.AsPredicate<V>(condition), canTransitionToSelf);
        }
        public void AddMandatory(T dest, Predicate<V> mandatory)
        {
            _mandatories.Add(dest, mandatory);
        }
        public void AddMandatory(T dest, Func<bool> mandatory)
        {
            _mandatories.Add(dest, Helpers.AsPredicate<V>(mandatory));
        }
        private void Start()
        {
            InitStates();
            _currentState.OnEnterState();
        }
/*        public void ResetToFirstState()
        {
            _currentState = _states.First().Value;
            
        }*/
        private void FixedUpdate()
        {
            _currentState.OnFixedUpdateState();
            /*IState<T, V>.Transition t;
            if (_currentState.checkForTransition(out t))
            {
                TransitionToState(t.Dest);
            }*/
        }
        private void Update()
        {
            _currentState.OnUpdateState();
            ForceTransitionCheck();
        }

        /*public*/ private void ForceTransitionCheck()
        {
            if (_currentState.checkForTransition(out IState<T, V>.Transition t))
            {
                TransitionToState(t.Dest);
            }
        }

        public void TransitionToState(T dest)
        {
            _currentState.OnExitState();
            TransitionToStateIgnoringEnter(dest);
        }
        public void TransitionToStateIgnoringEnter(T dest)
        {
            _currentState = _states[dest];
            _currentState.OnEnterState();
        }
        /// <summary>
        /// Set the _states list in this function, use AddState<> to perform that efficiently
        /// </summary>
        protected abstract void InitStates();


        public State<T, V> CurrentState { get => _currentState; set => _currentState = value; }
        public V ObjectToInteract
        {
            get
            {
                if (_objectToInteract == null)
                {
                    _objectToInteract = GetComponentInChildren<V>();
                    if (_objectToInteract == null)
                    {
                        Debug.LogWarning("No object of type : " + typeof(V).Name + " , trying to find one in parents");
                        _objectToInteract = GetComponentInParent<V>();
                        if (_objectToInteract == null)
                        {
                            Debug.LogWarning("No object of type : " + typeof(V).Name + ", trying to find one in the whole scene but it might" +
                            "not be the one expected");
                            _objectToInteract = FindObjectOfType<V>();
                            if (_objectToInteract == null)
                            {
                                Debug.LogError("No object of type : " + typeof(V).Name + " nowhere in scene");
                            }
                        }
                    }
                }
                return _objectToInteract;
            }
            set => _objectToInteract = value;
        }

        public Dictionary<T, State<T, V>> States { get => _states; set => _states = value; }

        public void AddState(State<T, V> state)
        {
            _states.Add(state.Key, state);
            if (_currentState == null)
            {
                _currentState = state;
            }
        }
        public TypeOFState AddState<TypeOFState>(object ctorFirstArg, params object[] ctorArgument) where TypeOFState : State<T, V>
        {
            List<object> args = new List<object> { ctorFirstArg };
            args.AddRange(ctorArgument);
            return CallCtorFromArgList<TypeOFState>(args);
        }
        public TypeOFState AddState<TypeOFState>(params object[] ctorArgument) where TypeOFState : State<T, V>
        {
            //List<object> args = new List<object> { ObjectToInteract };
            //return AddState<TypeOFState>(ObjectToInteract, ctorArgument);
            //args.AddRange(ctorArgument);
            return CallCtorFromArgList<TypeOFState>(ctorArgument.ToList()) ;
        }
        private TypeOFState CallCtorFromArgList<TypeOFState>(List<object> args) where TypeOFState : State<T, V>
        {
            args.Insert(0, ObjectToInteract);
            var argsType = args.Select((a) => a.GetType());
            //Debug.Log(string.Join(";", argsType.Select((t) => t.Name)));
            /* Debug.Log(string.Join("  ",typeof(TypeOFState).GetConstructors().Select((c)=> string.Join(";", c.GetParameters().Select((p)=>p.ParameterType.Name)))));
            */
            try
            {
                var ctor = typeof(TypeOFState).GetConstructor(argsType.ToArray());
                TypeOFState state = (TypeOFState)ctor.Invoke(args.ToArray());
                var member = (T)typeof(TypeOFState).GetProperty("Key").GetValue(state);
                //Debug.Log(member + " member of enum found");
                _states.Add(member, state);
                //Debug.Log("keys : "+string.Join(";", _mandatories.Keys));
                foreach (var t in state.Transitions)
                {
                    AddEventualMandatories(t);
                }
                if (_currentState == null)
                {
                    _currentState = state;
                }
                return state;
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("No PUBLICS constructors with arguments of types " + string.Join(";", argsType) + " of lenght : " + argsType.ToArray().Length
                    + " were found for " + typeof(TypeOFState).Name);
                return null;
            }
        }

        public TypeOFState AddState<TypeOFState>() where TypeOFState : State<T, V>
        {
            return AddState<TypeOFState>(new object[0]);
        }
        private void AddEventualMandatories(State<T, V>.Transition t)
        {
            if (_mandatories.ContainsKey(t.Dest))
            {
                t.AddCondition(_mandatories[t.Dest]);
            }
        }
        public StateMachine()
        {
            _states = new Dictionary<T, State<T, V>>();
        }
        public StateMachine(State<T, V> currentState, T firstState)
        {
            _currentState = currentState;
            _states = new Dictionary<T, State<T, V>>();
            _states.Add(_currentState.Key, _currentState);
            Array array = Enum.GetValues(typeof(T));
            _currentState = _states[firstState];
        }
        public StateMachine(List<State<T, V>> states)
        {
            _states = new Dictionary<T, State<T, V>>();
            foreach (var s in states)
            {
                _states.Add(s.Key, s);
            }
        }


    }
    public static class Helpers
    {
        public static Predicate<V> AsPredicate<V>(Func<bool> condition)
        {
            return (entry) => condition.Invoke();
        }
    }
}