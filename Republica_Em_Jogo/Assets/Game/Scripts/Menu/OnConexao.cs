using Game.Tools;
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Networking
{

    public class OnConexao : Singleton<OnConexao>
    {
        [SerializeField] private TMP_Text textModoConexao;
        public int clientsConnected => NetworkManager.Singleton.ConnectedClients.Count;

        public event Action Disconnect;
        [SerializeField] private Color offlineModeColor;
        [SerializeField] private Color onlineModeColor;


        private void Start()
        {
            OfflineConnection.Instance.conexaoEstabelecida += OfflineConexao;
            OnlineConnection.Instance.conexaoEstabelecida += OnlineConexao;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            
            // NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;

        }

        private void OnDestroy()
        {

            OfflineConnection.Instance.conexaoEstabelecida -= OfflineConexao;
            OnlineConnection.Instance.conexaoEstabelecida -= OnlineConexao;
            if (NetworkManager.Singleton == null) return;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
        }

        private void OnClientConnect(ulong data)
        {


            TryLoadGameplayScene();
        }

        public void TryLoadGameplayScene()
        {
            //TODO: ver depois como desconectar e carregar cena inicial
            //https://docs-multiplayer.unity3d.com/netcode/current/basics/scenemanagement/using-networkscenemanager/index.html
            if (!NetworkManager.Singleton.IsHost) return;
            if (clientsConnected != GameDataconfig.Instance.MaxConnections) return;
            NetworkManager.Singleton.SceneManager.LoadScene(GameDataconfig.Instance.GameplaySceneName, LoadSceneMode.Single);

        }


        private void OfflineConexao(bool obj)
        {
            textModoConexao.SetText("LOCAL");
            textModoConexao.color = offlineModeColor;
        }

        private void OnlineConexao(bool obj)
        {
            textModoConexao.SetText("ONLINE");
            textModoConexao.color = onlineModeColor;

        }

        public void Disconectar()
        {
            NetworkManager.Singleton.Shutdown();
            Disconnect?.Invoke();
        }




        // private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        // {
        //     Debug.Log("aproval check");
        //     // The client identifier to be authenticated
        //     var clientId = request.ClientNetworkId;

        //     // Additional connection data defined by user code
        //     var connectionData = request.Payload;

        //     // Your approval logic determines the following values
        //     response.Approved = true;
        //     response.CreatePlayerObject = true;

        //     // The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
        //     response.PlayerPrefabHash = null;

        //     // Position to spawn the player object (if null it uses default of Vector3.zero)
        //     response.Position = Vector3.zero;

        //     // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        //     response.Rotation = Quaternion.identity;

        //     // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        //     // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        //     response.Reason = "Some reason for not approving the client";

        //     // If additional approval steps are needed, set this to true until the additional steps are complete
        //     // once it transitions from true to false the connection approval response will be processed.
        //     response.Pending = false;

        // }



    }
}
