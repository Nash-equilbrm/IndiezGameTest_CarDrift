using UnityEngine;


namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseUIElement : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;
        protected UIType uiType = UIType.Unknow;
        protected bool isHide;
        private bool isInited;

        public bool IsHide
        {
            get { return isHide; }
        }

        public virtual void Init()
        {
            if (!isInited)
            {
                this.isInited = true;
                if (!this.gameObject.GetComponent<CanvasGroup>())
                {
                    this.gameObject.AddComponent<CanvasGroup>();
                }
                this.canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
                this.gameObject.SetActive(false);
            }
        }

        public virtual void Show(object data)
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
            this.isHide = false;
            SetActiveCanvasGroup(true);
        }

        public virtual void Hide()
        {
            this.isHide = true;
            SetActiveCanvasGroup(false);
        }


        private void SetActiveCanvasGroup(bool isActive)
        {
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = isActive;
                canvasGroup.alpha = isActive ? 1 : 0;
            }
        }
    }
}