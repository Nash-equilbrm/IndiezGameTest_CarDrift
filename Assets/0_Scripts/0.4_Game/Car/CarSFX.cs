using Game.Audio;
using Game.Commons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Car
{
    public class CarSFX : MonoBehaviour
    {
        public AudioSource impactAudioSource;
        public float minVelocityToDisplayDirt = 0f;
        public CarMovement carMovement = null;
        public CarCollision carCollision = null;

        private void Start()
        {
            impactAudioSource.volume = Constants.AUDIO_DEFAULT_VOLUMN;
        }

        private void OnEnable()
        {
            carCollision.onCollision += PlayImpactSFX;
        }

        private void OnDisable()
        {
            carCollision.onCollision -= PlayImpactSFX;
        }


        private void PlayImpactSFX(Vector3 position)
        {
            AudioManager.Instance.PlaySFX(Constants.AUDIO_SFX_CAR_IMPACT, impactAudioSource, replayIfDuplicate: false);
        }
    }
}

