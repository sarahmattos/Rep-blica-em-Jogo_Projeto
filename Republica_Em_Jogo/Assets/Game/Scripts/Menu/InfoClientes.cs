using TMPro;
using Unity.Netcode;

namespace Game.Networking
{

    public class InfoClientes :  NetworkBehaviour
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

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectionUpdate;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientConnectionUpdate;
            
        }

        private void OnClientConnectionUpdate(ulong obj)
        {
            if (!NetworkManager.Singleton.IsHost) return;
            clientesConnectados.Value = NetworkManager.Singleton.ConnectedClients.Count;
        }


        private void UpdatePlayerCount(int _, int next)
        {
            text_contagemJogadores.SetText(string.Concat(next, "/",GameDataconfig.Instance.MaxConnections," jogadores."));
        }

    }
}

