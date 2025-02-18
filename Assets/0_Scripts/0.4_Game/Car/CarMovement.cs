using Commons;
using UnityEngine;

namespace Game.Car
{
    public class CarMovement : MonoBehaviour
    {
        [LunaPlaygroundField("acceleration", 1, "Car Settings")] public float acceleration = 10;
        [LunaPlaygroundField("maxSpeed", 2, "Car Settings")] public float maxSpeed = 15;
        [LunaPlaygroundField("steerSpeed", 3, "Car Settings")] public float steerSpeed = 100;
        [LunaPlaygroundField("traction", 4, "Car Settings")] public float traction = 1;
        [LunaPlaygroundField("drag", 5, "Car Settings")] public float drag = .98f;
        [LunaPlaygroundField("driftAngleThreshold", 6, "Car Settings")] public float driftAngleThreshold = 10f;
        [LunaPlaygroundField("steeringThreshold", 7, "Car Settings")] public float steeringThreshold = 0.01f;

        public CarController carController = null;

        private Vector3 _directionVector = Vector3.zero;
        public Vector3 DirectionVector
        {
            get { return _directionVector; }
            set { _directionVector = value; }
        }

        public bool KeepGettingInput
        {
            get { return carController.keepGettingInput; }
        }

        private Vector3 _velocity = Vector3.zero;
        public Vector3 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }
        public float CurrentSpeedSqr
        {
            get { return _velocity.sqrMagnitude; }
        }

        public bool IsDrifting
        {
            get { return KeepGettingInput ? Vector3.Angle(_velocity, transform.forward) > driftAngleThreshold : false; }
        }

        private void OnEnable()
        {
            ResetMovement();
        }

        private void Update()
        {
            Move();
            Steer();
            ApplyTraction();
            if (!KeepGettingInput)
            {
                Drag();
            }
        }

        private void Move()
        {
            if (KeepGettingInput)
            {
                float turnAngle = Vector3.Angle(transform.forward, _directionVector);
                float turnFactor = Mathf.Clamp01(turnAngle / 90f);
                float adjustedAcceleration = acceleration * (1 - 0.5f * turnFactor);

                Vector3 accelerationVector = transform.forward * adjustedAcceleration;
                _velocity += accelerationVector * Time.deltaTime;

                if (_velocity.magnitude > maxSpeed)
                {
                    _velocity = _velocity.normalized * maxSpeed;
                }
                
                transform.position += _velocity * Time.deltaTime;
            }
        }

        private void Steer()
        {
            float angle = Vector3.Angle(transform.forward, _directionVector);
            Quaternion targetRotation = Quaternion.LookRotation(_directionVector);

            if (angle > 1f)
            {
                LogUtility.Info("Steering");
                Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.deltaTime);
                transform.rotation = newRotation;
                Drag();
            }
            else
            {
                LogUtility.Info("Stop Steering");
                transform.rotation = targetRotation;
            }
        }

        private void Drag()
        {
            _velocity *= drag;
            if (_velocity.sqrMagnitude < 0.01f)
            {
                _velocity = Vector3.zero;
            }
        }
        
        private void ApplyTraction()
        {
            _velocity = Vector3.Lerp(_velocity.normalized, transform.forward, traction * Time.deltaTime) * _velocity.magnitude;
        }
        

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _velocity);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _directionVector * 3);
        }

        internal void ResetMovement()
        {
            _velocity = Vector3.zero;
        }
    }
}