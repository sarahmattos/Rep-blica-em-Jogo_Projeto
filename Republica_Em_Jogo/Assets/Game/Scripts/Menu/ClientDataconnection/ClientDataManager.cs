using Game.Networking;
using Game.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Game
{
    public  class ClientDataManager : NetworkSingleton<ClientDataManager>
    {
        private ClientsData clientsData = new ClientsData();
        public int LocalPlayerID => clientsData.Guids.IndexOf(PlayerDataPreferences.GetClientGUID());

        public void Start()
        {
            PlayerDataPreferences.InitializeGuid();

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCallback;

        }

        public override void OnDestroy()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCallback;
            base.OnDestroy();
        }

        private void ApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;

            string payloadDecoding = System.Text.Encoding.UTF8.GetString(request.Payload);
            ConnectionPayload connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payloadDecoding);
            clientsData.ConnectClient(connectionPayload);

        }



    }







    [Serializable]
    public class ClientsData
    {

        //O Id dos clients passam a ser o index dos Guids (ou clientsNames, tanto faz)
        
        public List<string> Guids { get; set; }
        public List<string> ClientNames { get; set; }

        public int GetClientIdByGuid(string guid) => Guids.IndexOf(guid);
        public int GetClientIdByName(string clientName) => Guids.IndexOf(clientName);

        public ClientsData()
        {
            Guids = new List<string>();
            ClientNames = new List<string>();
        }

        public void ConnectClient(ConnectionPayload payload)
        {
            Tools.Logger.Instance.LogInfo("payload guid: " + payload.guid);
            Tools.Logger.Instance.LogWarning("payload playerName: " + payload.playerName);
            //TODO: manipular e verificar os dados de conexão conforme planejado
            Guids.Add(payload.guid);
            ClientNames.Add(payload.playerName);

        }




    }
}
