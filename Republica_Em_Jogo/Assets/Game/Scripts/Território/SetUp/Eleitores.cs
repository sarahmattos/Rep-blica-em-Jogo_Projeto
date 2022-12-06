using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Game.Territorio
{
    public class Eleitores : NetworkBehaviour
    {
        public NetworkVariable<int> eleitores = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private TMP_Text text_eleitores;

        private void Awake()
        {
            text_eleitores = GetComponentInChildren<TMP_Text>();

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
        }

        public void MudaValorEleitores(int value)
        {
            //TODO: tratamento para evitar que o valor seja menor q 0;
            eleitores.Value += value;
        }
    }

}
