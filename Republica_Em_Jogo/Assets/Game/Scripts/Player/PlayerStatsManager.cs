using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Tools;
using UnityEngine;

namespace Game.Player
{
    public class PlayerStatsManager : Singleton<PlayerStatsManager>
    {
        private State gameplayLoadState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.GAMEPLAY_SCENE_LOAD);
        private PlayerStats[] allPlayerStats;
        public PlayerStats[] AllPlayerStats => allPlayerStats;
        public PlayerStats GetLocalPlayerStats() {
            foreach(PlayerStats playerStats in AllPlayerStats) {
                if(playerStats.IsLocalPlayer) {
                    return playerStats;
                }
            }
            Tools.Logger.Instance.LogError("Falha ao enviar o PlayerStats do player local como referência.");

            return null;
        }
            
        
        public PlayerStats GetPlayerStatsDoPlayerAtual()
        {
            foreach (PlayerStats playerStats in AllPlayerStats)
            {
                if (playerStats.playerID == TurnManager.Instance.PlayerAtual)
                {
                    return playerStats;
                }
            }
            Tools.Logger.Instance.LogError("Falha ao enviar o PlayerStats do player atual como referência.");
            return null;
        }

        private void Start()
        {
            gameplayLoadState.Saida += FindAllPlayerStats;
        }

        private void OnDestroy()
        {
            gameplayLoadState.Saida -= FindAllPlayerStats;
        }

        private void FindAllPlayerStats()
        {
            allPlayerStats = FindObjectsOfType<PlayerStats>();
        }
    }
}
