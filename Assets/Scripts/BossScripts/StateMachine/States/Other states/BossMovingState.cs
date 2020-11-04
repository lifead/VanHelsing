﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class BossMovingState : BossBaseState
    {

        #region Fields
        private const float ANGLE_SPEED = 100f;
        private const float ANGLE_RANGE = 5f;
        private const float DISTANCE_TO_START_EATING = 2.3f;

        private Vector3 _target;
        private Quaternion TargetRotation;

        #endregion


        #region ClassLifeCycle

        public BossMovingState(BossStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        #endregion

        #region Methods

        public override void OnAwake()
        {
            CanBeOverriden = true;
            CanExit = true;
           
        }

        public override void Initialise()
        {
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.WalkSpeed;
            _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_EATING;
            _stateMachine._model.BossAnimator.Play("MovingState");
            _target = _stateMachine._model.BossCurrentTarget;
        }

        public override void Execute()
        {
            if (!CheckDistance())
            {
                MoveTo();
                RotateTo();
            }
            else
            {
                _stateMachine.SetCurrentStateOverride(_stateMachine.LastStateType);
            }
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }


        private bool CheckDistance()
        {
            var isNear = _stateMachine._model.BossData.CheckIsNearTarget(_stateMachine._model.BossTransform.position, _stateMachine._model.BossTransform.rotation, _target, DISTANCE_TO_START_EATING, ANGLE_RANGE);
                return isNear;

        }
        private void MoveTo()
        {
            _stateMachine._model.BossData.MoveTo(_stateMachine._model.BossNavAgent, _target, _stateMachine._model.BossData._bossSettings.WalkSpeed);
        }

        private void RotateTo()
        {
           _stateMachine._model.BossTransform.rotation =  _stateMachine._model.BossData.RotateTo(_stateMachine._model.BossTransform.rotation, _stateMachine._model.BossTransform.position, _target, ANGLE_SPEED);
        }

        #endregion
    }
}