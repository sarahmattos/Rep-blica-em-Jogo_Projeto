using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Game.managers;
using TMPro;
using Game.Player;
using Unity.Collections;

namespace Game.UI
{
    public class HudStatsJogador : NetworkBehaviour
    {
        
        public GameObject button; //botao para testar

        [SerializeField] private Image iconJogador;
        [SerializeField] private TMP_Text text_nomeJogador;
        [SerializeField] private TMP_Text text_eleitores;
        [SerializeField] private PlayerStats playerStats;
        public string textToDisplayEleitores => string.Concat("Eleitores: ", playerStats.Eleitores);
        

        public override void OnNetworkSpawn()
        {
            TurnManager.Instance.isLocalPlayerTurn += (bool value) => { button.SetActive(value); };
            FindingLocalPlayerStats();
            if (TurnManager.Instance.IsCurrent)
            {
                button.SetActive(true);
               
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            TurnManager.Instance.isLocalPlayerTurn -= (bool value) => { button.SetActive(value); };
            playerStats.initializeStats -= InitializeHudStats;

        }

        private void Start()
        {
            button.GetComponent<Button>().onClick.AddListener(() => { TurnManager.Instance.NextTurnServerRpc(); });
            playerStats.initializeStats += InitializeHudStats;
            InitializeHudStats(playerStats);

        }
        public void nameChange()
        {
            text_nomeJogador.SetText(playerStats.Nome);
        }
        private void FindingLocalPlayerStats()
        {
            PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            foreach (PlayerStats stats in allPlayerStats)
            {
                if (stats.playerID == (int)OwnerClientId)
                {
                    playerStats = stats;
                }
            }
        }

        private void InitializeHudStats(PlayerStats playerStats)
        {
            iconJogador.color = playerStats.Cor;
            text_nomeJogador.SetText("Jogador: "+ NetworkManager.Singleton.LocalClientId.ToString());
            //testfloat.Value = playerStats.Nome;
            text_eleitores.SetText(textToDisplayEleitores);
        }
        
}

}
