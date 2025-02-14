using Commons;
using Game.Commons;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Game.Car
{
    public class CarController : MonoBehaviour
    {
        [SerializeReference] private CarMovement _movementController;

        public CarMovement MovementController { get => _movementController; set => _movementController = value; }

        private void OnEnable()
        {
            _movementController.enabled = false;

            this.Register(EventID.OnStartGameplay, OnStartGameplay);
            this.Register(EventID.OnFinishGame, OnFinishGame);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.OnStartGameplay, OnStartGameplay);
            this.Unregister(EventID.OnFinishGame, OnFinishGame);
        }

        private void OnStartGameplay(object obj)
        {
            _movementController.ResetMovement();
            _movementController.enabled = true;
        }

        private void OnFinishGame(object obj)
        {
            // stop engine
            _movementController.ResetMovement();
            _movementController.KeepGettingInput = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == Constants.STR_FINISH_LINE_TAG)
            {
                LogUtility.Info("CarController", "Hit Finish Line");
                this.Broadcast(EventID.OnHitFinishLine);
            }
        }
    }
}

