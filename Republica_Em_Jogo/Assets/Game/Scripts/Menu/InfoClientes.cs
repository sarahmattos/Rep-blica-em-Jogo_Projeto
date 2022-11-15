using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Game.Tools;
using System.Linq;
using Logger = Game.Tools.Logger;

namespace Game.Networking
{
    public class InfoClientes : MonoBehaviour
    {
        [SerializeField] private TMP_Text text_contagemJogadores;
        IReadOnlyDictionary<ulong, NetworkClient> clientsConnected => NetworkManager.Singleton.ConnectedClients;
        
        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClienteCallbackConnection;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClienteCallbackConnection;
 
        }

        private void OnDisable()
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClienteCallbackConnection;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClienteCallbackConnection;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                Logger.Instance.LogInfo("LocalID: "+NetworkManager.Singleton.LocalClientId.ToString());
            }
            
        }

        private void OnClienteCallbackConnection(ulong obj)
        {
            UpdatePlayerCount(clientsConnected.Count);

        }


        private void UpdatePlayerCount(int value)
        {
            Tools.Logger.Instance.LogInfo(value + "pega.");
            text_contagemJogadores.SetText(string.Concat(value, "/",GameDataconfig.Instance.maxConnections," jogadores."));
        }

        //[ClientRpc]
        //private void UpdatePlayerListClientRpc()
        //{
        //    text_jogadoresListados.text = "";
        //    List<NetworkClient> networkClient = clientsConnected.Values.ToList();
            
        //    foreach (NetworkClient clients in networkClient)
        //    {
        //        text_jogadoresListados.text += clients.ClientId.ToString()+"\n";


        //    }
        //}

    }
}

