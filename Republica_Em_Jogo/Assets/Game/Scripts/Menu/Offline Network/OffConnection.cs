using Game.Player;
using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Logger = Game.Tools.Logger;
using UnityEngine.SceneManagement;

namespace Game.Connection
{
    [RequireComponent(typeof(IPManager))]
    public class OffConnection : MonoBehaviour
    {
        [SerializeField] private TMP_InputField ipAddressInput;
        [SerializeField] private TMP_InputField portInput;
        [SerializeField] private PlayerNameHandler playerNameHandler;
        private IPManager ipManager;
        private string IpHostingGame;
        [SerializeField] private TMP_Text ipHostingGameText;

        public int clientsConnected => NetworkManager.Singleton.ConnectedClients.Count;
        private void Awake()
        {
            ipManager = GetComponent<IPManager>();

        }

        void Start()
        {

            ipAddressInput.onValueChanged.AddListener((string value) => { PlayerPrefs.SetString("ipAddressJoin", value); });
            ipAddressInput.text = PlayerPrefs.GetString("ipAddressJoin");
            //if(NetworkManager.Singleton.IsHost)
            //{
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientdisconnect;
            //}

            NetworkManager.Singleton.OnTransportFailure += () =>
           {
               Logger.Instance.LogError("fALHO ao CRIAR HOST.");
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

            NetworkManager.Singleton.StartClient();
        }


        public void StartHostIP(string playerName, string ipaddress, int port)
        {

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(ipaddress, (ushort)port);
            SetConnectionPayload(GetPlayerId(), playerName);
            if (NetworkManager.Singleton.StartHost())
            {
                IpHostingGame = ipaddress;
                ipHostingGameText.SetText(IpHostingGame);
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





