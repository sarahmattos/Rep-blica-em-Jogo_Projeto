using TMPro;
using Unity.Netcode;
using UnityEngine;


namespace Game.Territorio
{
    public class Recursos : NetworkBehaviour
    {

        public NetworkVariable<int> saude = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<int> educacao = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        [SerializeField] private TMP_Text text_saude;
        [SerializeField] private TMP_Text text_edu;
        [SerializeField] private GameObject obj_saude;
        [SerializeField] private GameObject obj_edu;

        private void OnEnable()
        {
            saude.OnValueChanged += onSaudeMuda;
            saude.OnValueChanged += onEduMuda;

        }

        private void OnDisable()
        {
            saude.OnValueChanged -= onSaudeMuda;
            saude.OnValueChanged -= onEduMuda;
        }


        public void MudaValorSaude(int value)
        {
            saude.Value += value;
        }
        public void MudaValorEdu(int value)
        {
            saude.Value += value;
        }

        private void onSaudeMuda(int previousValue, int newValue)
        {
            text_saude.SetText(newValue.ToString());
        }
        private void onEduMuda(int previousValue, int newValue)
        {
            text_edu.SetText(newValue.ToString());
        }



    }

}
