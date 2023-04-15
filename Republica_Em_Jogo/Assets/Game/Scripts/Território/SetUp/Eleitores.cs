using TMPro;
using Unity.Netcode;
using UnityEngine;
using Game.UI;

namespace Game.Territorio
{
    public class Eleitores : NetworkBehaviour
    {
        private HudStatsJogador hs;
        private NetworkVariable<int> eleitores = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private TMP_Text text_eleitores;
        public int contaEleitores => eleitores.Value;
        public NetworkVariable<int> NumeroEleitores => eleitores;

        private void Awake()
        {
            text_eleitores = GetComponentInChildren<TMP_Text>();
            hs = FindObjectOfType<HudStatsJogador>();

        }

        private void OnEnable()
        {
            eleitores.OnValueChanged += onEleitoresMuda;

        }
        private void OnDisable()
        {
            eleitores.OnValueChanged -= onEleitoresMuda;

        }
        private void onEleitoresMuda(int previousValue, int newValue)
        {
            
            text_eleitores.SetText(newValue.ToString());
            hs.AtualizarPlayerStatsBairro();
        }

        
        [ServerRpc(RequireOwnership = false)]
        public void AcrescentaEleitorServerRpc(int value)
        {
            eleitores.Value += value;
        }
    }

}
