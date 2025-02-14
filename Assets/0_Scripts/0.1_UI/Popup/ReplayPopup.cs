using DG.Tweening;
using Patterns;
using System;
using UI;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    public class ReplayPopup : BasePopup
    {
        public Button playAgainBtn;
        public RectTransform resultPanel;
        public float animDuration;
        public Ease animEase;


        public override void Hide()
        {
            DoAnimationHide(() => base.Hide());
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            canvasGroup.alpha = 0;
            DoAnimationShow();
        }


        private void DoAnimationShow(Action onComplete = null)
        {
            var sq = DOTween.Sequence()
                .Join(canvasGroup.DOFade(1, animDuration).SetEase(animEase))
                .Join((playAgainBtn.transform as RectTransform).DOAnchorPos(Vector2.zero, animDuration).SetEase(animEase))
                .OnComplete(() => onComplete?.Invoke())
                .SetAutoKill(true);
        }

        private void DoAnimationHide(Action onComplete)
        {
            var sq = DOTween.Sequence()
                .Join(canvasGroup.DOFade(0, animDuration).SetEase(animEase))
                .Join((playAgainBtn.transform as RectTransform).DOAnchorPos(new(0, -100f), animDuration).SetEase(animEase))
                .OnComplete(() => onComplete?.Invoke())
                .SetAutoKill(true);
        }


        public void OnReplayBtnClicked()
        {
            this.Broadcast(EventID.OnReplayBtnClicked);
            Hide();
        }
    }
}

