using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

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

    }
    #endregion
}