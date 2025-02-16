using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Car
{
    public class CarVFX : MonoBehaviour
    {
        public ParticleSystem driftSmokeVFX = null;
        public ParticleSystem[] dirtTrailVFXs = null;
        public float minVelocityToDisplayDirt = 0f;
        public CarMovement carMovement = null;


        private void Update()
        {
            PlayDirtVFX();
            PlaySmokeVFX();
        }


        private void PlayDirtVFX()
        {
            if (carMovement.CurrentSpeedSqr < minVelocityToDisplayDirt * minVelocityToDisplayDirt)
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
            if (carMovement.IsDrifting && !driftSmokeVFX.isPlaying)
            {
                driftSmokeVFX.Play();
            }
        }
    }
}

