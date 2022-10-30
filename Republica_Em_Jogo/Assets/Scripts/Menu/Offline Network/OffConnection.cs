using Game.Player;
using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using UnityEngine;
using Logger = Game.Tools.Logger;

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
        private void Awake()
        {
            ipManager = GetComponent<IPManager>();

        }
        void Start()
        {

            ipAddressInput.onValueChanged.AddListener((string value) =>
            {
                PlayerPrefs.SetString("ipAddressJoin", value);
            });
            ipAddressInput.text = PlayerPrefs.GetString("ipAddressJoin");

            NetworkManager.Singleton.OnClientConnectedCallback += (ulong data) =>
            {
                Logger.Instance.LogInfo(string.Concat("jogador ",(int)data+1, " entrou na sala."));

                //IReadOnlyList<NetworkClient> clients = NetworkManager.Singleton.ConnectedClientsList;
                //foreach (NetworkClient client in clients)
                //{
                //    Logger.Instance.LogInfo(client.ClientId.ToString());
                //}


                if ((int)data + 1 == GameDataconfig.Instance.maxConnections)
                {
                    Logger.Instance.LogWarning(string.Concat("sala cheia. ", (int)data + 1, " jogadores nela."));
                    if(NetworkManager.Singleton.IsHost)
                    {
                        //PlayerPrefs.SetString("myIpAddress", IPManager.Instance.myIpAddress());
                        //NetworkManager.Singleton.NetworkConfig.
                        Logger.Instance.LogInfo("AGORA FOI VACILAO");
                    }
                }
            };

            NetworkManager.Singleton.OnTransportFailure += () =>
           {
               Logger.Instance.LogError("fALHO ao CRIAR HOST VACILÃO.");
           };


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





