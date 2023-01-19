using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
 using Game.Territorio;
 using Game.UI;

//namespace Game.Territorio
//{
    public class Projeto : NetworkBehaviour
    {
        private NetworkVariable<FixedString4096Bytes> projetoNetworkTexto = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<FixedString4096Bytes> zonaNetworkName = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<int> recompensaNetworkNum = new NetworkVariable<int>(-1);
        private NetworkVariable<int> idPlayer = new NetworkVariable<int>(-1);
        private NetworkVariable<int> votacaoRespostaFavor = new NetworkVariable<int>(0);
        private NetworkVariable<int> votacaoRespostaContra = new NetworkVariable<int>(0);
        private NetworkVariable<int> numPlayers = new NetworkVariable<int>(-1);

        private HudStatsJogador hs;
        public ProjetoObject projetoManager;
        private SetUpZona setUpZona;  
        private ZonaTerritorial zt;  

        [SerializeField] private TMP_Text text_projetoCarta;
        [SerializeField] private TMP_Text text_avisoProjeto;
        [SerializeField] private TMP_Text text_avisoOutros;
        
        public GameObject projetoUI;
        public GameObject restoUI;
        public GameObject bntsUi, btns2, fecharBtn;
        public GameObject verProjetoBtn;

        public string proposta;
        public int numRecompensa, numRecompensaDefault;
        public string recompensaText, zonaNameLocal;
        public int clienteLocal= -1;
        public int sim , nao, numPlayer;
        private string mostrarResposta;
        private bool mostrouResultado=false;
        public bool playerInZona=false;
        public bool distribuicaoProjeto=false;
    
        //Client cashing
        private string clientDados;
        private string textoTotal="";
        public void Awake(){
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            zt = GameObject.FindObjectOfType<ZonaTerritorial>();
            hs = FindObjectOfType<HudStatsJogador>();
        }

        //reseta valores para nova busca
        [ServerRpc(RequireOwnership = false)]
        public void DefaultValuesServerRpc()
        {
            recompensaNetworkNum.Value=-1;
            votacaoRespostaFavor.Value=0;
            votacaoRespostaContra.Value=0;
            projetoNetworkTexto.Value="";
            zonaNetworkName.Value="";
        }

        //pede pro host avaliar a votação
        [ServerRpc(RequireOwnership = false)]
        public void UpdateVotacaoServerRpc(int valor)
        {
            if(valor==0){
                votacaoRespostaFavor.Value++;
            }
            if(valor==1){
                votacaoRespostaContra.Value++;
            }
            
        }

        //host atualiza zona escolhida
        [ServerRpc(RequireOwnership = false)]
        public void UpdateZonaServerRpc(string clientDados)
        {
            zonaNetworkName.Value =clientDados;
        }
    
        //hosta atualiza a carta de projeto para os clientes
        [ServerRpc(RequireOwnership = false)]
        public void UpdateClientPositionServerRpc(string clientDados, int clientId, int num)
        {
        projetoNetworkTexto.Value = clientDados;
        recompensaNetworkNum.Value=num;
        idPlayer.Value= clientId;
        numPlayers.Value=NetworkManager.Singleton.ConnectedClientsIds.Count;
        }
        
        //sortea os valores do projeto
        public void sortearProjeto(){
            defaultValues();
            proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
            numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
            recompensaText = projetoManager.recompensaText;
            textoTotal= proposta +"\n"+ "\n"+ recompensaText+""+numRecompensa.ToString();
            atualizarProjeto(textoTotal);
            
        }
        //atualiza a carta de projeto ou pede pro host fazer isso
        public void atualizarProjeto(string textoTotal2){
            if(NetworkManager.Singleton.IsClient){  
                int id =(int)NetworkManager.Singleton.LocalClientId;
                UpdateClientPositionServerRpc(textoTotal2, id, numRecompensa);
                Debug.Log("cliente");
            }
            if (NetworkManager.Singleton.IsServer){
                projetoNetworkTexto.Value = textoTotal2;
                idPlayer.Value= (int)NetworkManager.Singleton.LocalClientId;
                recompensaNetworkNum.Value = numRecompensa;
                numPlayers.Value=NetworkManager.Singleton.ConnectedClientsIds.Count;
                Debug.Log("server");
                
            }
        }
        //verifica valores das variaves network se mudaram
        private void OnEnable()
        {
            //jogadores conectados
            numPlayers.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                numPlayer=newValue;
            };

            //id jogador
            idPlayer.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                //interface geral
                clienteLocal=newValue;
                projetoUI.SetActive(true);
                verProjetoBtn.SetActive(false);
                fecharBtn.SetActive(false);

                //interface para quem está escolhendo zona
                bntsUi.SetActive(true);
                text_avisoProjeto.text="Escolha uma zona:";

                //interface para quem está esperando zona ser escolhida
                if(newValue!=(int)NetworkManager.Singleton.LocalClientId){
                        bntsUi.SetActive(false);
                        text_avisoProjeto.text="Aguardando zona ser escolhida";
                }
            };
            //valor da recompensa
            recompensaNetworkNum.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                if(newValue!=-1){
                    numRecompensa= newValue;
                    numRecompensaDefault=numRecompensa;
                }
            };
            //texto do projeto
            projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
            {
                if(newValue!=""){
                    text_projetoCarta.text =  newValue.ToString();
                }
            };
            //zona escolhida
            zonaNetworkName.OnValueChanged += (FixedString4096Bytes  previousValue, FixedString4096Bytes  newValue) =>
            {
                zonaNameLocal=newValue.ToString();
                if(newValue!=""){

                    //desativa zonas para ser escolhidas
                    bntsUi.SetActive(false);
                    
                    //interface para quem escolheu zona e está esperando votação
                    if(clienteLocal==(int)NetworkManager.Singleton.LocalClientId){
                    text_avisoProjeto.text = "\n"+"\n"+"\n"+"Zona escolhida: "+newValue.ToString()+"\n"+"Aguardando votação... ";

                    //interface para quem está votando
                    }else{
                    text_avisoProjeto.text = "\n"+"\n"+"Zona escolhida: "+newValue.ToString()+"\n"+"Vote: ";
                    btns2.SetActive(true);
                    }
                }
                
            };
            
            //votacao a favor
            votacaoRespostaFavor.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                sim=newValue;
            };

            //votacao contra
            votacaoRespostaContra.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                nao=newValue;
            };
        }
        public void Update(){

            //quantidade de jogadores conectados
            if(numPlayer>1){

            //se todos votaram
                if(sim+nao>=numPlayer-1 ){

                    //desativa botão de votação
                    btns2.SetActive(false);
                    //ativa botao de fechar interface
                    fecharBtn.SetActive(true);

                    //se teve mais sim, foi aprovado
                    if(sim>nao){
                        text_avisoProjeto.text="\n"+"\n"+"\n"+"PROJETO APROVADO"+"\n"+"Recompensa: "+numRecompensaDefault+ " carta(s) e "+numRecompensaDefault+" eleitor(es)";
                        //parte da recompensa
                        eleitoresZonaFinal();
                    }

                    //se teve mais não ou empate, foi reprovado
                    if(nao>=sim){
                    text_avisoProjeto.text="\n"+"\n"+"\n"+"PROJETO NÃO APROVADO";
                    }
                }
            }
            
        }
        
        //chamado apos projeto ser aprovado
        public void eleitoresZonaFinal(){

            //adiciona eleitores aos jogadores que tem bairros da zona
             //if (NetworkManager.Singleton.IsServer){
                distribuicaoProjeto=true;
                setUpZona.eleitoresZona(numRecompensa, zonaNameLocal);
             //}
            
            //verifica se player tem bairro na zona escolhida
            setUpZona.playerZona(NetworkManager.Singleton.LocalClientId, zonaNameLocal);

            //dá carta de recurso para jogadores que possuem bairros na zona
            if(playerInZona==true){
                hs.updateRecursoCartaUI(numRecompensaDefault);
                playerInZona=false;
            }
            
            //reseta algumas variáveis
            zonaNameLocal="";
            clienteLocal=-1;
            numRecompensa=-1;
            
            }
            
         //ao apertar botao de fechar interface   
        public void fechar(){
            //desatuva interface
            fecharBtn.SetActive(false);
            projetoUI.SetActive(false);
            sim=0;
            nao=0;
        }

        //reseta variaveis oou pede pro hosta fazer isso
        public void defaultValues(){
            sim=0;
            nao=0;
            mostrouResultado=false;
            if (NetworkManager.Singleton.IsServer){
                recompensaNetworkNum.Value=-1;
                votacaoRespostaFavor.Value=0;
                votacaoRespostaContra.Value=0;
                projetoNetworkTexto.Value="";
                zonaNetworkName.Value="";
            }
            if(NetworkManager.Singleton.IsClient){ 
                DefaultValuesServerRpc();
            }
        }

        //funcao disparada ao apertar no nome de uma zona
        public void escolherZona(string zonaName){
            if (NetworkManager.Singleton.IsServer){
                zonaNetworkName.Value =zonaName;
            }
            if(NetworkManager.Singleton.IsClient){ 
                UpdateZonaServerRpc(zonaName);
            }
            
        }

        //funcao disparada ao apertar no botao de a favor ou contra a votação
        public void votacao(int resposta){

            if(NetworkManager.Singleton.IsClient){
                UpdateVotacaoServerRpc(resposta);
            }
            //texto interface recebe valores e botoees somem
            if(resposta==0)mostrarResposta="a favor";
            if(resposta==1)mostrarResposta="contra";
            btns2.SetActive(false);
            if(numPlayer>2){
                text_avisoProjeto.text = "\n"+"\n"+"\n"+"Seu partido votou "+mostrarResposta+"\n"+"Aguardando outros partidos...";
            }
        }
    }

