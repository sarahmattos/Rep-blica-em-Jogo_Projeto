using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Game.Networking
{

    public class InfoClientes : NetworkBehaviour
    {
        private TMP_Text text_contagemJogadores;
        private NetworkVariable<int> clientesConnectados = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private void Awake()
        {
            text_contagemJogadores = GetComponentInChildren<TMP_Text>();
        }
        private void Start()
        {

            clientesConnectados.OnValueChanged += UpdatePlayerCount;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectionUpdate;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientConnectionUpdate;
        }

        public override void OnDestroy()
        {
            clientesConnectados.OnValueChanged -= UpdatePlayerCount;
            if(NetworkManager.Singleton == null) return;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectionUpdate;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientConnectionUpdate;
            StopAllCoroutines();
            clientesConnectados.Dispose();
        }

        private void OnClientConnectionUpdate(ulong obj)
        {
            if (!NetworkManager.Singleton.IsServer) return;

            StartCoroutine(AtualizaCountJogadoresAtrasado(0.1f));
        }

        private IEnumerator AtualizaCountJogadoresAtrasado(float s)
        {
            yield return new WaitForSeconds(s);
            clientesConnectados.Value = NetworkManager.Singleton.ConnectedClients.Count;
            yield return null;
        }

        private void UpdatePlayerCount(int _, int next)
        {
            text_contagemJogadores.SetText(string.Concat(next, "/", GameDataconfig.Instance.MaxConnections, " jogadores."));
        }

    }
}

