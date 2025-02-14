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
            if (_target != null) return;
            var tuple = (Tuple<object, object>)obj;
            _target = (tuple.Item1 as CarController).transform;
        }

        void LateUpdate()
        {
            if (_target == null) return;

            Vector3 desiredPosition = _target.position + offset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            transform.LookAt(_target.position);
        }
    }
}
