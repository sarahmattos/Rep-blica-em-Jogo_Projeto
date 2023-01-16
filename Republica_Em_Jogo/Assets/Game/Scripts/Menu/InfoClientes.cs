using TMPro;
using Unity.Netcode;
using UnityEngine;


namespace Game.Networking
{
    public class InfoClientes : MonoBehaviour
    {
        [SerializeField] private TMP_Text text_contagemJogadores;
        int clientsConnected => NetworkManager.Singleton.ConnectedClients.Count;
        
        private void Start()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClienteCallbackConnection;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClienteCallbackConnection;
            }

        }

        private void OnDisable()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClienteCallbackConnection;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClienteCallbackConnection;
            }
        }

        private void OnClienteCallbackConnection(ulong obj)
        {
            UpdatePlayerCount(clientsConnected);

        }


        private void UpdatePlayerCount(int value)
        {
            text_contagemJogadores.SetText(string.Concat(value, "/",GameDataconfig.Instance.MaxConnections," jogadores."));
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

