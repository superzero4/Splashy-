using UnityEngine;

namespace StateMachinesExample
{
    public enum Air
    {
        GROUND, AIR, FALLING
    }
    public class AirState : PlayerTimedState<Air>
    {
        public AirState(SamplePlayerController p, string paramName, float exitTime) : base(p, paramName, exitTime, Air.AIR)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            _objectToInteract.Jump();
            //Debug.Break();
        }
        public override void OnFixedUpdateState()
        {
        }

        protected override void initTransitions()
        {
            AddTransition(Air.GROUND, (p) =>
            {
                //Debug.Log("Times :" + _exitTime + "," + Time.time);
                //Debug.Log("Testing Air => Ground returning (partial, not timed): " + p.Environment.GroundPresent);
                return  p.Test1;
            });
        }
    }
    public class FallState : PlayerState<Air>
    {
        public FallState(SamplePlayerController p, string paramName) : base(p, paramName,  Air.FALLING)
        {
        }
        public override void OnFixedUpdateState()
        {

        }

        protected override void initTransitions()
        {
            AddTransition(Air.GROUND, (p) =>
            {
                //Debug.Log("Testing Fall => Ground returning: " + p.Environment.GroundPresent);
                return p.Test2;
            });
        }
    }
    public class GroundState : PlayerState<Air>
    {

        public GroundState(SamplePlayerController p, string paramName) : base(p, paramName, Air.GROUND)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            if (_objectToInteract.Position == Positions.STANDING)
                _animator.SetTrigger("Movement");
            else
                _animator.SetTrigger("Position");
        }

        public override void OnFixedUpdateState()
        {

        }
        public override void OnExitState()
        {
            base.OnExitState();
            _animator.SetTrigger("Air");
        }
        protected override void initTransitions()
        {
            AddTransition(Air.AIR, () => _objectToInteract.Test3);
        }
    }
#if false
    public class CopyState : PlayerState<Air>
    {

        public CopyState(PlayerController p, string paramName, float exitTime) : base(p, paramName, exitTime, Air.)
        {
        }
        public override void OnUpdateState()
        {

        }

        protected override void initTransitions()
        {
            AddTransition(Air.AIR, );
            AddTransition(Air.FALLING, );
            AddTransition(Air.GROUND,)
        }
    }  
#endif
    public class PlayerAirStateMachine : PlayerStateMachine<Air>
    {
        protected override void InitStates()
        {
            AddMandatory(Air.AIR, () => false);
            AddState<GroundState>("IsGround");
            AddState<AirState>("IsAir",.1f);
            AddState<FallState>("IsFall");
            AddAnyStateTransition(Air.GROUND, (p) => p.Test1, false, System.Int32.MaxValue);
            AddAnyStateTransition(Air.FALLING, (p) => p.Test3 && !p.Test1, false, 0/*System.Int32.MaxValue*/);
        }
    }
}

