using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

namespace Game.Tools
{
    public static class AlterarPropriedades
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

        public static void NextValue(this int xis, int max)
        {
            if (xis < max)
            {
                xis = xis + 1;
                xis++;
            }
            else
            {
                xis = 0;
                xis++;
            }
        }


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
