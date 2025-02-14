using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Car
{
    public class CarVFX : MonoBehaviour
    {
        public ParticleSystem driftSmokeVFX;
        public ParticleSystem[] dirtTrailVFXs;
        public float minVelocityToDisplayDirt;
        [SerializeField] private CarMovement _carMovement;


        private void Update()
        {
            PlayDirtVFX();
            PlaySmokeVFX();
        }


        private void PlayDirtVFX()
        {
            if (_carMovement.CurrentSpeedSqr < minVelocityToDisplayDirt * minVelocityToDisplayDirt)
            {
                foreach (var dirtTrail in dirtTrailVFXs)
                {
                    dirtTrail.Stop();
                }
            }
            else
            {
                foreach (var dirtTrail in dirtTrailVFXs)
                {
                    if (!dirtTrail.isPlaying)
                    {
                        dirtTrail.Play();
                    }

                }
            }
        }


        private void PlaySmokeVFX()
        {
            if (_carMovement.IsDrifting && !driftSmokeVFX.isPlaying)
            {
                driftSmokeVFX.Play();
            }
        }
    }
}

