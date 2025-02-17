//using Commons;
//using System;
//using UnityEngine;

//namespace Game.Car
//{
//    [RequireComponent(typeof(Rigidbody))]
//    public class CarMovement : MonoBehaviour
//    {
//        [LunaPlaygroundField("acceleration", 1, "Car Settings")] public float acceleration = 10;
//        [LunaPlaygroundField("maxSpeed", 2, "Car Settings")] public float maxSpeed = 15;
//        [LunaPlaygroundField("steerSpeed", 3, "Car Settings")] public float steerSpeed = 100;
//        [LunaPlaygroundField("traction", 4, "Car Settings")] public float traction = 1;
//        [LunaPlaygroundField("drag", 5, "Car Settings")] public float drag = .98f;
//        [LunaPlaygroundField("driftAngleThreshold", 6, "Car Settings")] public float driftAngleThreshold = 10f;
//        [LunaPlaygroundField("steeringThreshold", 7, "Car Settings")] public float steeringThreshold = 0.01f;
//        //public float maxSpeedAfterCollision = 5f;
//        //public float pushForce = 1f;
//        public float dragCoefficient = 1f;
//        public float tractionCoefficient = 1f;

//        public Rigidbody rb = null;
//        public CarController carController = null;

//        private Vector3 _directionVector = Vector3.zero;
//        public Vector3 DirectionVector
//        {
//            get
//            {
//                return _directionVector;
//            }

//            set
//            {
//                _directionVector = value;
//            }
//        }



//        public bool KeepGettingInput {
//            get { return carController.keepGettingInput; }
//        }

//        public float CurrentSpeedSqr
//        {
//            get { return rb.velocity.sqrMagnitude; }
//        }


//        public bool IsDrifting
//        {
//            get { return KeepGettingInput ? Vector3.Angle(rb.velocity, transform.forward) > driftAngleThreshold : false; }
//        }


//        public float curSpeed;

//        private void Awake()
//        {
//            rb.interpolation = RigidbodyInterpolation.Interpolate;
//            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
//            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
//        }



//        private void Update()
//        {
//            Steer();
//            if (!KeepGettingInput)
//            {
//                ApplyDrag();
//            }
//        }


//        private void FixedUpdate()
//        {
//            Move();
//            ApplyTraction();
//            curSpeed = rb.velocity.magnitude;
//        }


//        private void Move()
//        {
//            if (KeepGettingInput)
//            {
//                float turnAngle = Vector3.Angle(transform.forward, _directionVector);
//                float turnFactor = Mathf.Clamp01(turnAngle / 90f);
//                float radius = Mathf.Max(1f, rb.velocity.magnitude / (steerSpeed * Mathf.Deg2Rad)); // Assume

//                float centripetalForce = rb.velocity.sqrMagnitude / radius;

//                float adjustedAcceleration = acceleration * (1 - 0.5f * turnFactor);

//                //LogUtility.Info("Carmovement FixedUpdate", $"acceleration: {acceleration}");



//                rb.AddForce(transform.forward * adjustedAcceleration, ForceMode.Acceleration);

//                if (rb.velocity.magnitude > maxSpeed)
//                {
//                    rb.velocity = rb.velocity.normalized * maxSpeed;
//                }


//                //LogUtility.Info("Carmovement FixedUpdate", $"rb.velocity.magnitude: {rb.velocity.magnitude}");
//                //LogUtility.Info("Carmovement FixedUpdate", $"rb.velocity: {rb.velocity}");
//            }
//        }

//        private void Steer()
//        {
//            if(Vector3.Angle(_directionVector, transform.forward) > steeringThreshold)
//            {
//                Quaternion targetRotation = Quaternion.LookRotation(_directionVector);
//                if (Vector3.Angle(transform.forward, _directionVector) > 1f)
//                {
//                    Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.fixedDeltaTime);
//                    rb.MoveRotation(newRotation);
//                }
//                else
//                {
//                    transform.rotation = targetRotation;
//                }
//            }
//        }

//        //private void ApplyDrag()
//        //{
//        //    rb.velocity *= drag;
//        //    if (rb.velocity.sqrMagnitude < 0.01f)
//        //    {
//        //        rb.velocity = Vector3.zero;
//        //    }
//        //}

