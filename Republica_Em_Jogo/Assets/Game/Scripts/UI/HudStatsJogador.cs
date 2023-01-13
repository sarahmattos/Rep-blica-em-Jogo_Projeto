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
        public int eduQuant, saudeQuant;
        public string textToDisplayEleitores => string.Concat("Eleitores: ", playerStats.EleitoresTotais);


        //TODO: Remover quando n�o precisar mais dos bot�es
        private void Start()
        {
            //eduQuant=20;
            //saudeQuant=16;
            //button.GetComponent<Button>().onClick.AddListener(() => { TurnManager.Instance.NextTurnServerRpc(); });
        }

        public override void OnNetworkSpawn()
        {
            GameStateHandler.Instance.initializeTerritorio += FindingLocalPlayerStats;
            //button.GetComponent<Button>().onClick.AddListener(() => { TurnManager.Instance.NextTurnServerRpc(); });
        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.initializeTerritorio -= FindingLocalPlayerStats;
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
            //if(playerStats.playerID==idTurno){
                Debug.Log(quantidade +"quantidade");
                if(quantidade>-1){
                    Debug.Log("entrou recurso");
                    playerStats.recursoDistribuicao(quantidade);
                text_saudeCarta.SetText("Saúde: "+playerStats.numSaude.ToString());
                text_eduCarta.SetText("Edu: "+playerStats.numEducacao.ToString());
                saudeQuant=playerStats.numSaude;
                eduQuant=playerStats.numEducacao;
                }
                
            //}
            
        }
        public void atualizarRecursoAposTroca(){
            playerStats.numSaude=saudeQuant;
            playerStats.numEducacao=eduQuant;
            text_saudeCarta.SetText("Saúde: "+playerStats.numSaude.ToString());
            text_eduCarta.SetText("Edu: "+playerStats.numEducacao.ToString());
        }
        
    }

}
