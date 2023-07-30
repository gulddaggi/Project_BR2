using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public enum StateName
    {
        MOVE = 100,
        DASH,
        ATTACK,
    }

    public abstract class BaseState // 기본 스테이트. 타 스테이트 구현시 이 스테이트 상속받을 것
    {
        protected PlayerController Controller { get; private set; }

        public BaseState(PlayerController controller)
        {
            this.Controller = controller;
        }

        public abstract void OnEnterState(); // 스테이트 돌입하는 순간 실행
        public abstract void OnUpdateState(); // 현재 스테이트 내내 갱신
        public abstract void OnFixedUpdateState(); // 현재 스테이트 내 피직스 처리
        public abstract void OnExitState(); // 현재 스테이트에서 빠져나올 시 실행

    }

    #region * 스테이트 머신 - 스테이트 전환시에 사용
    public class StateMachine 
    {
        public BaseState CurrentState { get; private set; }  // 현재 스테이트
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>(); // 스테이트 딕셔너리로 받아옴


        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        public void AddState(StateName stateName, BaseState state)  // 스테이트 등록
        {
            if (!states.ContainsKey(stateName)) // 키 중복되는지 체크(ContainsKey(Key))
            {
                states.Add(stateName, state); // 키가 중복되지 않으면 새 스테이트 등록
            }
        }

        public BaseState GetState(StateName stateName)  // 스테이트 가져오기
        {
            if (states.TryGetValue(stateName, out BaseState state)) // 밸류값 가져오기. out 키워드를 이용해 변수추가선언없이 사용
                return state;
            return null;
        }

        public void DeleteState(StateName removeStateName)  // 스테이트 삭제
        {
            if (states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName)    // (중요!) 스테이트 전환
        {
            CurrentState?.OnExitState();   // 현재 스테이트를 종료
            if (states.TryGetValue(nextStateName, out BaseState newState)) // 밸류값을 가져와서 스테이트 전환
            {
                CurrentState = newState;
            }
            CurrentState?.OnEnterState(); 
        }

        public void UpdateState()
        {
            CurrentState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            CurrentState?.OnFixedUpdateState();
        }
    }
    #endregion

    #region * MOVE State
    public class MoveState : BaseState
    {
        public const float CONVERT_UNIT_VALUE = 0.01f;
        public const float DEFAULT_CONVERT_MOVESPEED = 3f;
        public const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
        private int hashMoveAnimation;

        public MoveState(PlayerController controller) : base(controller)
        {
            hashMoveAnimation = Animator.StringToHash("Velocity"); // 애니메이션 패러미터 접근. 현재로서는 미사용
        }

        protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
        {
            if (Controller.PlayerMoveDirection == Vector3.zero)
            {
                return -DEFAULT_ANIMATION_PLAYSPEED;
            }

            // (바뀐 이동 속도 - 기본 이동속도) * 0.1f
            return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.1f;
        }

        public override void OnEnterState()
        {
            // 필요없는 부분이지만, 추상 메소드는 구현해야 하므로 비어 둔다.
        }

        public override void OnUpdateState()
        {
            // 필요없는 부분이지만, 추상 메소드는 구현해야 하므로 비어 둔다.
        }

        // 이동은 Rigidbody 기반이므로, FixedUpdate()에서 구현해줍니다.
        public override void OnFixedUpdateState()
        {
            float currentMoveSpeed = Controller.player.MoveSpeed * CONVERT_UNIT_VALUE;
            float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED + GetAnimationSyncWithMovement(currentMoveSpeed);
            Controller.LookAt(Controller.PlayerMoveDirection);
            Player.Instance.rigidBody.velocity = Controller.PlayerMoveDirection * Controller.player.MoveSpeed + Vector3.up * Controller.PlayerRigid.velocity.y;
            Player.Instance.animator.SetFloat(hashMoveAnimation, animationPlaySpeed);
        }

        // 이동 상태를 종료할 때는 애니메이션과 물리 속도를 초기화 해줘야 한다.
        public override void OnExitState()
        {
            Player.Instance.animator.SetFloat(hashMoveAnimation, 0f);
            Player.Instance.rigidBody.velocity = Vector3.zero;
        }
    }

    #endregion

    #region * Dash State

    public class DashState : BaseState
    {
        public int CurrentDashCount { get; set; } = 0;
        public bool CanAddInputBuffer { get; set; }     // 플레이어 버퍼 입력 가부
        public bool CanDashAttack { get; set; }
        public bool IsDash { get; set; }
        public int Hash_DashTrigger { get; private set; }
        public int Hash_IsDashBool { get; private set; }
        public int Hash_DashPlaySpeedFloat { get; private set; }
        public Queue<Vector3> inputDirectionBuffer { get; private set; }

        public const float DEFAULT_ANIMATION_SPEED = 2f;
        public readonly float dashPower;
        public readonly float dashTetanyTime;
        public readonly float dashCooltime;

        public DashState(PlayerController controller, float dashPower, float dashTetanyTime, float dashCoolTime) : base(controller)
        {
            inputDirectionBuffer = new Queue<Vector3>();
            this.dashPower = dashPower;
            this.dashTetanyTime = dashTetanyTime;
            this.dashCooltime = dashCoolTime;
            Hash_DashTrigger = Animator.StringToHash("Dash");
            Hash_IsDashBool = Animator.StringToHash("IsDashing");
            Hash_DashPlaySpeedFloat = Animator.StringToHash("DashPlaySpeed");
        }

        public override void OnEnterState()
        {
            IsDash = true;
            CanAddInputBuffer = false;
            CanDashAttack = false;
            Player.Instance.animator.applyRootMotion = false;
            Dash();
        }

        private void Dash()
        {
            Vector3 dashDirection = inputDirectionBuffer.Dequeue();
            dashDirection = (dashDirection == Vector3.zero) ? Player.Instance.transform.forward : dashDirection;

            Player.Instance.animator.SetBool(Hash_IsDashBool, true);
            Player.Instance.animator.SetTrigger(Hash_DashTrigger);
            // Player.Instance.PlayerController.LookAt(new Vector3(dashDirection.x, 0f, dashDirection.z));

            float dashAnimationPlaySpeed = DEFAULT_ANIMATION_SPEED + (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE - MoveState.DEFAULT_CONVERT_MOVESPEED) * 0.1f;
            Player.Instance.animator.SetFloat(Hash_DashPlaySpeedFloat, dashAnimationPlaySpeed);
            Player.Instance.rigidBody.velocity = dashDirection * (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE) * dashPower;
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            Player.Instance.rigidBody.velocity = Vector3.zero;
            Player.Instance.animator.applyRootMotion = true;
            Player.Instance.animator.SetBool(Hash_IsDashBool, false);
        }
    }

    #endregion
}