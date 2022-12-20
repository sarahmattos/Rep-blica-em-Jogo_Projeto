using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Game.Territorio
{
    public class Bairro : NetworkBehaviour
    {

        [SerializeField] private string nome;
        public NetworkVariable<int> playerIDNoControl = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public string Nome { get => nome; }

        private Material material;
        private TMP_Text text_nome;
        [SerializeField] private SetUpBairro setUpBairro;
        public SetUpBairro SetUpBairro { get => setUpBairro; } 
        public event Action playerControlMuda;


        private void Awake()
        {
            text_nome = GetComponentInChildren<TMP_Text>();
            material = GetComponentInChildren<MeshRenderer>().material;
            setUpBairro = GetComponentInChildren<SetUpBairro>();
            material.color = Color.gray;

            
        }

        private void OnEnable()
        {
            playerIDNoControl.OnValueChanged += onPlayerControlMuda;
        }

        private void OnDisable()
        {
            playerIDNoControl.OnValueChanged -= onPlayerControlMuda;
        }

        public void SetPlayerControl(int playerID)
        {
            playerIDNoControl.Value = playerID;
            setUpBairro.Eleitores?.MudaValorEleitores(1);
        }
        

        private void onPlayerControlMuda(int previousValue, int newValue)
        {
            material.color = GameDataconfig.Instance.PlayerColorOrder[newValue];
        }

        private void Start()
        {
            text_nome.SetText(Nome);

        }

        

    }

}
