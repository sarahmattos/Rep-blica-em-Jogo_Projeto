using System;
using System.Collections.Generic;
using System.Linq;
using Game.Tools;
using Unity.Netcode;
using UnityEngine;

namespace Game.Player
{
    public class PlayerStatsManager : NetworkSingleton<PlayerStatsManager>
    {
        private State gameplayLoadState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.GAMEPLAY_SCENE_LOAD);
        private List<PlayerStats> allPlayerStats = new List<PlayerStats>();
        public List<PlayerStats> AllPlayerStats => allPlayerStats;
        public PlayerStats GetLocalPlayerStats()
        {
            foreach (PlayerStats playerStats in AllPlayerStats)
            {
                if (playerStats.IsLocalPlayer)
                {
                    return playerStats;
                }
            }
            return new PlayerStats();
            // Tools.Logger.Instance.LogError("Falha ao enviar o PlayerStats do player local como referência.");
            //throw new NullReferenceException("Falha ao enviar o PlayerStats do player local");
        }

        public PlayerStats GetPlayerStats(int playerID)
        {
            foreach (PlayerStats playerStats in AllPlayerStats)
            {
                if (playerStats.playerID == playerID)
                    return playerStats;
            }
            throw new ArgumentOutOfRangeException("PlayerID parameter nao encontrado");
        }


        public PlayerStats GetPlayerStatsDoPlayerAtual()
        {
            foreach (PlayerStats playerStats in AllPlayerStats)
            {
                if (playerStats.playerID == TurnManager.Instance.PlayerAtualID)
                {
                    return playerStats;
                }
            }

            return new PlayerStats();

            throw new ArgumentOutOfRangeException("PLayer Stats atual ainda não definido.");
        }

        private void Start()
        {
            gameplayLoadState.Entrada += FindAllPlayerStats;
            TurnManager.Instance.PlayerRemovido += RemovePlayerStats;
        }

        public override void OnDestroy()
        {

            gameplayLoadState.Entrada -= FindAllPlayerStats;
            TurnManager.Instance.PlayerRemovido -= RemovePlayerStats;

            base.OnDestroy();
        }

        public void FindAllPlayerStats()
        {
            allPlayerStats = FindObjectsOfType<PlayerStats>().ToList();

        }

        public void RemovePlayerStats(PlayerStats playerStats)
        {
            allPlayerStats.Remove(playerStats);
        }

        public void RemovePlayerStats(int playerID)
        {
            for (int i = 0; i < allPlayerStats.Count; i++)
            {
                PlayerStats playerStats = allPlayerStats[i];
                if (playerStats.playerID == playerID)
                {
                    allPlayerStats.Remove(playerStats);
                    Tools.Logger.Instance.LogInfo("Local id in player stats list encontrado e sendo removido");
                    return;
                }
            }
        }



    }
}
