using DG.Tweening;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TweenOption<T> where T : struct
    {
        [SerializeField] private T tweenTypeData;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;


        public T TweenTypeData { get => tweenTypeData; set => tweenTypeData = value; }
        public float Duration { get => duration; set => duration = value; }
        public Ease Ease { get => ease;  set => ease = value; }

        //public TweenOption() { }
    }
}
