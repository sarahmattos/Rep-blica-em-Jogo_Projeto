using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasViewable : Viewable
    {

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Hide()
        {
            base.Hide(); 
            canvasGroup.alpha =0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public override void Show()
        {
            base.Show();
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }


    }
}
