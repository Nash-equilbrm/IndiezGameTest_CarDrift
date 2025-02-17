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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == Constants.STR_FINISH_LINE_TAG)
            {
                LogUtility.Info("CarController" + gameObject.name, "Hit Finish Line");
                this.Broadcast(EventID.OnHitFinishLine);
            }
            else if (other.gameObject.tag == Constants.STR_CHECKPOINT_TAG)
            {
                LogUtility.Info("CarController" + gameObject.name, "Hit Checkpoint");
                this.Broadcast(EventID.OnHitCheckpoint);
            }
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag(Constants.STR_WALL_TAG))
        //    {
        //        movementController.OnCollistion(collision);
        //    }
        //}



        void OnCollisionEnter(Collision collision)
        {
            CarCollision otherCarCollision = collision.gameObject.GetComponent<CarCollision>();

            if (otherCarCollision != null)
            {
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.Normalize();

                rb.AddForce(-pushDirection * collisionForce, ForceMode.Impulse);
                Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();

                if (otherRb != null)
                {
                    otherRb.AddForce(pushDirection * collisionForce, ForceMode.Impulse);
                    if(onCollision != null)
                    {
                        onCollision.Invoke(collision.GetContact(0).point);
                    }
                }

                Debug.Log("Cars collided! Applying pushback.");
            }
        }
    }

}
