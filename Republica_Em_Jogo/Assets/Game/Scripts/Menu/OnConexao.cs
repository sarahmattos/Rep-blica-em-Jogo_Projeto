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

        public event Action  Disconnect;
        [SerializeField] private Color offlineModeColor;
        [SerializeField] private Color onlineModeColor;


        private void Start()
        {
            OfflineConnection.Instance.conexaoEstabelecida += OfflineConexao;
            OnlineConnection.Instance.conexaoEstabelecida += OnlineConexao ;  
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        }

        private void OnDestroy()
        {
            OfflineConnection.Instance.conexaoEstabelecida -= OfflineConexao;
            OnlineConnection.Instance.conexaoEstabelecida -= OnlineConexao;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
        }

        private void OnClientConnect(ulong data)
        {
            if (!NetworkManager.Singleton.IsHost) return;
            if (clientsConnected == GameDataConfig.Instance.MaxConnections)
            {
                LoadGameplayScene();
            }
        }

        public void LoadGameplayScene()
        {
            //TODO: ver depois como desconectar e carregar cena inicial
            //https://docs-multiplayer.unity3d.com/netcode/current/basics/scenemanagement/using-networkscenemanager/index.html
            
            NetworkManager.Singleton.SceneManager.LoadScene(GameDataConfig.Instance.GameplaySceneName, LoadSceneMode.Single);
        }


        private void OfflineConexao(bool obj)
        {
            textModoConexao.SetText("OFFLINE");
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



    }
}