//        //private void ApplyTraction()
//        //{
//        //    rb.velocity = Vector3.Lerp(rb.velocity.normalized, _directionVector, traction * Time.fixedDeltaTime) * rb.velocity.magnitude;
//        //}

//        //public void OnCollistion(Collision collision)
//        //{
//        //    rb.velocity *= 0.5f;
//        //    Vector3 normal = collision.contacts[0].normal;
//        //    rb.AddForce(normal * pushForce, ForceMode.Impulse);

//        //    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeedAfterCollision); // maxSpeedAfterCollision < maxSpeed
//        //}

//        private void ApplyDrag()
//        {
//            float dragForceMagnitude = rb.velocity.sqrMagnitude * dragCoefficient;
//            Vector3 dragForce = -rb.velocity.normalized * dragForceMagnitude;
//            rb.AddForce(dragForce, ForceMode.Acceleration);
//        }

//        private void ApplyTraction()
//        {
//            Vector3 forwardVelocity = Vector3.Project(rb.velocity, transform.forward);
//            Vector3 sidewaysVelocity = rb.velocity - forwardVelocity;

//            float tractionForceMagnitude = sidewaysVelocity.magnitude * tractionCoefficient;
//            Vector3 tractionForce = -sidewaysVelocity.normalized * tractionForceMagnitude;
//            rb.AddForce(tractionForce, ForceMode.Acceleration);
//        }

//        private void OnDrawGizmos()
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
//            Gizmos.color = Color.green;
//            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
//            Gizmos.color = Color.blue;
//            Gizmos.DrawLine(transform.position, transform.position + _directionVector * 3);
//        }

//        public void ResetMovement()
//        {
//            rb.velocity = Vector3.zero;
//        }
//    }
//}










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
        //public float maxSpeedAfterCollision = 5f;
        //public float pushForce = 1f;
        public float dragCoefficient = 1f;
        public float tractionCoefficient = 1f;

        public Rigidbody rb = null;
        public CarController carController = null;

        private Vector3 _directionVector = Vector3.zero;
        public Vector3 DirectionVector
        {
            get
            {
                return _directionVector;
            }

            set
            {
                _directionVector = value;
            }
        }



        public bool KeepGettingInput
        {
            get { return carController.keepGettingInput; }
        }

        public float CurrentSpeedSqr
        {
            get { return rb.velocity.sqrMagnitude; }
        }


        public bool IsDrifting
        {
            get { return KeepGettingInput ? Vector3.Angle(rb.velocity, transform.forward) > driftAngleThreshold : false; }
        }


        public float curSpeed;
        private void Awake()
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        private void OnEnable()
        {
            ResetMovement();
        }


        private void FixedUpdate()
        {
            Move();
            Steer();

            if (!KeepGettingInput)
            {
                Drag();
            }

            curSpeed = rb.velocity.magnitude;
        }


        private void Move()
        {
            if (KeepGettingInput)
            {
                float turnAngle = Vector3.Angle(transform.forward, _directionVector);
                float turnFactor = Mathf.Clamp01(turnAngle / 90f);
                float radius = Mathf.Max(1f, rb.velocity.magnitude / (steerSpeed * Mathf.Deg2Rad)); // Assume

                float centripetalForce = rb.velocity.sqrMagnitude / radius;

                float adjustedAcceleration = acceleration * (1 - 0.5f * turnFactor);



                rb.AddForce(transform.forward * adjustedAcceleration, ForceMode.Acceleration);

                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                }
            }
        }

        private void Steer()
        {
            float angle = Vector3.Angle(transform.forward, _directionVector);
            Quaternion targetRotation = Quaternion.LookRotation(_directionVector);

            if (angle > 1f) 
            {
                LogUtility.Info("Steering");
                Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(newRotation);
            }
            else
            {
                LogUtility.Info("Stop Steering");
                rb.rotation = targetRotation;
                rb.angularVelocity = Vector3.zero;
            }
        }

        private void Drag()
        {
            rb.velocity *= drag;
            if (rb.velocity.sqrMagnitude < 0.01f)
            {
                rb.velocity = Vector3.zero;
            }
        }

        

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _directionVector * 3);
        }

        internal void ResetMovement()
        {
            rb.velocity = Vector3.zero;
        }
    }
}
