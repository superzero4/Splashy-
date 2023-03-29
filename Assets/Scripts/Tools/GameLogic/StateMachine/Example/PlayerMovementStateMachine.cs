using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace StateMachinesExample
{
    public enum Moves
    {
        IDLE, WALK, SPRINT, 
    }
    public class IdleState : PlayerState<Moves>
    {
        public IdleState(SamplePlayerController p, string paramName) : base(p, paramName, Moves.IDLE)
        {
        }
        public override void OnFixedUpdateState()
        {

        }

        protected override void initTransitions()
        {
            AddTransition(Moves.WALK, () => _objectToInteract.Test3);
            AddTransition(Moves.SPRINT, (p) =>
            {
                //Debug.Log("Testing idle => run returning : "+ _input.Direction.HasMovement);
                return _objectToInteract.Test2;
            });           
        }
    }
    public class WalkState : PlayerState<Moves>
    {
        public WalkState(SamplePlayerController p, string paramName) : base(p, paramName, Moves.WALK)
        {
        }
        public override void OnFixedUpdateState()
        {
            if (_objectToInteract.Position == Positions.COVER)
                _objectToInteract.MoveAlongCover();
            else
                _objectToInteract.Walk();
        }

        protected override void initTransitions()
        {
            AddTransition(Moves.SPRINT, (p) =>
            {
                //Debug.Log("Testing Walk => run returning : " + _input.Sprint.Held);
                return _objectToInteract.Test3 && p.Position != Positions.COVER;
            });
        }
    }
    public class SprintState : PlayerState<Moves>
    {
        //public static SprintTreshold=.5f;
        public SprintState(SamplePlayerController p, string paramName) : base(p, paramName, Moves.SPRINT)
        {
        }
        public override void OnFixedUpdateState()
        {
            _objectToInteract.Sprint();
        }

        protected override void initTransitions()
        {
            AddTransition(Moves.WALK, () => !_objectToInteract.Test1);
        }

    }
#if false
public class ToCopyState : PlayerState<Moves>
{
    public ToCopyState(PlayerController p, string paramName, float exitTime) : base(p, paramName, exitTime, Moves.)
    {
    }
    public override void OnUpdateState()
    {

    }

    protected override void initTransitions()
    {
        AddTransition(Moves.IDLE, () =>);
        AddTransition(Moves.WALK, () =>);
        AddTransition(Moves.SPRINT, () => );
    }
}
#endif
    public class PlayerMovementStateMachine : PlayerStateMachine<Moves>
    {
        /*public override TypeOfState AddState<TypeOfState>(string paramName = "", float exitTime = -1)
        {
            var s = base.AddState<TypeOfState>(paramName, exitTime);
            AddAnyState(s);
            return s;
        }*/
        protected override void InitStates()
        {
            //AddMandatory(Moves.SPRINT, (p) => p.Position != Positions.COVER);
            AddState<IdleState>("IsIdle");
            AddState<WalkState>("IsWalk");
            AddState<SprintState>("IsSprint");
            
            //Debug.Log(States[Moves.IDLE].Transitions.Count + " leaving idle");
            AddAnyStateTransition(Moves.IDLE, () => !ObjectToInteract.Test2,false);
            //Debug.Log(States[Moves.IDLE].Transitions.Count + " leaving idle");
            //Debug.Log(States[Moves.SPRINT].Transitions.Count + " leaving sprint");
        }
    }
}
