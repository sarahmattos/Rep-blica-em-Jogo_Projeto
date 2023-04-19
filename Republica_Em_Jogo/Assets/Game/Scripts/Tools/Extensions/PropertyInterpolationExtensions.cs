using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Tools
{

    //Equacoes de suavizacao: https://docs.google.com/document/d/1z5l1M5Y23YGOXPalfq-Il0mTXhGWpzOSy4Fm2gJGUU8/edit?usp=sharing
    public static class PropertyInterpolationExtensions
    {
        public static async Task EaseOutQuadraticSizeDelta(this RectTransform rectTransform, Vector2 sizeDelta)
        {
            Vector2 start = rectTransform.sizeDelta;
            float elapsedTime = 0;
            float timeInterval = 0.5f;
            while (elapsedTime < timeInterval)
            {
                rectTransform.sizeDelta = Vector2.Lerp(start, sizeDelta, TimeInterpolation.EaseInOutQuad(elapsedTime, timeInterval));

                elapsedTime += (float)Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }

            rectTransform.sizeDelta = sizeDelta;
        }

        public static async Task EaseOutAlpha(this CanvasGroup canvasGroup, float alpha)
        {
            float start = canvasGroup.alpha;
            float elapsedTime = 0;
            float timeInterval = 0.5f;

            while (elapsedTime < timeInterval)
            {
                canvasGroup.alpha = Mathf.Lerp(start, alpha, TimeInterpolation.EaseOutQuad(elapsedTime, timeInterval));

                elapsedTime += (float)Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }

            canvasGroup.alpha = alpha;
        }

        public static async Task EaseOutScale(this Transform transform, Vector3 scale)
        {

            Vector3 start = transform.localScale;
            float elapsedTime = 0;
            float timeInterval = 0.1f;

            while (elapsedTime < timeInterval)
            {
                transform.localScale = Vector3.Lerp(start, scale, TimeInterpolation.EaseOutQuad(elapsedTime, timeInterval));

                elapsedTime += (float)Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }

            transform.localScale = scale;
        }





    }
}
