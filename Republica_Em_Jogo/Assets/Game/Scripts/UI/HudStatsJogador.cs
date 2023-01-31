using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Player;
 using Game.Territorio;
 using Game;
using Logger = Game.Tools.Logger;

namespace Game.UI
{
    public class HudStatsJogador : NetworkBehaviour
    {
        public GameObject button; //botao para testar

        [SerializeField] private Image iconJogador;
        [SerializeField] private TMP_Text text_nomeJogador;
        [SerializeField] private TMP_Text text_eleitores;
        [SerializeField] private TMP_Text text_objetivo;
        [SerializeField] private TMP_Text text_distribuaEleitor;
        [SerializeField] private TMP_Text text_distribuaEleitorFinal;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private TMP_Text text_saudeCarta;
        [SerializeField] private TMP_Text text_eduCarta;
        [SerializeField] private TMP_Text text_bairros;
        [SerializeField] private TMP_Text text_eleitoresNovos;
        [SerializeField] private GameObject acaboudistribuicaoUi;
        [SerializeField] private GameObject acaboudistribuicaoUi2;
        [SerializeField] private GameObject distribuaEleitorUi;
        //variaveis que passam valores para classe playerStats
        public int eduQuant, saudeQuant, bairroQuant; 
        public float eleitoresNovosAtual;
        private Projeto projeto; 
        private SetUpZona setUpZona; 
        public string textToDisplayEleitores => string.Concat("Eleitores: ", playerStats.EleitoresTotais);
        public bool playerRecebeEleitor=true;
        public bool playerDiminuiEleitor=false;
        public bool distribuicaoGeral=false;
        public bool distribuicaoInicial=false;
        [SerializeField] private State state;

        public ControlePassarState cp;

        
        public override void OnNetworkSpawn()
        {
            state = GameStateHandler.Instance.StatePairValue[GameState.INICIALIZACAO];
            GameStateHandler.Instance.StatePairValue[GameState.INICIALIZACAO].Entrada += FindingLocalPlayerStats;
            projeto = FindObjectOfType<Projeto>();
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.StatePairValue[GameState.INICIALIZACAO].Entrada -= FindingLocalPlayerStats;
        }

        public override void OnDestroy()
        {
            TurnManager.Instance.vezDoPlayerLocal -= (bool value) => { button.SetActive(value); };

        }

        private void FindingLocalPlayerStats()
        {
            PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            foreach (PlayerStats stats in allPlayerStats)
            {
                if (stats.IsLocalPlayer)
                {
                    playerStats = stats;
                    InitializeHudStats();
                }
            }
        }

        private void InitializeHudStats()
        {
            iconJogador.color = playerStats.Cor;
            text_nomeJogador.SetText(playerStats.Nome);
            text_eleitores.SetText(textToDisplayEleitores);
            text_objetivo.SetText(playerStats.ObjetivoCarta);
            
        }

        //distribui carta de recurso (chamada pela classe projeto)
        public void updateRecursoCartaUI(int quantidade)
        {   
            if(quantidade>-1){

                //chama a funcao que randomiza
                playerStats.recursoDistribuicao(quantidade);

                //muda a interface
                text_saudeCarta.SetText("Saúde: "+playerStats.numSaude.ToString());
                text_eduCarta.SetText("Edu: "+playerStats.numEducacao.ToString());

                //pega os valores da classe player stats, mais tarde é usado na troca
                atualizarRecursoAntesTroca();
            }
        }
        public void atualizarRecursoAntesTroca(){
            saudeQuant=playerStats.numSaude;
            eduQuant=playerStats.numEducacao;
        }
        // chamada dps de troca de cartas pelo script: RecursosCartaManager
        public void atualizarRecursoAposTroca(){

            //valores do playerstats recebem o valor de volta
            playerStats.numSaude=saudeQuant;
            playerStats.numEducacao=eduQuant;
            
            //interface muda
            text_saudeCarta.SetText("Saúde: "+playerStats.numSaude.ToString());
            text_eduCarta.SetText("Edu: "+playerStats.numEducacao.ToString());
        }
        
        //a distribuicao inicial dos bairros atualiza o valor de bairros e eleitores totais
        public void AtualizarPlayerStatsBairro(){
            AtualizaBairros();
            AtualizaEleitores();
        }
        
        //atualiza texto eleitores
         public void AtualizaEleitores(){
            
            if(playerDiminuiEleitor==true){
                playerStats.eleitoresDiminuir();
            }else{
                playerStats.eleitoresAtualizar();
            }
            
            text_eleitores.SetText(textToDisplayEleitores);
         }
         
         //atualiza texto bairros
         public void AtualizaBairros(){
            playerStats.bairrosAtualizar();
            text_bairros.SetText(" Bairros: "+playerStats.bairrosTotais.ToString());
         }
        
        //quando o jogador distribui seus eleitores
         public void contagemEleitores(){
            playerStats.eleitoresNovos--;
            text_eleitoresNovos.SetText(playerStats.eleitoresNovos.ToString());
            //diminui valor eleitores na hud player
            AtualizaEleitores();
         }
        public void AtualizaUIAposDistribuicao(){
                distribuaEleitorUi.SetActive(false);
                distribuicaoGeral=false;
                if(playerDiminuiEleitor==true){
                    acaboudistribuicaoUi2.SetActive(true);
                    playerDiminuiEleitor=false;
                }else{
                    acaboudistribuicaoUi.SetActive(true);
                }
                
                if(projeto.distribuicaoProjeto==true){
                    projeto.distribuicaoProjeto=false;
                    playerRecebeEleitor=true;
                    setUpZona.ChamarReseteBairroNaZona();
                    
                }
        }

        //para o bairro acessar quantos eleitores novos podem ser distribuidos
         public void valorEleitorNovo(){
            eleitoresNovosAtual = playerStats.eleitoresNovos;
         }

         //botao chama funcao de distribuicao de eleitor no inicio das rodadas
         public void ChamatPlayerInicioRodada(){
            if(distribuicaoInicial==true){
                //se for state do inicio
                playerStats.inicioRodada();
                distribuaEleitorUi.SetActive(true);
                distribuicaoGeral=true;
                text_distribuaEleitor.SetText("Distribua seus eleitores");
                text_eleitoresNovos.SetText(playerStats.eleitoresNovos.ToString());
            }
            
         }

         public void ValorEleitoresNovos(int valor){
            if(playerRecebeEleitor==true){
                playerStats.eleitoresNovos=valor;
                playerRecebeEleitor=false;
                //
                Debug.Log("eleitores novos: "+playerStats.eleitoresNovos);
                distribuaEleitorUi.SetActive(true);
                distribuicaoGeral=true;
                if(playerDiminuiEleitor==true){
                    text_distribuaEleitor.SetText("Retire seus eleitores");
                }else{
                    text_distribuaEleitor.SetText("Distribua seus eleitores");
                }
                text_eleitoresNovos.SetText(playerStats.eleitoresNovos.ToString());
            }
            
         }
    }

}
