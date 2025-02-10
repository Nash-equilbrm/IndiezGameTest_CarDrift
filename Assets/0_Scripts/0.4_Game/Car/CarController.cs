using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Car
{
    public class CarController : MonoBehaviour
    {

        public float moveSpeed = 50;
        public float maxSpeed = 15;
        public float drag = 0.98f;
        public float steerAngle = 20;
        public float traction = 1;

        private Vector3 _moveForce;

        private void Update()
        {
            Move();
            Steer();
            Drag();
            ApplyTraction();
        }


        private void Move()
        {
            _moveForce += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += _moveForce * Time.deltaTime;
        }


        private void Steer()
        {
            float steerInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * steerInput * _moveForce.magnitude * steerAngle * Time.deltaTime);
        }


        private void Drag()
        {
            _moveForce *= drag;
            _moveForce = Vector3.ClampMagnitude(_moveForce, maxSpeed);
        }


        private void ApplyTraction()
        {
            Debug.DrawRay(transform.position, _moveForce.normalized * 3);
            Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
            _moveForce = Vector3.Lerp(_moveForce.normalized, transform.forward, traction * Time.deltaTime) * _moveForce.magnitude;
        }
    }
}
