using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Car
{
    public class CarVFX : MonoBehaviour
    {
        public ParticleSystem driftSmokeVFX = null;
        public ParticleSystem[] dirtTrailVFXs = null;
        public ParticleSystem impactVFX = null;
        public float minVelocityToDisplayDirt = 0f;
        public CarMovement carMovement = null;
        public CarCollision carCollision = null;


        private void OnEnable()
        {
            carCollision.onCollision += PlayImpactVFX;
        }

        private void OnDisable()
        {
            carCollision.onCollision -= PlayImpactVFX;
        }

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

        private void PlayImpactVFX(Vector3 position)
        {
            impactVFX.transform.position = position;
            impactVFX.Play();
        }
    }
}

