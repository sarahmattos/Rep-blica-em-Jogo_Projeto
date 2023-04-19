using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Tools
{
    public static class TasksExtensions
    {
        public static async Task SizeDeltaSmoothDamp(this RectTransform rectTransform, Vector2 target)
        {
            Vector2 velocityDamp = Vector2.one;
            float elapsedTime = 0f;
            float timeInterval = 0.5f;

            float multiply = 1.5f; //Quanto menor for o multiply, mais será a traiscao.

            while (elapsedTime < timeInterval)
            {
                rectTransform.sizeDelta = Vector2.SmoothDamp(
                    rectTransform.sizeDelta,
                    target,
                    ref velocityDamp,
                    timeInterval * timeInterval * multiply * Time.fixedDeltaTime
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }

        }

        public static async Task AlphaSmoothDamp(this CanvasGroup canvasGroup, float alpha)
        {
            float velocityDamp = 0;
            float elapsedTime = 0f;
            float timeInterval = 0.2f;

            while (elapsedTime < timeInterval)
            {
                canvasGroup.alpha = Mathf.SmoothDamp(
                    canvasGroup.alpha,
                    alpha,
                    ref velocityDamp,
                    timeInterval * timeInterval * 5 * Time.fixedDeltaTime
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }

        }

        public static async Task ScaleSmoothDamp(this Transform transform, Vector3 scale)
        {
            Vector3 currentVelocity = Vector3.zero;
            float elapsedTime = 0f;
            float timeInterval = 0.2f;

            while (elapsedTime < timeInterval)
            {
                transform.localScale = Vector3.SmoothDamp(
                    transform.localScale,
                    scale,
                    ref currentVelocity,
                    timeInterval * timeInterval * 5 * Time.fixedDeltaTime
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }

        }



    }
}
