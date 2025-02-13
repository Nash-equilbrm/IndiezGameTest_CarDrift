using Game.Car;
using Patterns;
using System;
using UnityEngine;


namespace Game.CameraUtils
{
    public class SmoothFollowCamera : MonoBehaviour
    {
        private Transform _target; // Target to follow
        public Vector3 offset = new Vector3(0, 5, -10); // Offset from the target
        public float smoothSpeed = 5f; // Smoothness factor

        private void OnEnable()
        {
            this.Register(EventID.OnSpawnedCars, OnSpawnedCars);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.OnSpawnedCars, OnSpawnedCars);
        }

        private void OnSpawnedCars(object obj)
        {
            var tuple = (Tuple<object, object>)obj;
            _target = (tuple.Item1 as CarController).transform;
        }

        void LateUpdate()
        {
            if (_target == null) return;

            // Desired position
            Vector3 desiredPosition = _target.position + offset;

            // Smoothly move the camera
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Make the camera look at the target
            transform.LookAt(_target.position);
        }
    }
}
