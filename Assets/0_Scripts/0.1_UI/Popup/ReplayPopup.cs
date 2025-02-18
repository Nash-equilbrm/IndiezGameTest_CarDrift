using DG.Tweening;
using Patterns;
using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    public class ReplayPopup : BasePopup
    {
        public TMP_Text resultText;
        public Button playAgainBtn;
        public RectTransform resultPanel;
        public float animDuration;
        public Ease animEase;

        private Action onComplete;

        public override void Hide()
        {
            DoAnimationHide(base.Hide);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            bool playerWin = (bool)data;
            resultText.text = playerWin ? "You Win" : "You Lose";
            canvasGroup.alpha = 0;
            DoAnimationShow();
        }


        private void DoAnimationShow(Action onComplete = null)
        {
            Sequence sq = DOTween.Sequence();
            this.onComplete = onComplete;

            sq.Join(canvasGroup.DOFade(1, animDuration).SetEase(animEase));

            RectTransform playAgainRect = playAgainBtn.transform as RectTransform;
            if (playAgainRect != null)
            {
                sq.Join(playAgainRect.DOAnchorPos(Vector2.zero, animDuration).SetEase(animEase));
            }

            sq.OnComplete(InvokeOnComplete);
            sq.SetAutoKill(true);
        }

        private void DoAnimationHide(Action onComplete)
        {
            Sequence sq = DOTween.Sequence();
            this.onComplete = onComplete;

            sq.Join(canvasGroup.DOFade(0, animDuration).SetEase(animEase));

            RectTransform playAgainRect = playAgainBtn.transform as RectTransform;
            if (playAgainRect != null)
            {
                sq.Join(playAgainRect.DOAnchorPos(new Vector2(0, -100f), animDuration).SetEase(animEase));
            }

            sq.OnComplete(InvokeOnComplete);
            sq.SetAutoKill(true);
        }


        public void OnReplayBtnClicked()
        {
            this.PubSubBroadcast(EventID.OnReplayBtnClicked);
            Hide();
        }

        private void InvokeOnComplete()
        {
            if (onComplete != null)
            {
                onComplete.Invoke();
            }
        }
    }
}

