using Game.Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.RaceTrack
{
    public class RaceTrackAnimation : MonoBehaviour
    {
        public Animator _startLineAnimator = null;


        public void PlayStartLineCountingAnim()
        {
            _startLineAnimator.Play(Constants.STR_START_LINE_COUNTING_ANIM);
        }
    }
}

