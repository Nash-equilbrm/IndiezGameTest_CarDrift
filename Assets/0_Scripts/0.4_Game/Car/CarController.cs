using Commons;
using System;
using UnityEngine;


namespace Game.Car
{
    public class CarController : MonoBehaviour
    {
        #region First version
        public float moveSpeed = 50;
        public float maxSpeed = 15;
        public float drag = 0.98f;
        public float steerAngle = 20;
        public float traction = 1;
        public Transform coordinateUI;

        public Func<Vector2> onGetInputValue;
        public Vector2 Input { get => onGetInputValue.Invoke(); }
        private Vector3 _directionVector;
        private Vector3 _moveForce;

        private void Update()
        {
            GetDirectionFromJoyStick();
            Move();
            Steer();
            Drag();
            ApplyTraction();
        }


        private void GetDirectionFromJoyStick()
        {
            if (coordinateUI is null || onGetInputValue is null) return;
            if (Input == Vector2.zero)
            {
                _directionVector = Vector2.zero;
                return;
            }
            var lookDirection = Camera.main.transform.TransformDirection(Input);
            lookDirection.y = 0;
            coordinateUI.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            _directionVector = coordinateUI.forward;
        }


        private void Move()
        {
            if (onGetInputValue == null) return;
            _moveForce += transform.forward * moveSpeed * _directionVector.z * Time.deltaTime;
            transform.position += _moveForce * Time.deltaTime;
        }


        private void Steer()
        {
            if (onGetInputValue == null) return;
            float steerInput = _directionVector.x;
            transform.Rotate(Vector3.up * steerInput * _moveForce.magnitude * steerAngle * Time.deltaTime);
        }


        private void Drag()
        {
            _moveForce *= drag;
            _moveForce = Vector3.ClampMagnitude(_moveForce, maxSpeed);
        }


        private void ApplyTraction()
        {
            _moveForce = Vector3.Lerp(_moveForce.normalized, transform.forward, traction * Time.deltaTime) * _moveForce.magnitude;
        }


        private void OnDrawGizmos()
        {
            if(onGetInputValue == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(coordinateUI.transform.position, coordinateUI.transform.position + _directionVector * 3);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _moveForce.normalized * 3);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.forward * 3);
        }
        #endregion
    }
}
