using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasViewable : Viewable
    {

        private CanvasGroup CanvasGroup { get; set; }
        private RectTransform RectTransform { get; set; }
        public Sequence sequence;
        public Tween tween;
        public VectorOptions VectorOptions;
        


        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();
        }

        public override void Hide()
        {
            base.Hide();
            CanvasGroup.DOFade(0, 0.3f);
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        public override void Show()
        {
            base.Show();
            CanvasGroup.DOFade(1, 0.3f);
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }


    }
}
