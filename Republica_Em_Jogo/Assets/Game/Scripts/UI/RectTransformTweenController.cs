using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformTweenController : MonoBehaviour
    {
        private RectTransform rectTransform;
        private TweenOption<Vector3> defaultOption = new TweenOption<Vector3>();
        [SerializeField] private List<TweenOption<Vector3>> tweenOptions;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private TweenOption<Vector3> TryGetOption(int index)
        {
            if (tweenOptions.Count <= index) return defaultOption;
            return tweenOptions[index];
        }

        public void DoMoveOption(int index)
        {
            TweenOption<Vector3> option = TryGetOption(index);
            rectTransform.DOBlendableLocalMoveBy(option.TweenTypeData, option.Duration).SetEase(option.Ease);

        }

        public void DoScaleOption(int index)
        {
            TweenOption<Vector3> option = TryGetOption(index);
            rectTransform.DOScale(option.TweenTypeData, option.Duration).SetEase(option.Ease);
        }

        public void DoRotateOption(int index)
        {
            TweenOption<Vector3> option = TryGetOption(index);
            rectTransform.DORotate(option.TweenTypeData, option.Duration).SetEase(option.Ease);
        }



    }
}
