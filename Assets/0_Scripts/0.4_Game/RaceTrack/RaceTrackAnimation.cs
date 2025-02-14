using Game.Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.RaceTrack
{
    public class RaceTrackAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _startLineAnimator;


        internal void PlayStartLineCountingAnim()
        {
            _startLineAnimator.Play(Constants.STR_START_LINE_COUNTING_ANIM);
        }
    }
}

