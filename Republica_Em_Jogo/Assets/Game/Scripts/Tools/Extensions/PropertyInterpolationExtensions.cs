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
                float t = TimeInterpolation.EaseInOutQuad(elapsedTime, timeInterval);
                rectTransform.sizeDelta = Vector2.Lerp(start, sizeDelta, t);

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
                float t = TimeInterpolation.EaseOutQuad(elapsedTime, timeInterval);
                canvasGroup.alpha = Mathf.Lerp(start, alpha, t);

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
                float t = TimeInterpolation.EaseOutQuad(elapsedTime, timeInterval);
                transform.localScale = Vector3.Lerp(start, scale, t);

                elapsedTime += (float)Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }

            transform.localScale = scale;
        }
        public static async Task EaseOutLocalPosition(this Transform transform, Vector3 targetPosition)
        {

            Vector3 start = transform.localPosition;
            float elapsedTime = 0;
            float timeInterval = 0.1f;

            while (elapsedTime < timeInterval)
            {
                float t = TimeInterpolation.EaseOutQuad(elapsedTime, timeInterval);
                transform.localPosition = Vector3.Lerp(start, targetPosition, t);

                elapsedTime += (float)Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }

            transform.localPosition = targetPosition;
        }


        public static async Task EaseOutColor(this Material material, Color finalColor, float timeInterval)
        {
            int PropertyToID = Shader.PropertyToID("_BaseColor");
            Color startColor = material.GetColor(PropertyToID);
            float elapsedTime = 0;

            while (elapsedTime < timeInterval)
            {
                float t = TimeInterpolation.EaseOutQuad(elapsedTime, timeInterval);
                material.SetColor(PropertyToID, Color.Lerp(startColor, finalColor, t));

                elapsedTime += (float)Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }

            material.SetColor(PropertyToID, finalColor);

        }




    }
}
