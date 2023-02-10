using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Game.UI;

namespace Game.Territorio
{
    
    public class Bairro :  NetworkBehaviour
    {

        [SerializeField] private string nome;
        private NetworkVariable<int> playerIDNoControl = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public string Nome { get => nome; }
        public NetworkVariable<int> PlayerIDNoControl => playerIDNoControl;
        [SerializeField] private Bairro[] vizinhos;
        public Bairro[] Vizinhos => vizinhos;
        public event Action playerControlMuda;
        public event Action<Bairro, int> bairroPlayerLocalForaControl;
        public event Action<Bairro, int> bairroPlayerLocalNoControl;
        public bool playerInControl=false;
        public bool bairroNaZonaEscolhida=false;
        private Interagivel interagivel;
        [SerializeField] private SetUpBairro setUpBairro;
        public SetUpBairro SetUpBairro { get => setUpBairro; }
        public Interagivel Interagivel => interagivel; 
        private HudStatsJogador hs;
        private Educaçao edu;
        private Saúde saude;

        private int auxContagem=0;

        private void Awake()
        {
            interagivel = GetComponentInChildren<Interagivel>();
            setUpBairro = GetComponentInChildren<SetUpBairro>();
            edu = GetComponentInChildren<Educaçao>();
            saude = GetComponentInChildren<Saúde>();
            hs = FindObjectOfType<HudStatsJogador>();
        }
        // private void Start()
        // {
        //     material = interagivel.Material;
        // }
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
            // Interagivel.Material.color = GameDataConfig.Instance.PlayerColorOrder[newValue];
            bairroPlayerLocalForaControl?.Invoke(this, previousValue);
            bairroPlayerLocalNoControl?.Invoke(this, newValue);

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
                if(setUpBairro.Eleitores.contaEleitores>1){
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
                if(setUpBairro.Eleitores.contaEleitores>0){
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
