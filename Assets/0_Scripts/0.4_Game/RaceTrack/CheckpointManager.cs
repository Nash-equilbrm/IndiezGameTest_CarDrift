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

      

        private void Awake()
        {
            if(_checkpoints == null || _checkpoints.Count == 0)
            {
                FindCheckpoints();
            }
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

