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
        private Material material;
        private TMP_Text text_nome;
        public event Action playerControlMuda;
        public bool playerInControl=false;
        public bool bairroNaZonaEscolhida=false;

        [SerializeField] private SetUpBairro setUpBairro;
        public SetUpBairro SetUpBairro { get => setUpBairro; } 

        private HudStatsJogador hs;
        private Educaçao edu;
        private Saúde saude;

        private void Update(){
            
        }

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
        public void MudaValorEleitorServerRpc(int valor)
        {
            setUpBairro.Eleitores.MudaValorEleitores(valor);
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
            material.color = GameDataConfig.Instance.PlayerColorOrder[newValue];

            //chama funcao pra atualizar bairro e eleitores na distribuicao inicial
            if(newValue == (int)NetworkManager.Singleton.LocalClientId){
                hs.AtualizarPlayerStatsBairro();
            }
        }

        private void Start()
        {
            text_nome.SetText(Nome);

        }

        //verifica se bairro pertence ao jogador
        public bool VerificaControl(){
            if(playerIDNoControl.Value == (int)NetworkManager.Singleton.LocalClientId){
                playerInControl=true;
                Debug.Log("seu bairro");
                Debug.Log("verificou control2");
            }else{
                playerInControl=false;
                Debug.Log("nao possui esse bairro");
            }
            return playerInControl;
        }

        //chamado pelo "MostrarNomeBairro" qnd clicado em um bairro
    public void EscolherBairroEleitor(){
        if(VerificaControl()){
            //retirar
            if(hs.playerDiminuiEleitor==true){
                if(setUpBairro.Eleitores.eleitores.Value>1){
                    //dimiui eleitor novo e aumenta eleito total
                    hs.contagemEleitores();
                    //recupera quantos eleitores novos
                    hs.valorEleitorNovo();
                    if(NetworkManager.Singleton.IsClient) MudaValorEleitorServerRpc(-1);
                    if(hs.eleitoresNovosAtual<1)hs.AtualizaUIAposDistribuicao();
                    
                }
            }else{
                //colocar
                if(setUpBairro.Eleitores.eleitores.Value>0){
                    hs.contagemEleitores();
                    hs.valorEleitorNovo();

                    if(NetworkManager.Singleton.IsClient) MudaValorEleitorServerRpc(1);
                    if(hs.eleitoresNovosAtual<1) hs.AtualizaUIAposDistribuicao();
                    
                }
            }
            
        }
    }
        
    }

}
