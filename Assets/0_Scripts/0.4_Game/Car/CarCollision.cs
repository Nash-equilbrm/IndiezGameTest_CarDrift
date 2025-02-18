using Commons;
using Game.Commons;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Car
{
    public class CarCollision : MonoBehaviour
    {
        [LunaPlaygroundField("collisionForce", 8, "Car Settings")] public float collisionForce = 10f; // Adjust this value
        public Rigidbody rb = null;
        public CarMovement movementController = null;
        public Action<Vector3> onCollision = null;
        [SerializeField] private bool _enableDetectingCollision = true;

        private void OnEnable()
        {
            this.PubSubRegister(EventID.OnStartGameplay, OnStartGameplay);
        }

        private void OnDisable()
        {
            this.PubSubUnregister(EventID.OnStartGameplay, OnStartGameplay);
        }

        private void OnStartGameplay(object obj)
        {
            _enableDetectingCollision = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_enableDetectingCollision) return;
            if (other.gameObject.tag == Constants.STR_FINISH_LINE_TAG)
            {
                this.PubSubBroadcast(EventID.OnHitFinishLine, this);
                _enableDetectingCollision = false;
            }
        }

        
        void OnCollisionEnter(Collision collision)
        {
            CarCollision otherCarCollision = collision.gameObject.GetComponent<CarCollision>();

            if (otherCarCollision != null)
            {
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.Normalize();

                CarMovement otherCarMovement = collision.gameObject.GetComponent<CarMovement>();

                if (movementController != null && otherCarMovement != null)
                {
                    movementController.Velocity -= pushDirection * collisionForce;
                    otherCarMovement.Velocity += pushDirection * collisionForce;

                    if (onCollision != null)
                    {
                        onCollision.Invoke(collision.GetContact(0).point);
                    }

                }
            }
        }
    }

}
