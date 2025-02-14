using Commons;
using DG.Tweening;
using System;
using UnityEngine;


namespace Game.Car
{
    public class CarMovement : MonoBehaviour
    {
        public float acceleration = 10;
        public float maxSpeed = 15;
        public float drag = 0.98f;
        public float steerAngle = 20;
        public float traction = 1;
        public Transform coordinateUI;

        public Func<Vector2> onGetInputValue;
        private Vector2 _input;
        private Vector2 _lastInput;

        private Vector3 _directionVector;
        private Vector3 _velocity;

        public bool KeepGettingInput { get; internal set; }

        private void Update()
        {
            GetDirectionFromJoyStick();
            Move();
            Steer();
            ApplyTraction();
        }


        private void GetDirectionFromJoyStick()
        {
            if (coordinateUI is null || onGetInputValue is null) return;

            _input = KeepGettingInput ? onGetInputValue.Invoke() : Vector2.zero;

            Vector3 lookDirection;
            if (_input == Vector2.zero)
                lookDirection = Camera.main.transform.TransformDirection(_lastInput);
            else
            {
                lookDirection = Camera.main.transform.TransformDirection(_input);
                _lastInput = _input;
            }


            lookDirection.y = 0;
            coordinateUI.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            _directionVector = coordinateUI.forward;

        }


        private void Move()
        {
            if (!KeepGettingInput) return;
            // Newton's law
            _velocity += transform.forward * acceleration * Time.deltaTime;
            _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
            transform.position += _velocity * Time.deltaTime;
        }




        private void Steer()
        {
            if (_directionVector.sqrMagnitude > 0.01f
                && _directionVector != transform.forward)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_directionVector);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerAngle * Time.deltaTime);
                Drag();
            }
        }


        private void Drag()
        {
            _velocity *= drag;
            _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
        }


        private void ApplyTraction()
        {
            _velocity = Vector3.Lerp(_velocity.normalized, transform.forward, traction * Time.deltaTime) * _velocity.magnitude;
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _velocity );
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _directionVector * 3);
        }

    }
}
