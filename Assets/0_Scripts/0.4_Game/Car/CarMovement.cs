using Commons;
using System;
using UnityEngine;

namespace Game.Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarMovement : MonoBehaviour
    {
        [LunaPlaygroundField("acceleration", 1, "Car Settings")] public float acceleration = 10;
        [LunaPlaygroundField("maxSpeed", 2, "Car Settings")] public float maxSpeed = 15;
        [LunaPlaygroundField("steerSpeed", 3, "Car Settings")] public float steerSpeed = 100;
        [LunaPlaygroundField("traction", 4, "Car Settings")] public float traction = 1;
        [LunaPlaygroundField("drag", 5, "Car Settings")] public float drag = .98f;
        [LunaPlaygroundField("driftAngleThreshold", 6, "Car Settings")] public float driftAngleThreshold = 10f;
        [LunaPlaygroundField("steeringThreshold", 7, "Car Settings")] public float steeringThreshold = 0.01f;
        public Rigidbody rb = null;

        public Transform coordinateUI = null;

        public Func<Vector2> onGetInputValue = null;
        private Vector2 _input = Vector2.zero;
        private Vector2 _lastInput = Vector2.zero;

        private Vector3 _directionVector = Vector3.zero;



        public bool keepGettingInput = false;
        public float CurrentSpeedSqr => rb.velocity.sqrMagnitude;

        public bool IsDrifting => keepGettingInput ? Vector3.Angle(rb.velocity, transform.forward) > driftAngleThreshold : false;

        //public float curSpeed;

        private void Awake()
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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

            if (!keepGettingInput)
            {
                Drag();
            }

            //curSpeed = _rb.velocity.magnitude;
        }

        private void GetDirectionFromJoyStick()
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
            _directionVector = coordinateUI.forward;
            //if(GameManager.Instance.logCar) LogUtility.Info("Carmovement Update", $"GetDirectionFromJoyStick : {input}");
        }

        private void Move()
        {
            if (keepGettingInput)
            {
                float turnAngle = Vector3.Angle(transform.forward, _directionVector);
                float turnFactor = Mathf.Clamp01(turnAngle / 90f);
                float radius = Mathf.Max(1f, rb.velocity.magnitude / (steerSpeed * Mathf.Deg2Rad)); // Assume

                float centripetalForce = rb.velocity.sqrMagnitude / radius;

                float adjustedAcceleration = acceleration * (1 - 0.5f * turnFactor);

                LogUtility.Info("Carmovement FixedUpdate", $"acceleration: {acceleration}");



                rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);

                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
                }
               
                LogUtility.Info("Carmovement FixedUpdate", $"rb.velocity.magnitude: {rb.velocity.magnitude}");
                LogUtility.Info("Carmovement FixedUpdate", $"rb.velocity: {rb.velocity}");
            }
        }

        private void Steer()
        {
            //if (_directionVector.sqrMagnitude > 0.01f)
            if(Vector3.Angle(_directionVector, transform.forward) > steeringThreshold)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_directionVector);
                if (Vector3.Angle(transform.forward, _directionVector) > 1f)
                {
                    Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.fixedDeltaTime);
                    rb.MoveRotation(newRotation);
                }
                else
                {
                    transform.rotation = targetRotation;
                }
                //if (GameManager.Instance.logCar) LogUtility.Info("Carmovement Update", $"Steering, angle = {Vector3.Angle(_directionVector, transform.forward)}");
            }
        }

        private void Drag()
        {
            rb.velocity *= drag;
            if (rb.velocity.sqrMagnitude < 0.01f)
            {
                rb.velocity = Vector3.zero;
            }
            //LogUtility.Info("Carmovement FixedUpdate", $"Drag: {rb.velocity}");
        }

        private void ApplyTraction()
        {
            rb.velocity = Vector3.Lerp(rb.velocity.normalized, _directionVector, traction * Time.deltaTime) * rb.velocity.magnitude;
            //if (GameManager.Instance.logCar) LogUtility.Info("Carmovement FixedUpdate", $"ApplyTraction: {rb.velocity}");
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(transform.position, transform.position + _directionVector * 3);
        //}

        public void ResetMovement()
        {
            _lastInput = Vector2.zero;
            rb.velocity = Vector3.zero;
            keepGettingInput = true;
        }
    }
}


//using Commons;
//using DG.Tweening;
//using System;
//using UnityEngine;


//namespace Game.Car
//{
//    public class CarMovement : MonoBehaviour
//    {
//        [LunaPlaygroundField("acceleration", 1, "Car Settings")] public float acceleration = 10;
//        [LunaPlaygroundField("maxSpeed", 2, "Car Settings")] public float maxSpeed = 15;
//        [LunaPlaygroundField("drag", 3, "Car Settings")] public float drag = 0.98f;
//        [LunaPlaygroundField("steerSpeed", 4, "Car Settings")] public float steerSpeed = 100;
//        [LunaPlaygroundField("traction", 5, "Car Settings")] public float traction = 0.01f;
//        [LunaPlaygroundField("driftAngleThreshold", 6, "Car Settings")] public float driftAngleThreshold = 20f;
//        public Transform coordinateUI;

//        public Func<Vector2> onGetInputValue;
//        private Vector2 _input;
//        private Vector2 _lastInput;

//        private Vector3 _directionVector;
//        private Vector3 _velocity;

//        public bool keepGettingInput;

//        public float CurrentSpeedSqr => _velocity.sqrMagnitude;
//        public bool IsDrifting => keepGettingInput ? Vector3.Angle(_velocity, transform.forward) > driftAngleThreshold : false;

//        private void Update()
//        {
//            GetDirectionFromJoyStick();
//            Steer();
//            ApplyTraction();
//        }

//        private void FixedUpdate()
//        {
//            Move();
//        }


//        private void GetDirectionFromJoyStick()
//        {
//            if (coordinateUI is null || onGetInputValue is null) return;

//            _input = keepGettingInput ? onGetInputValue.Invoke() : Vector2.zero;

//            Vector3 lookDirection;
//            if (_input == Vector2.zero)
//                lookDirection = Camera.main.transform.TransformDirection(_lastInput);
//            else
//            {
//                lookDirection = Camera.main.transform.TransformDirection(_input);
//                _lastInput = _input;
//            }


//            lookDirection.y = 0;
//            coordinateUI.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
//            _directionVector = coordinateUI.forward;

//        }


//        private void Move()
//        {
//            if (!keepGettingInput) return;
//            // Newton's law
//            _velocity += transform.forward * acceleration * Time.fixedDeltaTime;
//            _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
//            transform.position += _velocity * Time.fixedDeltaTime;
//        }




//        private void Steer()
//        {
//            if (_directionVector.sqrMagnitude > 0.01f
//                && _directionVector != transform.forward)
//            {
//                Quaternion targetRotation = Quaternion.LookRotation(_directionVector);
//                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.deltaTime);
//                Drag();
//            }
//        }


//        private void Drag()
//        {
//            _velocity *= drag;
//            _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
//        }


//        private void ApplyTraction()
//        {
//            _velocity = Vector3.Lerp(_velocity.normalized, transform.forward, traction * Time.deltaTime) * _velocity.magnitude;
//        }



//        private void OnDrawGizmos()
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawLine(transform.position, transform.position + _velocity);
//            Gizmos.color = Color.green;
//            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
//            Gizmos.color = Color.blue;
//            Gizmos.DrawLine(transform.position, transform.position + _directionVector * 3);
//        }

//        public void ResetMovement()
//        {
//            _lastInput = Vector2.zero;
//            _velocity = Vector3.zero;
//            keepGettingInput = true;
//        }

//    }
//}
