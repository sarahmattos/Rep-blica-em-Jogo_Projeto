using Game.Player;
using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Logger = Game.Tools.Logger;
using UnityEngine.SceneManagement;
using Game.Tools;

namespace Game.Networking
{
    public class OfflineConnection : Singleton<OfflineConnection>
    {
        [SerializeField] private TMP_InputField ipAddressInput;
        private PlayerNameHandler playerNameHandler => PlayerNameHandler.Instance;
        public int clientsConnected => NetworkManager.Singleton.ConnectedClients.Count;
        public Action<bool> conexaoEstabelecida;
        public Action<string> conexaoIpEstabelecida;
        void Start()
        {
            ipAddressInput.onValueChanged.AddListener((string value) => { PlayerPrefs.SetString("ipAddressJoin", value); });
            ipAddressInput.text = PlayerPrefs.GetString("ipAddressJoin");

                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientdisconnect;
          
            NetworkManager.Singleton.OnTransportFailure += () =>
           {
               Logger.Instance.LogError("Falha ao criar conexão.");
           };
        }

        private void OnDisable()
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientdisconnect;
        }

        private void OnClientdisconnect(ulong data)
        {
        
        }

        private void OnClientConnect(ulong data)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            if (clientsConnected == GameDataconfig.Instance.MaxConnections)
            {
                LoadGameplayScene();
            }
        }


        public void LoadGameplayScene()
        {
            NetworkManager.Singleton.SceneManager.LoadScene(GameDataconfig.Instance.GameSceneName, LoadSceneMode.Single);
        }
  

        public void ConfigAndStartHostIP()
        {
            StartHostIP(playerNameHandler.GetPlayerName, IPManager.Instance.myIpAddress(), IPManager.Instance.portDefault);
        }

        public void ConfigAndStartClientIP()
        {

            StartClientIP(playerNameHandler.GetPlayerName, ipAddressInput.text, IPManager.Instance.portDefault);
        }



        public void StartClientIP(string playerName, string ipaddress, int port)
        {
            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(ipaddress, (ushort)port);
            SetConnectionPayload(GetPlayerId(), playerName);

            if(NetworkManager.Singleton.StartClient())
            {
                conexaoEstabelecida?.Invoke(true);
                conexaoIpEstabelecida?.Invoke(ipaddress);
                Logger.Instance.LogInfo(string.Concat("Conexão com o IP: ",ipAddressInput.text));
                
            } else
            {
                Logger.Instance.LogError(string.Concat("Falha na conexão com o IP: ", ipAddressInput.text));
                conexaoEstabelecida?.Invoke(false);
            }
        }


        public void StartHostIP(string playerName, string ipaddress, int port)
        {

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(ipaddress, (ushort)port);
            SetConnectionPayload(GetPlayerId(), playerName);
            if (NetworkManager.Singleton.StartHost())
            {
                conexaoEstabelecida?.Invoke(true);
                Logger.Instance.LogInfo("Criando sala Offline.");

            } else
            {
                conexaoEstabelecida?.Invoke(false);

                Logger.Instance.LogInfo("Falha ao criar sala.");
            }

        }

        private void SetConnectionPayload(string playerId, string playerName)
        {
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                playerId = playerId,
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


    [Serializable]
    public class ConnectionPayload
    {
        public string playerId;
        public string playerName;
        public bool isDebug;
    }


}





