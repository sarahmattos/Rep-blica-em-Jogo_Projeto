using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Game.Networking
{
    public static class PlayerDataPreferences
    {
        public static string clientName
        {
            get => PlayerPrefs.GetString("ClientName");
            set => PlayerPrefs.SetString("ClientName", value);
        }

        public static string GetClientGUID()
        {
            return PlayerPrefs.GetString("clientGUID");
        }

        public static void InitializeGuid()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("clientGUID")))
            {
                Guid guid = Guid.NewGuid();
                PlayerPrefs.SetString("clientGUID", guid.ToString()) ;
                Tools.Logger.Instance.LogInfo("Gerando Guid: "+guid.ToString());
            }
        }

        public static string NewClientName =>   UnityEngine.Random.Range(0, 1000).ToString();




    }
}
