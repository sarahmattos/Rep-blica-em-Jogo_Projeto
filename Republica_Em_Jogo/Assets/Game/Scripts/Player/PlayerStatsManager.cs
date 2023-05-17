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
        public PlayerStats GetLocalPlayerStats()
        {
            foreach (PlayerStats playerStats in AllPlayerStats)
            {
                if (playerStats.IsLocalPlayer)
                {
                    return playerStats;
                }
            }
            // Tools.Logger.Instance.LogError("Falha ao enviar o PlayerStats do player local como referência.");
            throw new NullReferenceException("Falha ao enviar o PlayerStats do player local");
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
                if (playerStats.playerID == TurnManager.Instance.PlayerAtual)
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
        }

        private void OnDestroy()
        {

            gameplayLoadState.Entrada -= FindAllPlayerStats;
        }

        public void FindAllPlayerStats()
        {
            allPlayerStats = FindObjectsOfType<PlayerStats>();
            
        }



    }
}
