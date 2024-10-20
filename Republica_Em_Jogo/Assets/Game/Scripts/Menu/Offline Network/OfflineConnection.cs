using Game.Player;
using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Logger = Game.Tools.Logger;
using Game.Tools;

namespace Game.Networking
{
    public class OfflineConnection : Singleton<OfflineConnection>
    {
        [SerializeField] private TMP_InputField ipAddressInput;
        private PlayerNameHandler playerNameHandler => PlayerNameHandler.Instance;
        public Action<bool> conexaoEstabelecida;
        public Action<string> conexaoIpEstabelecida;
        void Start()
        {
            ipAddressInput.onValueChanged.AddListener((string value) => { PlayerPrefs.SetString("ipAddressJoin", value); });
            ipAddressInput.text = PlayerPrefs.GetString("ipAddressJoin");
          
        }



        public void ConfigAndStartHostIP()
        {
            StartHostIP(playerNameHandler.GetInputNameValue, IPManager.Instance.myIpAddress(), IPManager.Instance.portDefault);
        }

        public void ConfigAndStartClientIP()
        {

            StartClientIP(playerNameHandler.GetInputNameValue, ipAddressInput.text, IPManager.Instance.portDefault);
        }



        public void StartClientIP(string playerName, string ipaddress, int port)
        {
            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(ipaddress, (ushort)port);
            SetConnectionPayload( playerName);

            if(NetworkManager.Singleton.StartClient())
            {
                conexaoEstabelecida?.Invoke(true);
                conexaoIpEstabelecida?.Invoke(ipaddress);
                Logger.Instance.LogInfo(string.Concat("Conectando ao IP ",ipAddressInput.text));
                
            } else
            {
                Logger.Instance.LogError(string.Concat("Falha na conectar ao IP: ", ipAddressInput.text));
                conexaoEstabelecida?.Invoke(false);
            }
        }


        public void StartHostIP(string playerName, string ipaddress, int port)
        {

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(ipaddress, (ushort)port);
            SetConnectionPayload(playerName);
            if (NetworkManager.Singleton.StartHost())
            {
                conexaoEstabelecida?.Invoke(true);
            } else
            {
                conexaoEstabelecida?.Invoke(false);
                Logger.Instance.LogError("Falha ao criar sala.");
            }

        }

        private void SetConnectionPayload(string playerName)
        {
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                // playerId = playerId,
                playerName = playerName,
                isDebug = Debug.isDebugBuild
            });

            var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        }

        string GetPlayerId()
        {
            return UnityEngine.Random.Range(0, 1000).ToString();
   
        }

    }



    public class ConnectionPayload
    {
        // public string playerId;
        public string playerName;
        public bool isDebug;
    }


}





