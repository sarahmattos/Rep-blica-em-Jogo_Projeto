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
        // private Material material;
        public event Action playerControlMuda;
        public bool playerInControl=false;
        public bool bairroNaZonaEscolhida=false;
        private Interagivel interagivel;
        [SerializeField] private SetUpBairro setUpBairro;
        public SetUpBairro SetUpBairro { get => setUpBairro; }
        public Interagivel Interagivel => interagivel; 
        private HudStatsJogador hs;
        private Educaçao edu;
        private Saúde saude;

        private void Awake()
        {
            interagivel = GetComponentInChildren<Interagivel>();
            setUpBairro = GetComponentInChildren<SetUpBairro>();
            edu = GetComponentInChildren<Educaçao>();
            saude = GetComponentInChildren<Saúde>();
            hs = FindObjectOfType<HudStatsJogador>();
            // material = Interagivel.gameObject.GetComponent<MeshRenderer>().material;
        }
        [ServerRpc(RequireOwnership = false)]
        public void MudaValorEleitorServerRpc(int valor)
        {
            setUpBairro.Eleitores.MudaValorEleitores(valor);
            //hs.AtualizarPlayerStatsBairro();
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
            Interagivel.Material.color = GameDataConfig.Instance.PlayerColorOrder[newValue];

            //chama funcao pra atualizar bairro e eleitores na distribuicao inicial
            if(newValue == (int)NetworkManager.Singleton.LocalClientId){
                //hs.AtualizarPlayerStatsBairro();
            }
        }

        //verifica se bairro pertence ao jogador
        public bool VerificaControl(){
            if(playerIDNoControl.Value == (int)NetworkManager.Singleton.LocalClientId){
                playerInControl=true;
                //Debug.Log("seu bairro");
            }else{
                playerInControl=false;
                //Debug.Log("nao possui esse bairro");
            }
            return playerInControl;
        }

        //chamado pelo "MostrarNomeBairro" qnd clicado em um bairro
    public void EscolherBairroEleitor(){
        if(VerificaControl()){
            //retirar
            if(hs.playerDiminuiEleitor==true){
                if(setUpBairro.Eleitores.eleitores.Value>1){
                    if (NetworkManager.Singleton.IsClient) MudaValorEleitorServerRpc(-1);
                    //dimiui eleitor novo e aumenta eleito total
                    hs.contagemEleitores();
                    //recupera quantos eleitores novos
                    hs.valorEleitorNovo();
                    if(hs.eleitoresNovosAtual<1)hs.AtualizaUIAposDistribuicao();
                     //hs.AtualizarPlayerStatsBairro();
                    }
            }else{
                //colocar
                if(setUpBairro.Eleitores.eleitores.Value>0){
                    if (NetworkManager.Singleton.IsClient) MudaValorEleitorServerRpc(1);
                    hs.contagemEleitores();
                    hs.valorEleitorNovo();
                    if(hs.eleitoresNovosAtual<1) hs.AtualizaUIAposDistribuicao();
                    //hs.AtualizarPlayerStatsBairro();
                    }
            }
            
        }
    }
        
    }

}
