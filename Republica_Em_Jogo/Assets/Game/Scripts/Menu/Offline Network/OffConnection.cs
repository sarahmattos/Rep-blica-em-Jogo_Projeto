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
    public class OffConnection : Singleton<OffConnection>
    {
        [SerializeField] private TMP_InputField ipAddressInput;
        //[SerializeField] private TMP_InputField portInput;
        private PlayerNameHandler playerNameHandler => PlayerNameHandler.Instance;
        //[SerializeField] private TMP_Text ipHostingGameText;

        //private IPManager ipManager => IPManager.Instance;
        private string IpHostingGame;

        public int clientsConnected => NetworkManager.Singleton.ConnectedClients.Count;

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
                Logger.Instance.LogInfo(string.Concat("Conexão com o IP: ",ipAddressInput.text));
                
            } else
            {
                Logger.Instance.LogError(string.Concat("Falha na conexão com o IP: ", ipAddressInput.text));
            }
        }


        public void StartHostIP(string playerName, string ipaddress, int port)
        {

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(ipaddress, (ushort)port);
            SetConnectionPayload(GetPlayerId(), playerName);
            if (NetworkManager.Singleton.StartHost())
            {
                Logger.Instance.LogInfo("Criando sala Offline.");
            } else
            {
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
            //if (UnityServices.State != ServicesInitializationState.Initialized)
            //{
            //    return ClientPrefs.GetGuid() + m_ProfileManager.Profile;
            //}

            //return AuthenticationService.Instance.IsSignedIn ? AuthenticationService.Instance.PlayerId : ClientPrefs.GetGuid() + m_ProfileManager.Profile;
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





