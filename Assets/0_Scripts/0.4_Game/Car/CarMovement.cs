using System;
using UnityEngine;

namespace Game.Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarMovement : MonoBehaviour
    {
        public float acceleration = 10;
        public float maxSpeed = 15;
        public float steerSpeed = 100;
        public float traction = 1;
        public float drag = .98f;
        public float driftAngleThreshold = 10f;

        public Transform coordinateUI;

        public Func<Vector2> onGetInputValue;
        private Vector2 _input;
        private Vector2 _lastInput;

        private Vector3 _directionVector;


        [SerializeField] private Rigidbody _rb;

        [field: SerializeField] public bool KeepGettingInput { get; internal set; } = false;
        public float CurrentSpeedSqr => _rb.velocity.sqrMagnitude;

        public bool IsDrifting => KeepGettingInput ? Vector3.Angle(_rb.velocity, transform.forward) > driftAngleThreshold : false;

        //public float curSpeed;

        private void Awake()
        {
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        

        private void Update()
        {
            GetDirectionFromJoyStick();
            Steer();
        }

        private void FixedUpdate()
        {
            Move();
            ApplyTraction();

            if (!KeepGettingInput)
            {
                Drag();
            }

            //curSpeed = _rb.velocity.magnitude;
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
            if (KeepGettingInput)
            {
                float turnAngle = Vector3.Angle(transform.forward, _directionVector);
                float turnFactor = Mathf.Clamp01(turnAngle / 90f);
                float radius = Mathf.Max(1f, _rb.velocity.magnitude / (steerSpeed * Mathf.Deg2Rad)); // Assume

                float centripetalForce = _rb.velocity.sqrMagnitude / radius;

                float adjustedAcceleration = acceleration * (1 - 0.5f * turnFactor);



                _rb.AddForce(transform.forward * adjustedAcceleration, ForceMode.Acceleration);

                if (_rb.velocity.magnitude > maxSpeed)
                {
                    _rb.velocity = _rb.velocity.normalized * maxSpeed;
                }
            }
        }

        private void Steer()
        {
            if (_directionVector.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_directionVector);

                if (Vector3.Angle(transform.forward, _directionVector) > 1f)
                {
                    Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.fixedDeltaTime);
                    _rb.MoveRotation(newRotation);
                }
            }
        }

        private void Drag()
        {
            _rb.velocity *= drag;
            if (_rb.velocity.sqrMagnitude < 0.01f)
            {
                _rb.velocity = Vector3.zero;
            }
        }

        private void ApplyTraction()
        {
            _rb.velocity = Vector3.Lerp(_rb.velocity.normalized, _directionVector, traction * Time.deltaTime) * _rb.velocity.magnitude;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _rb.velocity);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _directionVector * 3);
        }

        internal void ResetMovement()
        {
            _lastInput = Vector2.zero;
            _rb.velocity = Vector3.zero;
            KeepGettingInput = true;
        }
    }
}

