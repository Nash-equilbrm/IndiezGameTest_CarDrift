using Commons;
using Game.Car;
using Game.Commons;
using Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class OpponentAI : MonoBehaviour
    {
        public CarMovement movementController = null;
        public CarController carController = null;
        public List<Transform> checkpoints = null;
        public float checkpointArrivalDistance = 5f;
        public float rotationDamping = 2f;
        public bool allowDrifting = true;

        private int _currentCheckpointIndex = 0;
        private Transform _currentCheckpoint = null;

        private Vector2 _aiInput;

        void Start()
        {
            if (movementController == null)
            {
                movementController = GetComponent<CarMovement>();
                if (movementController == null)
                {
                    enabled = false;
                    return;
                }
            }

            if (carController == null)
            {
                carController = GetComponent<CarController>();
                if (carController == null)
                {
                    enabled = false;
                    return;
                }
            }

            if (checkpoints == null || checkpoints.Count == 0)
            {
                enabled = false;
                return;
            }

            _currentCheckpoint = checkpoints[0];
            _currentCheckpointIndex = 0;

            movementController.onGetInputValue = GetAIInput;
            movementController.keepGettingInput = true;
        }

        void Update()
        {
            if (checkpoints == null || checkpoints.Count == 0) return;

            if (_currentCheckpoint == null)
            {
                GetNextCheckpoint();
                if (_currentCheckpoint == null)
                {
                    movementController.keepGettingInput = false;
                    return;
                }
            }

            Vector3 targetDirection = _currentCheckpoint.position - transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            _aiInput = CalculateAIInput(targetDirection);

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);

            float distanceToCheckpoint = Vector3.Distance(transform.position, _currentCheckpoint.position);
            if (distanceToCheckpoint <= checkpointArrivalDistance)
            {
                GetNextCheckpoint();
            }
        }

        void GetNextCheckpoint()
        {
            _currentCheckpointIndex++;
            if (_currentCheckpointIndex >= checkpoints.Count)
            {
                _currentCheckpointIndex = 0;
                //Debug.Log("OpponentAI: Đã đi hết checkpoints, quay lại điểm bắt đầu.");
                this.Broadcast(EventID.OnHitFinishLine);
            }

            _currentCheckpoint = checkpoints[_currentCheckpointIndex];
        }

        private Vector2 GetAIInput()
        {
            return _aiInput;
        }

        private Vector2 CalculateAIInput(Vector3 targetDirection)
        {
            float angle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

            float horizontalInput = Mathf.Clamp(-angle / 45f, -1f, 1f);
            float verticalInput = 1f;

            return new Vector2(horizontalInput, verticalInput);
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag(Constants.STR_WALL_TAG))
        //    {
        //        carController.MovementController.OnCollistion(collision);
        //    }
        //}
    }
}
