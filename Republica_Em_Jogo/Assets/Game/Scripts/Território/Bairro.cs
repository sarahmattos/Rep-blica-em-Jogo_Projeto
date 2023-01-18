using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Game.UI;

namespace Game.Territorio
{
    public class Bairro : NetworkBehaviour
    {

        [SerializeField] private string nome;
        public NetworkVariable<int> playerIDNoControl = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public string Nome { get => nome; }
        private HudStatsJogador hs;
        private Material material;
        private TMP_Text text_nome;
        [SerializeField] private SetUpBairro setUpBairro;
        public SetUpBairro SetUpBairro { get => setUpBairro; } 
        public event Action playerControlMuda;
        private Educaçao edu;
        private Saúde saude;


        private void Awake()
        {
            text_nome = GetComponentInChildren<TMP_Text>();
            material = GetComponentInChildren<MeshRenderer>().material;
            setUpBairro = GetComponentInChildren<SetUpBairro>();
            material.color = Color.gray;
            edu = GetComponentInChildren<Educaçao>();
            saude = GetComponentInChildren<Saúde>();
            hs = FindObjectOfType<HudStatsJogador>();
        }
        [ServerRpc(RequireOwnership = false)]
        public void MudaValorEleitorServerRpc()
        {
            setUpBairro.Eleitores.MudaValorEleitores(1);
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
            if(newValue == (int)NetworkManager.Singleton.LocalClientId){
                hs.AtualizarPlayerStatsBairro();
            }
        }

        private void Start()
        {
            text_nome.SetText(Nome);

        }

        public void VerificaRecurso(){
            if(playerIDNoControl.Value == (int)NetworkManager.Singleton.LocalClientId){
                edu.playerControlRecurso=true;
                saude.playerControlRecurso=true;
                Debug.Log("seu bairro");
            }else{
                edu.playerControlRecurso=false;
                saude.playerControlRecurso=false;
                Debug.Log("nao possui esse bairro");
            }
        }

        
        
        public void EscolherBairroEleitor(){
            if(playerIDNoControl.Value == (int)NetworkManager.Singleton.LocalClientId){
                hs.valorEleitorNovo();
                if(hs.eleitoresNovosAtual>0){
                    hs.atualizarEleitores();
                    if(NetworkManager.Singleton.IsClient){ 
                    MudaValorEleitorServerRpc();
                    }
                }
            }
        }
        
        
    }

}
