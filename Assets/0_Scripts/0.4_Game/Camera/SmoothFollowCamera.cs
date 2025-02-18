using Game.Car;
using Patterns;
using System;
using Commons;
using DG.Tweening;
using UnityEngine;


namespace Game.CameraUtils
{
    public class SmoothFollowCamera : MonoBehaviour
    {
        private Transform _target = null;
        public Vector3 offset = new Vector3(0, 5, -10); // Offset from the target
        public float smoothSpeed = 5f; // Smoothness factor
        
        private void OnEnable()
        {
            Sequence seq = DOTween.Sequence().AppendInterval(.5f).AppendCallback(RegisterEvents);
        }

        private void OnDisable()
        {
            this.PubSubUnregister(EventID.OnSpawnedGameobjects, OnSpawnedCars);
        }
        
        private void RegisterEvents()
        {
            LogUtility.Info("Camera", "RegisterEvents");
            this.PubSubRegister(EventID.OnSpawnedGameobjects, OnSpawnedCars);
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

            //transform.LookAt(_target.position);

            Vector3 direction = _target.position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
            }
        }

        // private IEnumerator IERegisterEvents()
        // {
        //     while (!PubSub.HasInstance)
        //     {
        //         yield return null;
        //     }
        //     this.PubSubRegister(EventID.OnSpawnedGameobjects, OnSpawnedCars);
        // }
        
       

    }
}
