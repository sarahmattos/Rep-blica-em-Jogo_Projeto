using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;

namespace Game.Tools
{
    public static class Extensoes
    {
        public static void MudarCor(this Material material, Color color)
        {
            if (color == null) material.color = Color.gray;
            material.color = color;
        }

        private static System.Random rnd = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void ShufleNetworkList(this NetworkList<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void AddAll<T>(this IList<T> list, IList<T> newValues)
        {
            foreach (T values in newValues)
            {
                list.Add(values);
            }
        }

        public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
        {
            T key = default;
            foreach (KeyValuePair<T, W> pair in dict)
            {
                if (EqualityComparer<W>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }

        // public static async void SetFloatSmooth(this float current, float target) {

        //     Vector2 targetSizeDelta = T.sizeDelta;
        //     targetSizeDelta.x = width;
        //     Vector2 velocityDamp = Vector2.one;
        //     float elapsedTime = 0f;
        //     float timeInterval = 0.5f;

        //     while (elapsedTime < timeInterval)
        //     {
        //         rectTransform.sizeDelta = Vector2.SmoothDamp(
        //             rectTransform.sizeDelta,
        //             targetSizeDelta,
        //             ref velocityDamp,
        //             timeInterval*timeInterval*100*Time.deltaTime
        //         );

        //         elapsedTime += Time.deltaTime;
        //         await Task.Delay((int)(Time.deltaTime * 1000));
        //     }

        // } 


        public static async Task SizeDeltaSmoothDamp(this RectTransform rectTransform, Vector2 target) {
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
                    timeInterval*timeInterval*multiply*Time.fixedDeltaTime
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }

        } 


        public static async Task AlphaSmoothDamp(this CanvasGroup canvasGroup, float alpha) {
            float velocityDamp = 0;
            float elapsedTime = 0f;
            float timeInterval = 0.2f;

            while (elapsedTime < timeInterval)
            {
                canvasGroup.alpha = Mathf.SmoothDamp(
                    canvasGroup.alpha,
                    alpha,
                    ref velocityDamp,
                    timeInterval*timeInterval*5*Time.fixedDeltaTime
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }

        } 




        public static async Task ScaleSmoothDamp(this Transform transform, Vector3 scale) {
            Vector3 currentVelocity = Vector3.zero;
            float elapsedTime = 0f;
            float timeInterval = 0.2f;

            while (elapsedTime < timeInterval)
            {
                transform.localScale = Vector3.SmoothDamp(
                    transform.localScale,
                    scale,
                    ref currentVelocity,
                    timeInterval*timeInterval*5*Time.fixedDeltaTime
                );

                elapsedTime += Time.fixedDeltaTime;
                await Task.Delay((int)(Time.fixedDeltaTime * 1000));
            }
    
        }






        //public static void NextValue(this int value, int max)
        //{
        //   value = (1 + value) % max; 

        //}


        //public static void ShufleNetworkListPlayer(this NetworkList<PlayerData> list)
        //{
        //    int n = list.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = rnd.Next(n + 1);
        //        PlayerData value = list[k];
        //        list[k] = list[n];
        //        list[n] = value;
        //    }
        //}

        //public static void ShuffleNetworkList<T>(this NetworkList<T> list) where T : IReaderWriter
        //{
        //    int n = list.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = rnd.Next(n + 1);
        //        T value = list[k];
        //        list[k] = list[n];
        //        list[n] = value;
        //    }
        //}


    }
}
