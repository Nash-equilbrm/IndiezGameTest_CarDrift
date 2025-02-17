using Game.Commons;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Game.RaceTrack
{
    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> _checkpoints = new List<Transform>();
        public List<Transform> Checkpoints
        {
            get
            {
                return _checkpoints;
            }
        }

        private int _nextCheckpointIndex = 0;

        public int NextCheckpointIndex
        {
            get { return _nextCheckpointIndex; }
        }


        private void Awake()
        {
            if(_checkpoints == null || _checkpoints.Count == 0)
            {
                FindCheckpoints();
            }
            _nextCheckpointIndex = 0;
        }

        private void OnEnable()
        {
            this.Register(EventID.OnHitCheckpoint, OnHitCheckpoint);
            this.Register(EventID.OnStartInitGame, OnStartInitGame);
        }


        private void OnDisable()
        {
            this.Unregister(EventID.OnHitCheckpoint, OnHitCheckpoint);
            this.Unregister(EventID.OnStartInitGame, OnStartInitGame);
        }

        private void OnStartInitGame(object obj)
        {
            _nextCheckpointIndex = 0;
        }

        private void OnHitCheckpoint(object obj)
        {
            if (_nextCheckpointIndex == _checkpoints.Count - 1) return;
            _nextCheckpointIndex++;
        }

        private void FindCheckpoints()
        {
            _checkpoints.Clear();
            foreach (Transform child in transform)
            {
                if (child.CompareTag(Constants.STR_CHECKPOINT_TAG))
                {
                    _checkpoints.Add(child);
                }
            }
        }
    }
}

