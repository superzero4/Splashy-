using UnityEngine;

namespace StateMachinesExample
{
    public enum Positions
    {
        STANDING, CROUCH, COVER,
    }
    public class CrouchState : PlayerState<Positions>
    {

        public CrouchState(SamplePlayerController p, string paramName) : base(p, paramName, Positions.CROUCH)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            _objectToInteract.AdjustColliderSize(_objectToInteract.CrouchHeight);
        }
        public override void OnFixedUpdateState()
        {
        }
        protected override void initTransitions()
        {
            AddTransition(Positions.STANDING, () => _objectToInteract.Test2);
            AddTransition(Positions.COVER, (p) => CoverState.CheckForCover(p));
            //AddTransition(Positions.CROUCH, () => );
        }
    }
    public class StandingState : PlayerState<Positions>
    {
        public StandingState(SamplePlayerController p, string paramName) : base(p, paramName, Positions.STANDING)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            _objectToInteract.AdjustColliderSize(_objectToInteract.StandingHeight);
            if (_objectToInteract.Air == Air.GROUND)
                _animator.SetTrigger("Movement");
        }
        public override void OnFixedUpdateState()
        {

        }
        public override void OnExitState()
        {
            base.OnExitState();
            _animator.SetTrigger("Position");
        }
        protected override void initTransitions()
        {
            //Cover state also have mandatory requirements which also set the _cover variable
            AddTransition(Positions.COVER, (p) =>  CoverState.CheckForCover(p));        
            AddTransition(Positions.CROUCH, (p) => p.Test1);
        }
    }
    public class CoverState : PlayerState<Positions>
    {
        public static Ray? _coverNormal;
        public static bool CheckForCover(SamplePlayerController p)
        {
            if (p.Test1)
            {
                //Debug.Log("Setting ray to : " + r);
                _coverNormal = new Ray();
                return true;
            }
            else
                return false;
        }
        public static bool CheckForCornerCover(SamplePlayerController p,Ray currentCover,Ray? precedentRaycastResult)
        {
            if (precedentRaycastResult != null)
            {
                _coverNormal = precedentRaycastResult;
                return true;
            }
            //This part of code should theorically work (maybe need adjustements) for acute angles but it's mostly for right angle
            // considering the otbuse angle are taken care with the CoverLeft() function that passes his resut in this function and are treated above
            if (p.Test3)
            {
                //Debug.Log("Setting _cover to : " + _newCover);
                _coverNormal = new Ray();
                return true;
            }
            else
            {
                //Debug.Log("Didn't find new cover" + (_newCover==null)+" precedent got :"+(precedentRaycastResult));
                return false;
            }
        }
        //public float baseRange;
        public CoverState(SamplePlayerController p, string paramName) : base(p, paramName, Positions.COVER)
        {
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            _objectToInteract.AdjustColliderSize(_objectToInteract.CrouchHeight);
            Debug.Log(_coverNormal.Value.direction + "direction in enterstate");
            _objectToInteract.SnapToCover(_coverNormal.Value.origin, _coverNormal.Value.direction);
            //Debug.Break();
            /*baseRange=_environment.CoverDetectionRange;
            _environment.CoverDetectionRange = _environment.LeavingCoverDetectionRange;*/
        }
        public override void OnFixedUpdateState()
        {
            Vector3 vector3 = _objectToInteract.transform.position - _coverNormal.Value.direction * _objectToInteract.DistanceToCover;
            vector3.y = _coverNormal.Value.origin.y;
            //PlayerEnvironmentCheckers.CreateMarker("Ik",vector3);
        }
        public override void OnExitState()
        {
            base.OnExitState();
            //_coverNormal = null;
            //Debug.Break();
            //_objectToInteract.CurrentCover = null;
            //_environment.CoverDetectionRange = baseRange;
        }
        protected override void initTransitions()
        {
            AddTransition(Positions.STANDING, () => _objectToInteract.Test3);
            AddTransition(Positions.COVER, (p) =>  CheckForCornerCover(p,_coverNormal.Value,null));
            AddTransition(Positions.CROUCH, (p) => p.Test3 || p.Test2);
        }
    }
#if false
    public class Copy : PlayerState<Positions>
    {
        
        public Copy(PlayerController p, string paramName, float exitTime) : base(p, paramName, exitTime, Positions.)
        {
        }
        public override void OnUpdateState()
        {

        }

        protected override void initTransitions()
        {
            AddTransition(Positions.IDLE, () =>);
            AddTransition(Positions.COVER, () =>);
            AddTransition(Positions.CROUCH, () => );
        }
    }
#endif
    public class PlayerPositionsStateMachine : PlayerStateMachine<Positions>
    {
        protected override void InitStates()
        {
            /*AddMandatory(Positions.COVER, (p) =>
            {

            });*/
            AddState<StandingState>("IsStanding");
            AddState<CrouchState>("IsCrouch");
            AddState<CoverState>("IsCover");
        }
       
    }
}
