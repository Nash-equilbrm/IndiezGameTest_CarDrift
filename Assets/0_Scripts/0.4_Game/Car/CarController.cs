using Commons;
using Game.Commons;
using Patterns;
using System;
using UnityEngine;
using UnityEngine.Windows;


namespace Game.Car
{
    public class CarController : MonoBehaviour
    {
        public CarMovement movementController = null;
        public Transform coordinateUI = null;
        public bool keepGettingInput = true;
        private Vector2 _input = Vector2.zero;
        private Vector2 _lastInput = Vector2.zero;

        public Func<Vector2> onGetInputValue = null;

        private void Start()
        {
            this.PubSubRegister(EventID.OnStartGameplay, OnStartGameplay);
            this.PubSubRegister(EventID.OnFinishGame, OnFinishGame);
        }

        private void OnDestroy()
        {
            this.PubSubUnregister(EventID.OnStartGameplay, OnStartGameplay);
            this.PubSubUnregister(EventID.OnFinishGame, OnFinishGame);
        }

        private void Update()
        {
            ControlCoordinate();
        }

        private void OnStartGameplay(object obj)
        {
            LogUtility.Info("OnStartGameplay" + gameObject.name, "ResetMovement");
            ResetController();
        }

        private void OnFinishGame(object obj)
        {
            // stop engine
            ResetController();
            keepGettingInput = false;
        }


      

        protected virtual void ControlCoordinate()
        {
            if (coordinateUI is null || onGetInputValue is null) return;
            _input = keepGettingInput ? onGetInputValue.Invoke() : Vector2.zero;

            Vector3 lookDirection;
            Vector2 input;
            if (_input == Vector2.zero)
            {
                input = _lastInput;
            }
            else
            {
                input = _input;
                _lastInput = _input;
            }
            lookDirection = Camera.main.transform.TransformDirection(input);
            lookDirection.y = 0;
            coordinateUI.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            movementController.DirectionVector = coordinateUI.forward;
        }


        protected virtual void ResetController()
        {
            movementController.ResetMovement();
            //_input = Vector2.zero;
            _lastInput = Vector2.zero;
            keepGettingInput = true;
        }
    }
}

