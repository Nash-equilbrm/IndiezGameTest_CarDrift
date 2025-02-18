using Game.Commons;
using Patterns;
using UnityEngine;

namespace Game.Audio
{
    public class AudioController : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioManager.Instance.PlayBGM(Constants.AUDIO_BG_MUSIC);
            this.PubSubRegister(EventID.OnReplayBtnClicked, ReplayButtonClick);


            this.PubSubRegister(EventID.OnGameStartCounting, OnGameStartCounting);
            this.PubSubRegister(EventID.OnCounting, OnCounting);
            this.PubSubRegister(EventID.OnFinishCounting, OnFinishCounting);
            this.PubSubRegister(EventID.OnFinishGame, OnFinishGame);
        }

        private void OnDisable()
        {
            this.PubSubUnregister(EventID.OnReplayBtnClicked, ReplayButtonClick);


            this.PubSubUnregister(EventID.OnGameStartCounting, OnGameStartCounting);
            this.PubSubUnregister(EventID.OnCounting, OnCounting);
            this.PubSubUnregister(EventID.OnFinishCounting, OnFinishCounting);
            this.PubSubUnregister(EventID.OnFinishGame, OnFinishGame);
        }

        private void ReplayButtonClick(object obj)
        {
            AudioManager.Instance.PlaySFX(Constants.AUDIO_SFX_REPLAY_BUTTON);
        }


        private void OnCounting(object obj)
        {
            PlaySFX(Constants.AUDIO_SFX_COUNTING);
        }

        private void OnFinishCounting(object obj)
        {
            PlaySFX(Constants.AUDIO_SFX_FINISH_COUNTING);
        }

        private void OnFinishGame(object obj)
        {
            AudioManager.Instance.StopBGM();
            if (GameManager.Instance.PlayerWin)
            {
                PlaySFX(Constants.AUDIO_SFX_WIN);
            }
            else
            {
                PlaySFX(Constants.AUDIO_SFX_LOSE);
            }
        }

        private void OnGameStartCounting(object obj)
        {
            AudioManager.Instance.PlayBGM(Constants.AUDIO_BG_MUSIC);
            PlaySFX(Constants.AUDIO_SFX_START_COUNTING);
        }


        private void PlaySFX(string name)
        {
            AudioManager.Instance.PlaySFX(name, replayIfDuplicate: false);
        }
    }
}