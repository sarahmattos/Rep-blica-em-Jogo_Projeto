using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Game.managers;
using TMPro;
using Game.Player;
using Game.Tools;
using Logger = Game.Tools.Logger;

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

        private void Start()
        {
            FindingLocalPlayerStats();
            //playerStats.initializeStats += InitializeHudStats;
            button.GetComponent<Button>().onClick.AddListener(() => { TurnManager.Instance.NextTurnServerRpc(); });

            //fui();

        }

        //public void fui()
        //{
        //    TurnManager.Instance.isLocalPlayerTurn += (bool value) => { button.SetActive(value); };

        //    if (TurnManager.Instance.IsCurrent)
        //    {
        //        button.SetActive(true);
        //    }
        //    NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;


        //}
        //private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
        //{

        //    switch (sceneEvent.SceneEventType)
        //    {

        //        case SceneEventType.Load:
        //            {
        //                if (sceneEvent.SceneName == GameDataconfig.Instance.GameSceneName)
        //                {
        //                    Logger.Instance.LogInfo("entrou no scenemanager pelo hud stat");

        //                }
        //                break;
        //            }
        //    };
        //}


        public override void OnDestroy()
        {
            base.OnDestroy();
            TurnManager.Instance.isLocalPlayerTurn -= (bool value) => { button.SetActive(value); };
            //playerStats.initializeStats -= InitializeHudStats;
            //NetworkManager.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;

        }



        private void FindingLocalPlayerStats()
        {
            PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            foreach (PlayerStats stats in allPlayerStats)
            {
                if (stats.playerID == (int)OwnerClientId)
                {
                    playerStats = stats;
                    InitializeHudStats(playerStats);
                }
            }
        }

        private void InitializeHudStats(PlayerStats playerStats)
        {
            Logger.Instance.LogInfo("initialize huds");
            iconJogador.color = playerStats.Cor;
            text_nomeJogador.SetText(playerStats.Nome);
            text_eleitores.SetText(textToDisplayEleitores);
        }

    }

}
