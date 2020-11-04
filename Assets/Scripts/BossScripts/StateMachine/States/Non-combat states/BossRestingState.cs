﻿using UnityEngine;

namespace BeastHunter
{
    public class BossRestingState : BossBaseState
    {
        #region Constants

        private const float RESTING_TIME = 10f;
        private const float DISTANCE_TO_START_RESTING = 1.5f;

        #endregion


        #region Fields

        private Vector3 _target;
        private float _restingCountTime = RESTING_TIME;
        private bool _canRest;

        #endregion


        #region ClassLifeCycle

        public BossRestingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            CanExit = true;
            CanBeOverriden = true;
            IsBattleState = false;
        }

        public override void Initialise()
        {
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.WalkSpeed;
            _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_RESTING;
            _stateMachine._model.BossAnimator.Play("MovingState");
        }

        public override void Execute()
        {
            if (_target != null)
            {
                CheckTarget();
            }
            CheckDistance();
            Resting();
        }

        public override void OnExit()
        {

        }

        public override void OnTearDown()
        {

        }

        private void MoveToLair()
        {
            if (_stateMachine._model.Lair != null)
            {
                _stateMachine._model.BossData.MoveTo(_stateMachine._model.BossNavAgent, _target, _stateMachine._model.BossData._bossSettings.WalkSpeed);
                //_stateMachine._model.BossCurrentTarget = _stateMachine._model.Lair.transform.position;
                //_stateMachine.SetCurrentState(BossStatesEnum.Moving);
                
            }
            else
            {
                //find nearest tree and go there
            }
        }

        private void CheckTarget()
        {
            _target = _stateMachine._model.Lair.transform.position;
        }

        private void CheckDistance()
        {
            //if (Mathf.Sqrt((_stateMachine._model.BossTransform.position - _target)
            //    .sqrMagnitude) <= DISTANCE_TO_START_RESTING & _stateMachine._model.CurrentStamina >= _stateMachine._model.MaxStamina)

            if (_stateMachine._model.BossData.CheckIsNearTarget(_stateMachine._model.BossTransform.position, _stateMachine._model.BossTransform.rotation, _target, DISTANCE_TO_START_RESTING, 0) 
                & _stateMachine._model.CurrentStamina >= _stateMachine._model.MaxStamina)
            {
                _canRest = true;
            }
            else
            {
                _canRest = false;
                MoveToLair();
            }
        }

        private void Resting()
        {
            if (_canRest)
            {
                Debug.Log("Resting");
                _stateMachine._model.BossAnimator.Play("RestingState",0,0);
                _restingCountTime -= Time.deltaTime;
                if (_restingCountTime <= 0)
                {
                    _canRest = false;
                    _restingCountTime = RESTING_TIME;
                    _stateMachine._model.CurrentStamina = 0;
                    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Idle);
                }
            }
        }
        #endregion

    }
}