using Commons;
using Game.Car;
using Game.Commons;
using Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class OpponentAIController : CarController
    {
        public List<Transform> checkpoints = null;
        public float checkpointArrivalDistance = 5f;
        public float rotationDamping = 2f;
        public bool allowDrifting = true;

        [SerializeField] private int _currentCheckpointIndex = -1;
        [SerializeField] private Transform _currentCheckpoint = null;



        void GetNextCheckpoint()
        {
            _currentCheckpointIndex++;
            if (_currentCheckpointIndex >= checkpoints.Count)
            {
                _currentCheckpointIndex = -1;
                keepGettingInput = false;
                // this.PubSubBroadcast(EventID.OnHitFinishLine);
            }
            if(_currentCheckpointIndex < 0 ||  _currentCheckpointIndex >= checkpoints.Count) { return; }
            _currentCheckpoint = checkpoints[_currentCheckpointIndex];
        }


        protected override void ControlCoordinate()
        {
            if (checkpoints == null || checkpoints.Count == 0) { return; }

            if (_currentCheckpoint == null)
            {
                GetNextCheckpoint();
                if (_currentCheckpoint == null)
                {
                    keepGettingInput = false;
                    return;
                }
            }

            Vector3 targetDirection = _currentCheckpoint.position - transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            coordinateUI.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            movementController.DirectionVector = coordinateUI.forward;

            float distanceToCheckpoint = Vector3.Distance(transform.position, _currentCheckpoint.position);
            if (distanceToCheckpoint <= checkpointArrivalDistance)
            {
                GetNextCheckpoint();
            }
        }

        protected override void ResetController()
        {
            base.ResetController();
            _currentCheckpointIndex = -1;
            _currentCheckpoint = null;
        }
    }
}
