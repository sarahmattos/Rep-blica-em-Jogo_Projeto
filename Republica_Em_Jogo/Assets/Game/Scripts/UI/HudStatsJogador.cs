using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Player;
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
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private TMP_Text text_saudeCarta;
        [SerializeField] private TMP_Text text_eduCarta;
        [SerializeField] private TMP_Text text_bairros;
        public int eduQuant, saudeQuant, bairroQuant; 
        public float eleitoresNovosAtual;
        public string textToDisplayEleitores => string.Concat("Eleitores: ", playerStats.EleitoresTotais);
        [SerializeField] private State state;

        void Update(){
            AtualizaEleitoresText();
        }
        public override void OnNetworkSpawn()
        {
            state = GameStateHandler.Instance.GameStatePairValue[GameState.INICIALIZACAO];
            GameStateHandler.Instance.GameStatePairValue[GameState.INICIALIZACAO].Entrada += FindingLocalPlayerStats;
            
            //button.GetComponent<Button>().onClick.AddListener(() => { TurnManager.Instance.NextTurnServerRpc(); });
        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.GameStatePairValue[GameState.INICIALIZACAO].Entrada -= FindingLocalPlayerStats;
        }


        //TODO: Remover quando n�o precisar mais dos bot�es
        public override void OnDestroy()
        {
            TurnManager.Instance.isLocalPlayerTurn -= (bool value) => { button.SetActive(value); };

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

        public void updateRecursoCartaUI(int quantidade)
        {
                Debug.Log(quantidade +"quantidade");
                if(quantidade>-1){
                    Debug.Log("entrou recurso");
                    playerStats.recursoDistribuicao(quantidade);
                    text_saudeCarta.SetText("Saúde: "+playerStats.numSaude.ToString());
                    text_eduCarta.SetText("Edu: "+playerStats.numEducacao.ToString());
                    saudeQuant=playerStats.numSaude;
                    eduQuant=playerStats.numEducacao;
                }
                
            
        }
        public void atualizarRecursoAposTroca(){
            playerStats.numSaude=saudeQuant;
            playerStats.numEducacao=eduQuant;
            text_saudeCarta.SetText("Saúde: "+playerStats.numSaude.ToString());
            text_eduCarta.SetText("Edu: "+playerStats.numEducacao.ToString());
        }

        public void AtualizarPlayerStatsBairro(){
            playerStats.bairrosTotais = bairroQuant;
            text_bairros.SetText("Bairros: "+playerStats.bairrosTotais.ToString());
        }

         public void AtualizaEleitoresText(){
            text_eleitores.SetText(textToDisplayEleitores);
         }
        
         public void valorEleitorNovo(){
            eleitoresNovosAtual = playerStats.eleitoresNovos;
         }
         public void atualizarEleitores(){
            
            playerStats.eleitoresNovos--;
            playerStats.eleitoresAtualizar();
         }
    }

}
